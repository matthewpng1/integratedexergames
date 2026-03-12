using UnityEngine;
using TMPro;

/// <summary>
/// Displays player statistics on the victory/end game screen.
/// </summary>
public class VictoryResultsDisplay : MonoBehaviour
{
    [Header("Player 1 UI")]
    [SerializeField] private TMP_Text player1NameText;
    [SerializeField] private TMP_Text player1JumpsText;
    [SerializeField] private TMP_Text player1DeathsText;
    [SerializeField] private TMP_Text player1CollectiblesText;
    [SerializeField] private TMP_Text player1ScoreText;
    [SerializeField] private TMP_Text player1FinishedText;

    [Header("Player 2 UI")]
    [SerializeField] private TMP_Text player2NameText;
    [SerializeField] private TMP_Text player2JumpsText;
    [SerializeField] private TMP_Text player2DeathsText;
    [SerializeField] private TMP_Text player2CollectiblesText;
    [SerializeField] private TMP_Text player2ScoreText;
    [SerializeField] private TMP_Text player2FinishedText;

    void OnEnable()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        if (GameSessionStats.Instance == null)
        {
            Debug.LogWarning("GameSessionStats not found!");
            return;
        }

        var p1Stats = GameSessionStats.Instance.GetPlayer1Stats();
        var p2Stats = GameSessionStats.Instance.GetPlayer2Stats();

        // Update Player 1
        if (player1JumpsText != null)
            player1JumpsText.text = $"Jumps: {p1Stats.jumps}";
        if (player1DeathsText != null)
            player1DeathsText.text = $"Deaths: {p1Stats.deaths}";
        if (player1CollectiblesText != null)
            player1CollectiblesText.text = $"Collectibles: {p1Stats.collectibles}";
        if (player1ScoreText != null)
            player1ScoreText.text = $"Score: {p1Stats.Score}";
        if (player1FinishedText != null)
            player1FinishedText.text = $"Finished: {p1Stats.finished}";
        if (player1NameText != null)
            player1NameText.text = p1Stats.controller != null ? "Player 1" : "N/A";

        // Update Player 2
        if (p2Stats.controller != null)
        {
            if (player2NameText != null)
                player2NameText.text = "Player 2";
            if (player2JumpsText != null)
                player2JumpsText.text = $"Jumps: {p2Stats.jumps}";
            if (player2DeathsText != null)
                player2DeathsText.text = $"Deaths: {p2Stats.deaths}";
            if (player2CollectiblesText != null)
                player2CollectiblesText.text = $"Collectibles: {p2Stats.collectibles}";
            if (player2ScoreText != null)
                player2ScoreText.text = $"Score: {p2Stats.Score}";
            if (player2FinishedText != null)
                player2FinishedText.text = $"Finished: {p2Stats.finished}";
        }
        else
        {
            // Hide Player 2 UI if there's no second player
            if (player2NameText != null)
                player2NameText.gameObject.SetActive(false);
            if (player2JumpsText != null)
                player2JumpsText.gameObject.SetActive(false);
            if (player2DeathsText != null)
                player2DeathsText.gameObject.SetActive(false);
            if (player2CollectiblesText != null)
                player2CollectiblesText.gameObject.SetActive(false);
            if (player2FinishedText != null)
                player2FinishedText.gameObject.SetActive(false);
        }
    }
}
