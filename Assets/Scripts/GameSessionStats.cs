using UnityEngine;
using Platformer.Mechanics;

/// <summary>
/// Tracks per-player statistics during a single level/session.
/// Includes jumps, deaths, and collectibles for each player.
/// </summary>
public class GameSessionStats : MonoBehaviour
{
    public static GameSessionStats Instance;

    [System.Serializable]
    public class PlayerStats
    {
        public PlayerController controller;
        public int jumps = 0;
        public int deaths = 0;
        public int collectibles = 0;
        public bool finished = false;

        // compute score: jumps + collectibles + bonus if finished
        public int Score => jumps + collectibles + (finished ? 10 : 0);
    }

    private PlayerStats player1Stats = new PlayerStats();
    private PlayerStats player2Stats = new PlayerStats();

    private bool player1Finished = false;
    private bool player2Finished = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(PlayerController p1, PlayerController p2)
    {
        player1Stats.controller = p1;
        player2Stats.controller = p2;
        ResetStats();
    }

    public void ResetStats()
    {
        player1Stats.jumps = player1Stats.deaths = player1Stats.collectibles = 0;
        player1Stats.finished = false;

        player2Stats.jumps = player2Stats.deaths = player2Stats.collectibles = 0;
        player2Stats.finished = false;

        player1Finished = player2Finished = false;
    }

    public void MarkPlayerFinished(PlayerController pc)
    {
        if (pc == player1Stats.controller)
        {
            player1Finished = true;
            player1Stats.finished = true;
        }
        else if (pc == player2Stats.controller)
        {
            player2Finished = true;
            player2Stats.finished = true;
        }
    }

    public bool BothPlayersFinished()
    {
        if (player2Stats.controller == null) // single-player
            return player1Finished;
        return player1Finished && player2Finished;
    }

    public void AddJump(PlayerController player)
    {
        if (player == player1Stats.controller)
            player1Stats.jumps++;
        else if (player == player2Stats.controller)
            player2Stats.jumps++;
    }

    public void AddDeath(PlayerController player)
    {
        if (player == player1Stats.controller)
            player1Stats.deaths++;
        else if (player == player2Stats.controller)
            player2Stats.deaths++;
    }

    public void AddCollectible(PlayerController player)
    {
        if (player == player1Stats.controller)
            player1Stats.collectibles++;
        else if (player == player2Stats.controller)
            player2Stats.collectibles++;
    }

    public PlayerStats GetPlayerStats(PlayerController player)
    {
        if (player == player1Stats.controller)
            return player1Stats;
        else if (player == player2Stats.controller)
            return player2Stats;
        return null;
    }

    public PlayerStats GetPlayer1Stats() => player1Stats;
    public PlayerStats GetPlayer2Stats() => player2Stats;
}
