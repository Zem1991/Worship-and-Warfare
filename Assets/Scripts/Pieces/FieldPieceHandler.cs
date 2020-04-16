using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPieceHandler : MonoBehaviour
{
    public readonly int MAX_UNITS = 5;

    [Header("Pieces")]
    public List<TownPiece2> townPieces = new List<TownPiece2>();
    public List<PartyPiece2> partyPieces = new List<PartyPiece2>();
    public List<AbstractPickupPiece2> pickupPieces = new List<AbstractPickupPiece2>();

    public void RemoveAll()
    {
        foreach (var item in townPieces) Destroy(item.gameObject);
        townPieces.Clear();

        foreach (var item in partyPieces) Destroy(item.gameObject);
        partyPieces.Clear();

        foreach (var item in pickupPieces) Destroy(item.gameObject);
        pickupPieces.Clear();
    }

    public void CreateAll(List<TownData> towns, List<PartyData> parties, List<PickupData> pickups)
    {
        RemoveAll();

        CreateTowns(towns);
        CreateParties(parties);
        CreatePickups(pickups);
    }

    private void CreateTowns(List<TownData> towns)
    {
        if (towns == null) return;

        AbstractDBContentHandler<DB_Faction> dbFactions = DBHandler_Faction.Instance;
        AbstractDBContentHandler<DB_TownBuilding> dbTownBuildings = DBHandler_TownBuilding.Instance;

        PlayerManager pm = PlayerManager.Instance;
        FieldManager fm = FieldManager.Instance;
        FieldMap fieldMap = fm.mapHandler.map;

        TownPiece2 prefabTownPiece = AllPrefabs.Instance.fieldTownPiece;
        Town prefabTown = AllPrefabs.Instance.town;
        Party prefabParty = AllPrefabs.Instance.party;

        foreach (var townData in towns)
        {
            int posX = townData.mapPosition[0];
            int posY = townData.mapPosition[1];

            Vector2Int tileId = new Vector2Int(posX, posY);
            FieldTile fieldTile = fieldMap.tiles[tileId];
            Vector3 pos = fieldTile.transform.position;
            Quaternion rot = Quaternion.identity;

            Player owner = pm.allPlayers[townData.ownerId - 1];

            string factionId = townData.factionId;
            DB_Faction dbFaction = dbFactions.Select(factionId);

            TownPiece2 newTownPiece = Instantiate(prefabTownPiece, pos, rot, transform);
            townPieces.Add(newTownPiece);

            Town town = Instantiate(prefabTown, newTownPiece.transform);
            town.Initialize(dbFaction, newTownPiece, townData.townName);

            Party garrisonParty = Instantiate(prefabParty, town.transform);
            garrisonParty.Initialize();
            garrisonParty.name = "Garrison";
            town.garrison = garrisonParty;

            foreach (var townBuildingData in townData.townBuildings)
            {
                string townBuildingId = townBuildingData.townBuildingId;
                DB_TownBuilding dbTownBuilding = dbTownBuildings.Select(townBuildingId);
                town.BuildStructure(dbTownBuilding, null);
            }

            newTownPiece.currentTile = fieldTile;
            newTownPiece.currentTile.occupantPiece = newTownPiece;
            newTownPiece.Initialize(owner, town);
        }
    }

    private void CreateParties(List<PartyData> parties)
    {
        if (parties == null) return;

        PlayerManager pm = PlayerManager.Instance;

        foreach (PartyData partyData in parties)
        {
            Vector2Int mapPostion = new Vector2Int(partyData.mapPosition[0], partyData.mapPosition[1]);
            Player owner = pm.allPlayers[partyData.ownerId - 1];
            PartyPiece2 newParty = CreateParty(mapPostion, partyData, owner);
            partyPieces.Add(newParty);
        }
    }

    private void CreatePickups(List<PickupData> pickups)
    {
        if (pickups == null) return;

        ResourcePickupPiece2 prefabResourcePiece = AllPrefabs.Instance.resourcePickupPiece;
        ArtifactPickupPiece2 prefabArtifactPiece = AllPrefabs.Instance.artifactPickupPiece;

        AbstractDBContentHandler<DB_Resource> dbResources = DBHandler_Resource.Instance;
        AbstractDBContentHandler<DB_Artifact> dbArtifacts = DBHandler_Artifact.Instance;

        FieldManager fm = FieldManager.Instance;
        FieldMap fieldMap = fm.mapHandler.map;

        foreach (var pData in pickups)
        {
            int posX = pData.mapPosition[0];
            int posY = pData.mapPosition[1];

            Vector2Int tileId = new Vector2Int(posX, posY);
            FieldTile fieldTile = fieldMap.tiles[tileId];
            Vector3 pos = fieldTile.transform.position;
            Quaternion rot = Quaternion.identity;

            AbstractPickupPiece2 newPiece = null;
            switch (pData.pickupType)
            {
                case PickupType.RESOURCE:
                    newPiece = CreateResourcePickup(prefabResourcePiece, pos, rot, dbResources, pData);
                    break;
                case PickupType.ARTIFACT:
                    newPiece = CreateArtifactPickup(prefabArtifactPiece, pos, rot, dbArtifacts, pData);
                    break;
            }
            newPiece.currentTile = fieldTile;
            newPiece.currentTile.occupantPiece = newPiece;
            pickupPieces.Add(newPiece);
        }
    }

    public PartyPiece2 CreateParty(Vector2Int mapPosition, PartyData partyData, Player owner)
    {
        FieldMap fieldMap = FieldManager.Instance.mapHandler.map;
        PartyPiece2 prefabPartyPiece = AllPrefabs.Instance.fieldPartyPiece;
        Party prefabParty = AllPrefabs.Instance.party;

        int posX = mapPosition.x;
        int posY = mapPosition.y;

        Vector2Int tileId = new Vector2Int(posX, posY);
        FieldTile fieldTile = fieldMap.tiles[tileId];
        Vector3 pos = fieldTile.transform.position;
        Quaternion rot = Quaternion.identity;

        PartyPiece2 newParty = Instantiate(prefabPartyPiece, pos, rot, transform);
        Party party = Instantiate(prefabParty, newParty.transform);
        party.Initialize(partyData);

        newParty.Initialize(owner, party);
        newParty.currentTile = fieldTile;
        newParty.currentTile.occupantPiece = newParty;
        Debug.Log("Party created: " + newParty);
        return newParty;
    }

    public void RemoveParty(PartyPiece2 piece)
    {
        partyPieces.Remove(piece);
        piece.currentTile.occupantPiece = null;
        Destroy(piece.gameObject);
    }

    public ResourcePickupPiece2 CreateResourcePickup(ResourcePickupPiece2 prefabPiece, Vector3 pos, Quaternion rot, AbstractDBContentHandler<DB_Resource> dbResources, PickupData pickupData)
    {
        ResourcePickupPiece2 newPiece = Instantiate(prefabPiece, pos, rot, transform);
        newPiece.Initialize(dbResources.Select(pickupData.resourceType.ToString()), pickupData.resourceAmount);
        return newPiece;
    }

    public ArtifactPickupPiece2 CreateArtifactPickup(ArtifactPickupPiece2 prefabPiece, Vector3 pos, Quaternion rot, AbstractDBContentHandler<DB_Artifact> dbArtifacts, PickupData pickupData)
    {
        ArtifactPickupPiece2 newPiece = Instantiate(prefabPiece, pos, rot, transform);
        newPiece.Initialize(dbArtifacts.Select(pickupData.artifactId));
        return newPiece;
    }

    public void RemovePickup(AbstractPickupPiece2 pickup)
    {
        pickupPieces.Remove(pickup);
        pickup.currentTile.occupantPiece = null;
        Destroy(pickup.gameObject);
    }

    public bool Pathfind(PartyPiece2 piece, FieldTile targetTile,
        bool needGroundAccess = true, bool needWaterAccess = false, bool needLavaAccess = false)
    {
        bool result = Pathfinder.FindPath(piece.currentTile, targetTile,
            Pathfinder.OctoHeuristic, needGroundAccess, needWaterAccess, needLavaAccess,
            out PathfindResults pathfindResults);
        piece.pieceMovement.SetPath(pathfindResults, targetTile);
        return result;
    }

    public List<PartyPiece2> GetIdlePieces(List<PartyPiece2> pieces)
    {
        List<PartyPiece2> result = new List<PartyPiece2>();
        foreach (var item in pieces)
        {
            if (item.ICP_IsIdle()) result.Add(item);
        }
        return result;
    }

    public List<PartyPiece2> GetPlayerPieces(Player player)
    {
        List<PartyPiece2> result = new List<PartyPiece2>();
        foreach (var item in partyPieces)
        {
            if (item.pieceOwner.GetOwner() == player) result.Add(item);
        }
        return result;
    }

    public IEnumerator YieldForIdlePieces(List<PartyPiece2> pieces)
    {
        while (GetIdlePieces(pieces).Count != pieces.Count)
        {
            yield return null;
        }
    }
}
