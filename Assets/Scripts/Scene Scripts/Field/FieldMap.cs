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

        int maxX = mapSize.x - 1;
        int maxY = mapSize.y - 1;
        int current = 0;
        int rowIdFix = 0;
        for (int row = maxY; row >= 0; row--)
        {
            for (int col = 0; col < mapSize.x; col++)
            {
                Vector3 pos = new Vector3(col, 0, row);
                Quaternion rot = Quaternion.identity;

                Vector2Int tileId = new Vector2Int(col, rowIdFix);
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
                    neighbourId = new Vector2Int(col - 1, rowIdFix);
                    neighbour = tiles[neighbourId];
                    tile.l = neighbour;
                    neighbour.r = tile;
                }
                if (row < maxY)
                {
                    neighbourId = new Vector2Int(col, rowIdFix - 1);
                    neighbour = tiles[neighbourId];
                    tile.f = neighbour;
                    neighbour.b = tile;
                }
                if (col > 0 && row < maxY)
                {
                    neighbourId = new Vector2Int(col - 1, rowIdFix - 1);
                    neighbour = tiles[neighbourId];
                    tile.fl = neighbour;
                    neighbour.br = tile;
                }
                if (col < maxX && row < maxY)
                {
                    neighbourId = new Vector2Int(col + 1, rowIdFix - 1);
                    neighbour = tiles[neighbourId];
                    tile.fr = neighbour;
                    neighbour.bl = tile;
                }
            }
            rowIdFix++;
        }
    }

    public void ApplyMapData(MapData mapData)
    {
        DBContentHandler<DB_Tileset> dbTilesets = DBHandler_Tileset.Instance;

        int current = 0;
        foreach (var tile in tiles.Values)
        {
            TileData tileData = mapData.tiles[current];
            tile.lowerLandId = tileData.lowerLandId;
            tile.upperLandId = tileData.upperLandId;
            tile.waterId = tileData.waterId;
            tile.featureId = tileData.featureId;
            tile.roadId = tileData.roadId;

            if (tile.lowerLandId != null)
            {
                DB_Tileset lowerLand = dbTilesets.Select(tile.lowerLandId) as DB_Tileset;

                Sprite s = lowerLand.image;
                tile.db_tileset_lowerLand = lowerLand;
                tile.ChangeLandSprite(s);

                tile.groundMovementCost = lowerLand.groundMovementCost;
                tile.allowGroundMovement = lowerLand.allowGroundMovement;
                tile.allowWaterMovement = lowerLand.allowWaterMovement;
                tile.allowLavaMovement = lowerLand.allowLavaMovement;
            }

            if (tile.waterId != null)
            {
                DB_Tileset water = dbTilesets.Select(tile.waterId) as DB_Tileset;

                Sprite s = water.image;
                tile.db_tileset_feature = water;
                tile.ChangeFeatureSprite(s);

                tile.groundMovementCost += water.groundMovementCost;
                tile.allowGroundMovement &= water.allowGroundMovement;
                tile.allowWaterMovement &= water.allowWaterMovement;
                tile.allowLavaMovement &= water.allowLavaMovement;
            }

            if (tile.featureId != null)
            {
                DB_Tileset feature = dbTilesets.Select(tile.featureId) as DB_Tileset;

                Sprite s = feature.image;
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
