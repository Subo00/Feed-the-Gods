
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersHandler : MonoBehaviour
{
    public static PlayersHandler Instance;
    [SerializeField] private Transform[] SpawnPoints;
    private int playerCount = 0;
    private List<Player> players = new List<Player>();

    private void Start()
    {
        if(Instance != null) Destroy(this.gameObject);
        Instance = this;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.transform.position = SpawnPoints[playerCount].transform.position;
        playerCount++;
        players.Add(playerInput.GetComponent<Player>());
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
}
