using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : AbstractSingleton<PlayerManager>
{
    public static readonly int MAX_PLAYERS = 2;
    //public static readonly Color[] PLAYER_COLORS =
    //{
    //    Color.red,
    //    Color.blue,
    //    Color.yellow,
    //    Color.green,
    //    Color.magenta,
    //    Color.cyan,
    //    Color.white,
    //    Color.black
    //};
    //public readonly Color INACTIVE_COLOR = Color.gray;

    [Header("Players")]
    public Player localPlayer;
    public List<Player> allPlayers;
    public List<Player> activePlayers;

    public void RemovePlayers()
    {
        foreach (Player item in allPlayers)
        {
            Destroy(item.gameObject);
        }
        localPlayer = null;
        allPlayers.Clear();
        activePlayers.Clear();
    }

    public void InstantiatePlayers(List<PlayerData> data)
    {
        RemovePlayers();

        for (int i = 0; i < data.Count; i++)
        {
            InstantiatePlayer(data[i], i + 1);
        }

        activePlayers = new List<Player>(allPlayers);

        if (!localPlayer)
        {
            Debug.LogWarning("The Local Player was set to the first player as a safety.");
            localPlayer = allPlayers[0];
        }
    }

    public void InstantiatePlayer(PlayerData playerData, int id)
    {
        DBContentHandler<DB_Color> dbColors = DBHandler_Color.Instance;

        Player prefab = AllPrefabs.Instance.player;

        Player newPlayer = Instantiate(prefab, transform);
        newPlayer.Initialize(playerData, id, dbColors.Select(playerData.colorId));
        allPlayers.Add(newPlayer);
    }

    public void RunAIPlayers()
    {
        foreach (Player p in allPlayers)
        {
            p.aiPersonality?.RunAI();
        }
    }

    //public void InactivatePlayer(Player p)
    //{
    //    p.type = PlayerType.INACTIVE;
    //    p.color = INACTIVE_COLOR;
    //      p.EndTurn();
    //      allActivePlayers.remove(p);
    //    Debug.Log("Player " + p.name + " is now inactive!");
    //}

    //public List<Color> GetAvailableColors()
    //{
    //    List<Color> result = new List<Color>(PLAYER_COLORS);
    //    foreach (var item in allPlayers)
    //    {
    //        result.Remove(item.color);
    //    }
    //    return result;
    //}

    public Player EndTurnForPlayer(Player player)
    {
        player.EndTurn();
        foreach (Player pl in activePlayers)
        {
            if (pl.HasTurn()) return pl;
        }
        return null;
    }

    public void RefreshTurnForActivePlayers(int currentTurn)
    {
        foreach (var pl in activePlayers)
        {
            pl.RefreshTurn(currentTurn);
        }
    }
}
