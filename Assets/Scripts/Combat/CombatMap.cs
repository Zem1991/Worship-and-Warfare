using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMap : MonoBehaviour
{
    public const int WIDTH = 15;
    public const int HEIGHT = 9;

    [Header("Data")]
    public List<CombatTile> tiles;

    public void Remove()
    {
        if (tiles != null)
        {
            foreach (var item in tiles)
            {
                Destroy(item);
            }
            tiles = null;
        }
    }

    public void Create(TileData tileData)
    {
        Remove();
        tiles = new List<CombatTile>();

        Vector3 startPos = new Vector3();
        startPos.x = (WIDTH - 1) / -2F;
        startPos.z = (HEIGHT - 1) / -2F;

        int current = 0;
        CombatTile previousRowReference = null;
        CombatTile previousRowReferenceLeft = null;
        for (int row = 0; row < HEIGHT; row++)
        {
            CombatTile firstOfCurrentRow = null;
            CombatTile previousInCurrentRow = null;

            int maxCol = WIDTH;
            float colPosAdjust = 0;
            bool oddCol = row % 2 == 1;
            if (oddCol)
            {
                maxCol++;
                colPosAdjust = -0.5F;
            }

            for (int col = 0; col < maxCol; col++)
            {
                CombatTile prefab = CombatManager.Instance.prefabTile;
                Vector3 pos = startPos;
                pos.x += col + colPosAdjust;
                pos.z += row;
                Quaternion rot = Quaternion.identity;

                CombatTile newTile = Instantiate(prefab, pos, rot, transform);
                tiles.Add(newTile);

                newTile.id = current;
                newTile.rowId = row;
                newTile.colId = col;
                newTile.name = "Tile #" + newTile.id + " (" + newTile.colId + "; " + newTile.rowId + ")";
                current++;

                if (!firstOfCurrentRow)
                {
                    firstOfCurrentRow = newTile;
                }

                if (previousInCurrentRow) previousInCurrentRow.r = newTile;
                newTile.l = previousInCurrentRow;

                if (previousRowReference)
                {
                    if (oddCol)
                    {
                        newTile.br = previousRowReference;
                        newTile.bl = previousRowReference.l;
                        newTile.br.fl = newTile;
                        if (newTile.bl) newTile.bl.fr = newTile;
                    }
                    else
                    {
                        newTile.bl = previousRowReference;
                        newTile.br = previousRowReference.r;
                        newTile.bl.fr = newTile;
                        newTile.br.fl = newTile;
                    }
                    previousRowReferenceLeft = previousRowReference;
                    previousRowReference = previousRowReference.r;
                }
                else if (previousRowReferenceLeft)
                {
                    newTile.bl = previousRowReferenceLeft;
                    newTile.bl.fr = newTile;
                }

                previousInCurrentRow = newTile;

                if (col == maxCol - 1)
                {
                    previousRowReference = firstOfCurrentRow;
                    previousRowReferenceLeft = firstOfCurrentRow.l;
                }
            }
        }
    }
}
