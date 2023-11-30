using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] GameObject GoComboImage = null;
    [SerializeField] UnityEngine.UI.Text ComboText = null;

    Animator mAnimator;
    string mComboUpTrigger = "ComboUp";
    public int CurrentCombo { get; private set; } = 0;
    public int MaxComboScore { get; private set; } = 0;

    const int MIN_COMBO_SCORE_TO_SHOW = 2;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(GoComboImage != null);
        Debug.Assert(ComboText != null);
        ResetComboScore();
        mAnimator = GetComponent<Animator>();
        Debug.Assert(mAnimator != null);
    }

    public void IncreaseComboScore(int score = 1)
    {
        CurrentCombo += score;
        ComboText.text = string.Format("{0:#,##0}", CurrentCombo);
        if (CurrentCombo > MaxComboScore)
        {
            MaxComboScore = CurrentCombo;
        }
        if (CurrentCombo > MIN_COMBO_SCORE_TO_SHOW)
        {
            ComboText.gameObject.SetActive(true);
            GoComboImage.SetActive(true);
            mAnimator.SetTrigger(mComboUpTrigger);
        }
    }

    public void ResetComboScore()
    {
        CurrentCombo = 0;
        MaxComboScore = 0;
        ComboText.text = string.Format("{0:#,##0}", CurrentCombo);
        ComboText.gameObject.SetActive(false);
        GoComboImage.SetActive(false);
    }

}
