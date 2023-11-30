using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] GoGameUIs = null;
    [SerializeField] GameObject GoTitleUI = null;
    public static GameManager Instance;
    public bool IsStartGame { get; set; } = false;

    ComboManager mComboManager;
    ScoreManager mScoreManager;
    TimingManager mTimingManager;
    StatusManager mStatusManager;
    StageManager mStageManager;
    NoteManager mNoteManager;
    Result mResult;
    PlayerController mPlayerController;
    [SerializeField] CenterFrame mCenterFrameForPlayBGMInStage;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            mComboManager = FindObjectOfType<ComboManager>();
            mScoreManager = FindObjectOfType<ScoreManager>();
            mTimingManager = FindObjectOfType<TimingManager>();
            mStatusManager = FindObjectOfType<StatusManager>();
            mStageManager = FindObjectOfType<StageManager>();
            mNoteManager = FindObjectOfType<NoteManager>();
            mResult = FindObjectOfType<Result>();
            mPlayerController = FindObjectOfType<PlayerController>();
            Debug.Assert(mComboManager != null &&
                         mScoreManager != null &&
                         mTimingManager != null &&
                         mStatusManager != null &&
                         mStageManager != null &&
                         mNoteManager != null &&
                         mResult != null &&
                         mPlayerController != null &&
                         mCenterFrameForPlayBGMInStage != null);
        }
    }
    public void GameStart(int currentSongIndex, int bpm)
    {
        for (int i = 0; i < GoGameUIs.Length; ++i)
        {
            GoGameUIs[i].SetActive(true);
        }
        mCenterFrameForPlayBGMInStage.BgmName = $"BGM{currentSongIndex}";
        mNoteManager.BeatPerMin = bpm;
        mStageManager.DestroyCurrentStage();
        mStageManager.InitStage(currentSongIndex);
        mComboManager.ResetComboScore();
        mScoreManager.Init();
        mTimingManager.Init();
        mStatusManager.Init();
        mPlayerController.Init();
        mResult.CurrentSongIdx = currentSongIndex;
        AudioManager.Instance.StopBGM();
        IsStartGame = true;
    }

    public void OnGoToMainMenuClickedInResultWindow()
    {
        for (int i = 0; i < GoGameUIs.Length; ++i)
        {
            GoGameUIs[i].SetActive(false);
        }

        GoTitleUI.SetActive(true);
    }
}
