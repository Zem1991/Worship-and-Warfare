using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public readonly int MAX_PLAYERS = 2;
    public readonly Color[] PLAYER_COLORS =
    {
        Color.red,
        Color.blue,
        Color.yellow,
        Color.green,
        Color.magenta,
        Color.cyan,
        Color.white,
        Color.black
    };
    public readonly Color INACTIVE_COLOR = Color.gray;

    [Header("Prefabs")]
    public Player prefabPlayer;

    [Header("Players")]
    public Player localPlayer;
    public List<Player> allPlayers;

    public void InstantiatePlayers(PlayerData[] data)
    {
        foreach (var item in data)
        {
            InstantiatePlayer(item);
        }

        if (!localPlayer)
        {
            Debug.LogWarning("The Local Player was set to the first player as a safety.");
            localPlayer = allPlayers[0];
        }
    }

    private void InstantiatePlayer(PlayerData data)
    {
        Player newPlayer = Instantiate(prefabPlayer, transform);
        allPlayers.Add(newPlayer);

        newPlayer.color = PLAYER_COLORS[data.colorId];
        newPlayer.playerName = data.name;

        newPlayer.name = newPlayer.playerName;
    }

    //public void InactivatePlayer(Player p)
    //{
    //    p.type = PlayerType.INACTIVE;
    //    p.color = INACTIVE_COLOR;
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
}
