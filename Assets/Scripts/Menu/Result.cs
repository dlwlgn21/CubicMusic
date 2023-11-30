using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Result : MonoBehaviour
{

    [SerializeField] GameObject GoUI = null;
    [SerializeField] Text[] TimingScoreTexts = null;
    [SerializeField] Text MaxComboText = null;
    [SerializeField] Text TotalScoreText = null;

    public int CurrentSongIdx { get; set; } = 0;

    ScoreManager mScoreManager;
    ComboManager mComboManager;
    TimingManager mTimingManager;
    DataBaseManager mDataBaseManager;
    void Start()
    {
        mScoreManager = FindObjectOfType<ScoreManager>();
        Debug.Assert(mScoreManager != null);
        mComboManager = FindObjectOfType<ComboManager>();
        Debug.Assert(mComboManager != null);
        mTimingManager = FindObjectOfType<TimingManager>();
        Debug.Assert(mTimingManager != null);
        mDataBaseManager = FindObjectOfType<DataBaseManager>();
        Debug.Assert(mDataBaseManager != null);
    }

    public void ShowResult()
    {
        FindObjectOfType<CenterFrame>().IsMusicStart = false;
        AudioManager.Instance.StopBGM();
        GoUI.SetActive(true);
        int[] tmingScores = mTimingManager.GetTimingScores();
        int currentScore = mScoreManager.CurrentScore;
        for (int i = 0; i < (int)TimingManager.eTimingType.NORMAL; ++i)
        {
            TimingScoreTexts[i].text = $"{tmingScores[i]}";
        }
        MaxComboText.text = $"{mComboManager.MaxComboScore}";
        TotalScoreText.text = $"{currentScore}";

        if (currentScore > mDataBaseManager.Scores[CurrentSongIdx])
        {
            mDataBaseManager.Scores[CurrentSongIdx] = currentScore;
            mDataBaseManager.SaveScores();
        }
    }

    public void OnGoToMainMenuBtnClicked()
    {
        mComboManager.ResetComboScore();
        GoUI.SetActive(false);
        GameManager.Instance.OnGoToMainMenuClickedInResultWindow();
    }
}
