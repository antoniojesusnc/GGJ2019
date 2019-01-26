using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonGameObject<GameManager>
{
    const int LevelsNumber = 5;
    const string KeyBestTime = "BestTime{0}";
    const string KeyUnlockLevel = "Unlock{0}";

    public int CurrentLevel { get; set; }

    public class BestTimeInfo
    {
        private int v;

        public BestTimeInfo(int levelNumber, float bestTime, bool unlock)
        {
            Level = levelNumber;
            BestTime = bestTime;
            Unlock = unlock;
        }

        public int Level { get; set; }
        public float BestTime { get; set; }
        public bool Unlock { get; set; }
    }

    List<BestTimeInfo> _bestTimes;

    void Awake()
    {
        LoadHighScores();
    }

    private void LoadHighScores()
    {
        _bestTimes = new List<BestTimeInfo>();

        for (int i = 0; i < LevelsNumber; ++i)
        {
            float bestTime = PlayerPrefs.GetFloat(string.Format(KeyBestTime, (i + 1)), float.MaxValue);


            bool complete = true;
            if (i > 0)
                complete = PlayerPrefs.GetInt(string.Format(KeyUnlockLevel, (i + 1)), 0) == 0 ? false : true;

            _bestTimes.Add(new BestTimeInfo(i + 1, bestTime, complete));
        }
    }

    public float GetBestTime(int level)
    {
        var temp = GetBestTimeInfo(level);
        if (temp == null)
        {
            Debug.LogWarning("Level Not found: " + level);
            return 0;
        }

        return temp.BestTime;
    }

    public void UpdateBestTime(int level, float bestTime)
    {
        var temp = GetBestTimeInfo(level);
        if (temp == null)
        {
            Debug.LogWarning("Level Not found: " + level);
            return;
        }

        temp.BestTime = bestTime;
        PlayerPrefs.SetFloat(string.Format(KeyBestTime, CurrentLevel), bestTime);
    }

    public bool IsLevelUnLock(int level)
    {
        var temp = GetBestTimeInfo(level);
        if (temp == null)
        {
            Debug.LogError("Level Not found: " + level);
            return false;
        }
        return temp.Unlock;
    }

    public void SetLevelAsUnlock(int level)
    {
        var temp = GetBestTimeInfo(level);
        if (temp == null)
        {
            Debug.LogError("Level Not found: " + level);
            return;
        }
        temp.Unlock = true;
        PlayerPrefs.GetInt(string.Format(KeyUnlockLevel, level), 1);
    }

    private BestTimeInfo GetBestTimeInfo(int level)
    {
        for (int i = _bestTimes.Count - 1; i >= 0; --i)
        {
            if (_bestTimes[i].Level == level)
                return _bestTimes[i];
        }
        return null;
    }

    public bool IsBestScore(int level, float newTime)
    {
        var bestTime = GetBestTimeInfo(level);
        return bestTime.BestTime > newTime;
    }
}
