
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayersHandler : MonoBehaviour, IDataPersistence
{
    public static PlayersHandler Instance;
    public Action<int> OnPlayerCountChange;
    [SerializeField] private Transform[] SpawnPoints;
    private int playerCount = 0;
    private List<Player> players = new List<Player>();

    // Cached on LoadData, consumed per slot in OnPlayerJoined
    private Dictionary<int, PlayerData> savedPlayerData = new Dictionary<int, PlayerData>();

    public int GetPlayerCount() { return players.Count; }

    private void Awake()
    {
        if(Instance != null) Destroy(this.gameObject);
        Instance = this;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int index = playerCount;

        if (savedPlayerData.TryGetValue(index, out PlayerData saved))
        {
            playerInput.transform.position = saved.position;
            playerInput.transform.rotation = Quaternion.Euler(saved.rotation);
            playerInput.GetComponent<Player>().Inventory.LoadPlayerData(saved);
        }
        else
        {
            playerInput.transform.position = SpawnPoints[index].transform.position;
        }

        playerCount++;
        Player player = playerInput.GetComponent<Player>();
        player.SetPlayerIndex(index);
        players.Add(player);
        OnPlayerCountChange?.Invoke(playerCount);
    }

    public void DisconnectPlayer(Player player)
    {
        players.Remove(player);
        playerCount--;
        Destroy(player.transform.parent.gameObject);
        OnPlayerCountChange?.Invoke(playerCount);
    }

    public void DisablePlayers(Player activPlayer)
    {
        foreach (var player in players)
            player.SetControlEnable(false);
        
        activPlayer.SetControlEnable(true);
    }

    public void EnablePlayers()
    {
        foreach(var player in players)
            player.SetControlEnable(true);
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        players.Remove(playerInput.GetComponent<Player>());
        playerCount--;
        OnPlayerCountChange?.Invoke(playerCount);
    }

    /// <summary>
    /// Caches saved PlayerData entries so OnPlayerJoined can restore each
    /// player's position when they connect.
    /// </summary>
    public void LoadData(GameData data)
    {
        savedPlayerData.Clear();
        foreach (var kvp in data.players)
            savedPlayerData[kvp.Key] = kvp.Value;
    }

    /// <summary>
    /// Writes position and rotation for each connected player into GameData.
    /// Inventory is written separately by each player's InventoryManager.
    /// </summary>
    public void SaveData(ref GameData data)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null) continue;

            if (!data.players.ContainsKey(i))
                data.players[i] = new PlayerData();

            data.players[i].position = players[i].transform.position;
            data.players[i].rotation = players[i].transform.eulerAngles;

            players[i].Inventory.SavePlayerData(data.players[i]);
        }
    }
}
