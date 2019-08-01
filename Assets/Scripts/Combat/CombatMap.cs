using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMap : MonoBehaviour
{
    public const int WIDTH = 15;
    public const int HEIGHT = 9;

    [Header("Common Tiles")]
    public List<CombatTile> allTiles;
    public List<CombatTile> attackerStartTiles;
    public List<CombatTile> defenderStartTiles;

    [Header("Special Tiles")]
    public CombatTile attackerHeroTile;
    public CombatTile defenderHeroTile;

    public void Remove()
    {
        if (allTiles != null)
        {
            foreach (var item in allTiles)
            {
                Destroy(item);
            }
            allTiles = null;
        }

        if (attackerStartTiles != null)
        {
            foreach (var item in attackerStartTiles)
            {
                Destroy(item);
            }
            attackerStartTiles = null;
        }

        if (defenderStartTiles != null)
        {
            foreach (var item in defenderStartTiles)
            {
                Destroy(item);
            }
            defenderStartTiles = null;
        }
    }

    public void Create(TileData tileData)
    {
        Remove();

        allTiles = new List<CombatTile>();
        attackerStartTiles = new List<CombatTile>();
        defenderStartTiles = new List<CombatTile>();
        CombatTile prefabTile = CombatManager.Instance.prefabTile;

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
                Vector3 pos = startPos;
                pos.x += col + colPosAdjust;
                pos.z += row;
                Quaternion rot = Quaternion.identity;

                CombatTile newTile = Instantiate(prefabTile, pos, rot, transform);
                allTiles.Add(newTile);

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

                int lastCol = maxCol - 1;
                if (col == lastCol)
                {
                    previousRowReference = firstOfCurrentRow;
                    previousRowReferenceLeft = firstOfCurrentRow.l;
                }

                //Set attacker's start tiles
                if (col == 0) attackerStartTiles.Add(newTile);
                //Set defender's start tiles
                if (col == lastCol) defenderStartTiles.Add(newTile);
            }
        }

        Vector3 heroTilePos = startPos;
        heroTilePos.x -= 2.5F;
        heroTilePos.z += 6.5F;
        Quaternion heroTileRot = Quaternion.identity;
        attackerHeroTile = Instantiate(prefabTile, heroTilePos, heroTileRot, transform);

        heroTilePos.x *= -1;
        defenderHeroTile = Instantiate(prefabTile, heroTilePos, heroTileRot, transform);
    }
}
