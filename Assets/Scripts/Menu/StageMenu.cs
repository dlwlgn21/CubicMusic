using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[System.Serializable]
public class SongInfo
{
    public string SongName;
    public string ComposerName;
    public int BeatPerMin;
    public Sprite DiskSprite;
}


public class StageMenu : MonoBehaviour
{
    [SerializeField] SongInfo[] SongInfoList;
    [SerializeField] GameObject GoTitleMenuUI = null;

    [SerializeField] Text SongNameText;
    [SerializeField] Text SongComposerText;
    [SerializeField] Text SongMaxScoreText;
    [SerializeField] Image DiskImage;

    DataBaseManager mDatabaseManager;

    int mCurrentSongIdx = 0;

    void OnEnable()
    {
        if (mDatabaseManager == null)
        {
            mDatabaseManager = FindObjectOfType<DataBaseManager>();
            Debug.Assert(mDatabaseManager != null);
        }
        SetSongUI();
        PlaySong();
    }
    public void OnNextBtnClicked()
    {
        PlayTouchSound();
        mCurrentSongIdx = (mCurrentSongIdx + 1) % SongInfoList.Length;
        SetSongUI();
        PlaySong();
    }
    public void OnPrevBtnClicked()
    {
        PlayTouchSound();
        --mCurrentSongIdx;
        if (mCurrentSongIdx < 0)
        {
            mCurrentSongIdx = SongInfoList.Length - 1;
        }
        SetSongUI();
        PlaySong();
    }

    void SetSongUI()
    {
        SongNameText.text = SongInfoList[mCurrentSongIdx].SongName;
        SongComposerText.text = SongInfoList[mCurrentSongIdx].ComposerName;
        SongMaxScoreText.text = $"{mDatabaseManager.Scores[mCurrentSongIdx]}";
        DiskImage.sprite = SongInfoList[mCurrentSongIdx].DiskSprite;
    }
    private void PlaySong()
    {
        switch (mCurrentSongIdx)
        {
            case 0:
                AudioManager.Instance.PlayBGM(AudioManager.BGM0_CLIP_NAME);
                break;
            case 1:
                AudioManager.Instance.PlayBGM(AudioManager.BGM1_CLIP_NAME);
                break;
            case 2:
                AudioManager.Instance.PlayBGM(AudioManager.BGM2_CLIP_NAME);
                break;
            default:
                Debug.Assert(false, "Wrong Song Index!!!!");
                break;
        }
    }
    private void PlayTouchSound()
    {
        AudioManager.Instance.PlaySFX(AudioManager.TOUCH_SFX_CLIP_NAME);
    }
    public void OnBackBtnClicked()
    {
        GoTitleMenuUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnPlayBtnClicked()
    {
        int bpm = SongInfoList[mCurrentSongIdx].BeatPerMin;
        GameManager.Instance.GameStart(mCurrentSongIdx, bpm);
        gameObject.SetActive(false);
    }
}
