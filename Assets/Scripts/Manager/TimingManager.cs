using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Assertions;

public class TimingManager : MonoBehaviour
{
public enum eTimingType
{
    PERPECT,
    COOL,
    GOOD,
    BAD,
    MISS,
    NORMAL,
    COUNT
}

struct TimingBoxWidth
{
    public float Left;
    public float Right;
}

int[] mTimingScores = new int[(int)eTimingType.NORMAL];

public List<GameObject> BoxNoteList = new List<GameObject>(); // 판정 범위에 있는지 모든 노트를 비교해야 함

[SerializeField] Transform CenterPosition = null;
[SerializeField] RectTransform[] TimingRectsTransforms = null; // 다양한 판정 범위를 가짐. (Perpect, Cool, Good, Bad)
TimingBoxWidth[] mTimingBoxWidths = null;

EffectManager mEffectManager = null;
ScoreManager mScoreManager = null;
ComboManager mComboManager = null;
StageManager mStageManager = null;
PlayerController mPlayerController = null;
StatusManager mStatusManager = null;
AudioManager mAudioManager = null;
void Start()
{
    Debug.Assert(CenterPosition != null);
    Debug.Assert(TimingRectsTransforms.Length != 0);
    mEffectManager = FindObjectOfType<EffectManager>();
    Debug.Assert(mEffectManager != null);
    mScoreManager = FindObjectOfType<ScoreManager>();
    Debug.Assert(mScoreManager != null);
    mComboManager = FindObjectOfType<ComboManager>();
    Debug.Assert(mComboManager != null);
    mStageManager = FindObjectOfType<StageManager>();
    Debug.Assert(mStageManager != null);
    mPlayerController = FindObjectOfType<PlayerController>();
    Debug.Assert(mPlayerController != null);
    mStatusManager = FindObjectOfType<StatusManager>();
    Debug.Assert(mStatusManager != null);
    mAudioManager = AudioManager.Instance;
    Debug.Assert(mAudioManager != null);

    mTimingBoxWidths = new TimingBoxWidth[TimingRectsTransforms.Length];
    for (int i = 0; i < mTimingBoxWidths.Length; ++i)
    {
        mTimingBoxWidths[i].Left = CenterPosition.localPosition.x - TimingRectsTransforms[i].rect.width * 0.5f;
        mTimingBoxWidths[i].Right = CenterPosition.localPosition.x + TimingRectsTransforms[i].rect.width * 0.5f;
    }
}

public void Init()
{
    for (int i = 0; i < mTimingScores.Length; ++i)
    {
        mTimingScores[i] = 0;
    }
}


public bool IsHitCorrectTiming()
{
    for (int i = 0; i < BoxNoteList.Count; ++i)
    {
        float noteXPos = BoxNoteList[i].transform.localPosition.x;
        for (int j = 0; j < mTimingBoxWidths.Length; ++j)
        {
            if (noteXPos >= mTimingBoxWidths[j].Left && noteXPos <= mTimingBoxWidths[j].Right)
            {

                eTimingType eTiming = (eTimingType)j;
                switch (eTiming)
                {
                    case eTimingType.PERPECT:
                        //Debug.Log($"Perpect!");
                        mEffectManager.PlayNoteHitEffectAnimation();
                        break;
                    case eTimingType.COOL:
                        //Debug.Log($"Cool!");
                        mEffectManager.PlayNoteHitEffectAnimation();
                        break;
                    case eTimingType.GOOD:
                        //Debug.Log($"Good!");
                        mEffectManager.PlayNoteHitEffectAnimation();
                        break;
                    case eTimingType.BAD:
                        //Debug.Log($"Bad!");
                        break;
                    default:
                        break;
                }

                if (IsCanShowNextPlate())
                {
                    mScoreManager.IncreaseScore(eTiming);
                    mStageManager.ShowNextPlate();
                    mEffectManager.PlayJudmentEffectAnimation(eTiming);
                    mTimingScores[j] += 1;
                    mStatusManager.IncreaseShieldIfComboScoreEnough();
                }
                else
                {
                    mEffectManager.PlayJudmentEffectAnimation(eTimingType.NORMAL);
                }
                BoxNoteList[i].GetComponent<Note>().HideNote();
                BoxNoteList.RemoveAt(i);

                mAudioManager.PlaySFX(AudioManager.CLAP_SFX_CLIP_NAME);
                return true;
            }
        }
    }
    mComboManager.ResetComboScore();
    mEffectManager.PlayJudmentEffectAnimation(eTimingType.MISS);
    IncreaseMissTimingScore();
    return false;
}

bool IsCanShowNextPlate()
{
    if (Physics.Raycast(mPlayerController.mDestinationPosition, Vector3.down, out RaycastHit hitInfo, 1.1f))
    {
        if (hitInfo.transform.CompareTag("BasicPlate"))
        {
            BasicPlate tmpPlate = hitInfo.transform.GetComponent<BasicPlate>();
            if (tmpPlate.bIsCanShow)
            {
                tmpPlate.bIsCanShow = false;
                return true;
            }
        }
    }

    return false;
}

public int[] GetTimingScores()
{
    return mTimingScores;
}

public void IncreaseMissTimingScore()
{
    mTimingScores[(int)eTimingType.MISS] += 1;
    mStatusManager.ResetShieldCombo();
}
}
