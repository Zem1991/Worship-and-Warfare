using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMap : AbstractMap<FieldTile>
{
    public override void Create(Vector2Int size)
    {
        Remove();
        mapSize = size;
        FieldTile prefabTile = AllPrefabs.Instance.fieldTile;

        int current = 0;
        for (int row = 0; row < mapSize.y; row++)
        {
            for (int col = 0; col < mapSize.x; col++)
            {
                Vector3 pos = new Vector3(col, 0, row);
                Quaternion rot = Quaternion.identity;

                Vector2Int tileId = new Vector2Int(col, row);
                FieldTile tile = Instantiate(prefabTile, pos, rot, transform);
                tiles.Add(tileId, tile);

                tile.id = current;
                tile.posId = tileId;
                tile.name = "Tile #" + current + " " + tileId.ToString();
                current++;

                Vector2Int neighbourId;
                FieldTile neighbour;
                if (col > 0)
                {
                    neighbourId = new Vector2Int(col - 1, row);
                    neighbour = tiles[neighbourId];
                    tile.l = neighbour;
                    neighbour.r = tile;
                }
                if (row > 0)
                {
                    neighbourId = new Vector2Int(col, row - 1);
                    neighbour = tiles[neighbourId];
                    tile.b = neighbour;
                    neighbour.f = tile;
                }
                if (col > 0 && row > 0)
                {
                    neighbourId = new Vector2Int(col - 1, row - 1);
                    neighbour = tiles[neighbourId];
                    tile.bl = neighbour;
                    neighbour.fr = tile;
                }
                if (col < mapSize.x - 1 && row > 0)
                {
                    neighbourId = new Vector2Int(col + 1, row - 1);
                    neighbour = tiles[neighbourId];
                    tile.br = neighbour;
                    neighbour.fl = tile;
                }
            }
        }
    }

    public void ApplyMapData(MapData mapData)
    {
        DatabaseManager db = DatabaseManager.Instance;
        DBHandler_Tileset tilesets = db.tilesets;

        int current = 0;
        foreach (var tile in tiles.Values)
        {
            TileData tileData = mapData.tiles[current];
            tile.lowerLandId = tileData.lowerLandId;
            tile.upperLandId = tileData.upperLandId;
            tile.waterId = tileData.waterId;
            tile.featureId = tileData.featureId;
            tile.roadId = tileData.roadId;

            DB_Tileset lowerLand = tilesets.Select(tile.lowerLandId) as DB_Tileset;
            Sprite s = lowerLand.image;
            tile.db_tileset_lowerLand = lowerLand;
            tile.ChangeLandSprite(s);

            tile.groundMovementCost = lowerLand.groundMovementCost;
            tile.allowGroundMovement = lowerLand.allowGroundMovement;
            tile.allowWaterMovement = lowerLand.allowWaterMovement;
            tile.allowLavaMovement = lowerLand.allowLavaMovement;

            DB_Tileset feature = tilesets.Select(tile.featureId, false) as DB_Tileset;
            if (feature)
            {
                s = feature.image;
                tile.db_tileset_feature = feature;
                tile.ChangeFeatureSprite(s);

                tile.groundMovementCost += feature.groundMovementCost;
                tile.allowGroundMovement &= feature.allowGroundMovement;
                tile.allowWaterMovement &= feature.allowWaterMovement;
                tile.allowLavaMovement &= feature.allowLavaMovement;
            }

            current++;
        }
    }
}
