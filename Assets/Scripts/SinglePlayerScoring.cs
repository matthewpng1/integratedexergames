using UnityEngine;

/// <summary>
/// Utility class for calculating score bonuses in single player mode.
/// Uses difficulty-based bonuses that are applied per completed round.
/// </summary>
public static class SinglePlayerScoring
{
    /// <summary>
    /// Calculates the score bonus for a difficulty level.
    /// Rules:
    /// - Easy: 0 points
    /// - Medium: 10 points
    /// - Hard: 20 points
    /// </summary>
    /// <param name="difficulty">Difficulty level (0=Easy, 1=Medium, 2=Hard).</param>
    /// <returns>The bonus for a single round at this difficulty.</returns>
    public static int GetDifficultyBonus(int difficulty)
    {
        switch (difficulty)
        {
            case 0: return 0;    // Easy
            case 1: return 10;   // Medium
            case 2: return 20;   // Hard
            default: return 0;
        }
    }

    /// <summary>
    /// Gets the gripper difficulty bonus from stored profile.
    /// </summary>
    /// <param name="username">Username to look up.</param>
    /// <returns>The bonus per gripper round.</returns>
    public static int GetGripperBonus(string username)
    {
        int gripperDifficulty = PlayerPrefs.GetInt($"GYG_Profile:{username}:gripperDifficulty", 0);
        return GetDifficultyBonus(gripperDifficulty);
    }

    /// <summary>
    /// Gets the ankle difficulty bonus from stored profile.
    /// </summary>
    /// <param name="username">Username to look up.</param>
    /// <returns>The bonus per ankle round.</returns>
    public static int GetAnkleBonus(string username)
    {
        int ankleDifficulty = PlayerPrefs.GetInt($"GYG_Profile:{username}:ankleDifficulty", 0);
        return GetDifficultyBonus(ankleDifficulty);
    }

    /// <summary>
    /// Gets the total bonus earned so far (gripper rounds + ankle rounds completed).
    /// </summary>
    /// <param name="username">Username to look up.</param>
    /// <returns>Total bonus earned so far.</returns>
    public static int GetTotalBonusEarned(string username)
    {
        int gripperBonus = GetTotalGripperBonusEarned(username);
        int ankleBonus = GetTotalAnkleBonusEarned(username);
        int totalBonus = gripperBonus + ankleBonus;
        Debug.Log($"SinglePlayerScoring: GetTotalBonusEarned({username}) - gripperBonus={gripperBonus} + ankleBonus={ankleBonus} = {totalBonus}");
        return totalBonus;
    }

    /// <summary>
    /// Gets the total gripper bonus earned so far (based on completed gripper rounds).
    /// </summary>
    /// <param name="username">Username to look up.</param>
    /// <returns>Total gripper bonus earned so far.</returns>
    public static int GetTotalGripperBonusEarned(string username)
    {
        int gripperRounds = PlayerPrefs.GetInt($"GYG_Progress:{username}:gripperRounds", 0);
        int gripperBonus = GetGripperBonus(username);
        int total = gripperRounds * gripperBonus;
        Debug.Log($"SinglePlayerScoring: GetTotalGripperBonusEarned({username}) - gripperRounds={gripperRounds} * gripperBonus={gripperBonus} = {total}");
        return total;
    }

    /// <summary>
    /// Gets the total ankle bonus earned so far (based on completed ankle rounds).
    /// </summary>
    /// <param name="username">Username to look up.</param>
    /// <returns>Total ankle bonus earned so far.</returns>
    public static int GetTotalAnkleBonusEarned(string username)
    {
        int ankleRounds = PlayerPrefs.GetInt($"GYG_Progress:{username}:ankleRounds", 0);
        int ankleBonus = GetAnkleBonus(username);
        int total = ankleRounds * ankleBonus;
        Debug.Log($"SinglePlayerScoring: GetTotalAnkleBonusEarned({username}) - ankleRounds={ankleRounds} * ankleBonus={ankleBonus} = {total}");
        return total;
    }

    /// <summary>
    /// Increments gripper rounds completed and returns new total bonus earned.
    /// </summary>
    /// <param name="username">Username to update.</param>
    /// <returns>New total bonus.</returns>
    public static int CompleteGripperRound(string username)
    {
        int gripperRounds = PlayerPrefs.GetInt($"GYG_Progress:{username}:gripperRounds", 0);
        int gripperBonus = GetGripperBonus(username);
        int totalBonusBefore = GetTotalBonusEarned(username);
        
        gripperRounds++;
        PlayerPrefs.SetInt($"GYG_Progress:{username}:gripperRounds", gripperRounds);
        PlayerPrefs.Save();
        
        int totalBonusAfter = GetTotalBonusEarned(username);
        Debug.Log($"[***BONUS ADDED***] SinglePlayerScoring.CompleteGripperRound({username}): gripperRounds: {gripperRounds - 1}->{gripperRounds}, gripperBonus={gripperBonus}, Total Bonus: {totalBonusBefore}->{totalBonusAfter} (added {gripperBonus})");
        
        return totalBonusAfter;
    }

    /// <summary>
    /// Increments ankle rounds completed and returns new total bonus earned.
    /// </summary>
    /// <param name="username">Username to update.</param>
    /// <returns>New total bonus.</returns>
    public static int CompleteAnkleRound(string username)
    {
        int ankleRounds = PlayerPrefs.GetInt($"GYG_Progress:{username}:ankleRounds", 0);
        int ankleBonus = GetAnkleBonus(username);
        int totalBonusBefore = GetTotalBonusEarned(username);
        
        ankleRounds++;
        PlayerPrefs.SetInt($"GYG_Progress:{username}:ankleRounds", ankleRounds);
        PlayerPrefs.Save();
        
        int totalBonusAfter = GetTotalBonusEarned(username);
        Debug.Log($"[***BONUS ADDED***] SinglePlayerScoring.CompleteAnkleRound({username}): ankleRounds: {ankleRounds - 1}->{ankleRounds}, ankleBonus={ankleBonus}, Total Bonus: {totalBonusBefore}->{totalBonusAfter} (added {ankleBonus})");
        
        return totalBonusAfter;
    }

    /// <summary>
    /// Resets round counters (e.g., on new day or logout).
    /// </summary>
    /// <param name="username">Username to reset.</param>
    public static void ResetRoundCounts(string username)
    {
        PlayerPrefs.SetInt($"GYG_Progress:{username}:gripperRounds", 0);
        PlayerPrefs.SetInt($"GYG_Progress:{username}:ankleRounds", 0);
        PlayerPrefs.Save();
        Debug.Log($"SinglePlayerScoring: Round counters reset for {username}");
    }
}
