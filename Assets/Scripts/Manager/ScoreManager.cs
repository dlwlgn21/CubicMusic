using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text ScoreText = null;

    [SerializeField] int IncreaseScoreAmount = 10;
    public int CurrentScore { get; private set; } = 0;

    [SerializeField] float[] ScoreWeights = null;
    [SerializeField] int ComboBonusScore = 10;

    Animator mAnimator;
    string scoreUpTrigger = "ScoreUp";

    ComboManager mComboManager;

    // Start is called before the first frame update
    void Start()
    {
        mComboManager = FindObjectOfType<ComboManager>();
        Debug.Assert(mComboManager != null);
        Debug.Assert(ScoreText != null);
        mAnimator = GetComponent<Animator>();
        Debug.Assert(mAnimator != null);
        Init();
    }

    public void Init()
    {
        CurrentScore = 0;
        ScoreText.text = "0";
    }
    public void IncreaseScore(TimingManager.eTimingType eType)
    {
        // Increase combo
        mComboManager.IncreaseComboScore();

        // calculate comboScore
        // ComboSection = 10~19 == 10, 20~29 = 20 ...
        int bonusComboScore = (mComboManager.CurrentCombo / 10) * ComboBonusScore;

        // Increase score
        int tmpIncreaseScoreAmount = IncreaseScoreAmount + bonusComboScore;
        tmpIncreaseScoreAmount = (int)(tmpIncreaseScoreAmount * ScoreWeights[(int)eType]);
        CurrentScore += tmpIncreaseScoreAmount;
        
        ScoreText.text = string.Format("{0:#,##0}", CurrentScore);
        
        mAnimator.SetTrigger(scoreUpTrigger);
    }

}
