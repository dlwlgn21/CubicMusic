using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{

    [SerializeField] Animator NoteHitEffectAnimator;
    [SerializeField] Animator JudmentEffectAnimator;
    [SerializeField] Sprite[] JudmentSprites;
    [SerializeField] UnityEngine.UI.Image JudmentImage;
    string mTriggerName = "Hit";
    void Start()
    {
        Debug.Assert(NoteHitEffectAnimator != null);
        Debug.Assert(JudmentEffectAnimator != null);
        Debug.Assert(JudmentSprites.Length != 0);
    }

    public void PlayNoteHitEffectAnimation()
    {
        NoteHitEffectAnimator.SetTrigger(mTriggerName);
    }
    public void PlayJudmentEffectAnimation(TimingManager.eTimingType eType)
    {
        JudmentImage.sprite = JudmentSprites[(int) eType];
        JudmentEffectAnimator.SetTrigger(mTriggerName);
    }
}
