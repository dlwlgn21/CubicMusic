using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public int[] Scores;

    void Start()
    {
        LoadScores();
    }
    public void SaveScores()
    {
        for (int i = 0; i < Scores.Length; ++i)
        {
            PlayerPrefs.SetInt($"Score{i}", Scores[i]);
        }
    }

    public void LoadScores()
    {
        for (int i = 0; i < Scores.Length; ++i)
        {
            if (PlayerPrefs.HasKey($"Score{i}"))
            {
                Scores[i] = PlayerPrefs.GetInt($"Score{i}");
            }
        }
    }
}
