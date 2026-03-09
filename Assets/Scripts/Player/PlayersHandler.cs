
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayersHandler : MonoBehaviour //For local MULTIPLAYER 
{
    public static PlayersHandler Instance;
    public Action<int> OnPlayerCountChange;
    [SerializeField] private Transform[] SpawnPoints;
    private int playerCount = 0;
    private List<Player> players = new List<Player>();
    public int GetPlayerCount() { return players.Count; }


    private void Awake()
    {
        if(Instance != null) Destroy(this.gameObject);
        Instance = this;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.transform.position = SpawnPoints[playerCount].transform.position;
        playerCount++;
        players.Add(playerInput.GetComponent<Player>());
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

}
