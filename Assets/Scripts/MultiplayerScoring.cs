using UnityEngine;

/// <summary>
/// Difficulty levels for exercises.
/// </summary>
public enum DifficultyLevel
{
    Easy = 0,    // 0 bonus
    Medium = 1,  // 10 bonus
    Hard = 2     // 20 bonus
}

/// <summary>
/// Types of exercises that affect the weight bonus thresholds.
/// </summary>
public enum MultiplayerExerciseType
{
    Gripper = 0,
    AnkleWeight = 1
}

/// <summary>
/// Utility class for calculating score bonuses in multiplayer mode.
/// Based on difficulty level.
/// </summary>
public static class MultiplayerScoring
{
    /// <summary>
    /// Calculates the score bonus for a player based on difficulty level.
    /// Rules:
    /// - Easy: 0 points
    /// - Medium: 10 points
    /// - Hard: 20 points
    /// </summary>
    /// <param name="difficulty">Difficulty level selected.</param>
    /// <returns>The total score bonus.</returns>
    public static int CalculateScoreBonus(DifficultyLevel difficulty)
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy: return 0;
            case DifficultyLevel.Medium: return 10;
            case DifficultyLevel.Hard: return 20;
            default: return 0;
        }
    }

    /// <summary>
    /// Gets the score bonus for Player 1 from stored data.
    /// </summary>
    /// <returns>The score bonus for Player 1.</returns>
    public static int GetPlayer1ScoreBonus()
    {
        int difficultyIndex = PlayerPrefs.GetInt("MultiplayerPlayer1Difficulty", 0); // Default Easy
        DifficultyLevel difficulty = (DifficultyLevel)difficultyIndex;
        return CalculateScoreBonus(difficulty);
    }

    /// <summary>
    /// Gets the score bonus for Player 2 from stored data.
    /// </summary>
    /// <returns>The score bonus for Player 2.</returns>
    public static int GetPlayer2ScoreBonus()
    {
        int difficultyIndex = PlayerPrefs.GetInt("MultiplayerPlayer2Difficulty", 0); // Default Easy
        DifficultyLevel difficulty = (DifficultyLevel)difficultyIndex;
        return CalculateScoreBonus(difficulty);
    }
}