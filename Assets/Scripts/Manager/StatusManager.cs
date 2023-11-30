using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusManager : MonoBehaviour
{
    const int MAX_HP_SHIELD_AMOUNT = 3;

    int mMaxHp = MAX_HP_SHIELD_AMOUNT;
    int mCurrentHp = MAX_HP_SHIELD_AMOUNT;

    int mMaxShield = MAX_HP_SHIELD_AMOUNT;
    int mCurrentShield = 0;
    public bool IsDead { get; private set; } = false;

    [SerializeField] GameObject[] HpImages = null;
    [SerializeField] GameObject[] ShieldImages = null;

    [SerializeField] int NeedComboScoreToIncreaseShield = 5;
    int mCurrentShieldCombo = 0;
    [SerializeField] Image ShieldGauge = null;

    // Blink Section
    [SerializeField] float BlinkSpeedSec = 0.1f;
    [SerializeField] int BlinkCount = 10;
    int mCurrentBlinkCount = 0;
    bool mbIsBlinking = false;

    Result mResult;
    NoteManager mNoteManager;

    [SerializeField] MeshRenderer mPlayerMeshRenderer = null;

    void Start()
    {
        mResult = FindObjectOfType<Result>();
        Debug.Assert(mResult != null);
        mNoteManager = FindObjectOfType<NoteManager>();
        Debug.Assert(mNoteManager != null);
    }
    public void Init()
    {
        mCurrentHp = MAX_HP_SHIELD_AMOUNT;
        mCurrentShield = 0;
        mCurrentShieldCombo = 0;
        ShieldGauge.fillAmount = 0f;
        IsDead = false;
        SettingHpImage();
        SettingShieldImage();
    }
    public void IncreaseShieldIfComboScoreEnough()
    {
        ++mCurrentShieldCombo;
        if (mCurrentShieldCombo >= NeedComboScoreToIncreaseShield)
        {
            mCurrentShieldCombo = 0;
            IncreaseShield();
        }
        ShieldGauge.fillAmount = (float)mCurrentShieldCombo / NeedComboScoreToIncreaseShield;
    }

    public void ResetShieldCombo()
    {
        mCurrentShieldCombo = 0;
        ShieldGauge.fillAmount = 0f;

    }

    public void IncreaseHp(int amount)
    {
        mCurrentHp += Math.Abs(amount);
        if (mCurrentHp >= mMaxHp)
        {
            mCurrentHp = mMaxHp;
        }
        SettingHpImage();
    }

    public void DecreaseHp(int decreaseAmount)
    {
        if (mbIsBlinking)
        {
            return;
        }

        if (mCurrentShield > 0)
        {
            DecreaseShield(decreaseAmount);
        }
        else
        {
            mCurrentHp -= Math.Abs(decreaseAmount);
            SettingHpImage();

            if (mCurrentHp <= 0)
            {
                IsDead = true;
                mNoteManager.RemoveAllNote();
                mResult.ShowResult();
            }
            else
            {
                StartCoroutine(BlinkPlayerCoroutine());
            }
        }


    }

    private void IncreaseShield()
    {
        ++mCurrentShield;
        if (mCurrentShield >= mMaxShield)
        {
            mCurrentShield = mMaxShield;
        }
        SettingShieldImage();
    }

    public void DecreaseShield(int amount)
    {
        mCurrentShield -= Math.Abs(amount);
        if (mCurrentShield <= 0)
        {
            mCurrentShield = 0;
        }
        SettingShieldImage();
    }

    private void SettingHpImage()
    {
        for (int i = 0; i < HpImages.Length; ++i)
        {
            if (i < mCurrentHp)
            {
                HpImages[i].SetActive(true);
            }
            else
            {
                HpImages[i].SetActive(false);
            }
        }
    }

    private void SettingShieldImage()
    {
        for (int i = 0; i < ShieldImages.Length; ++i)
        {
            if (i < mCurrentShield)
            {
                ShieldImages[i].SetActive(true);
            }
            else
            {
                ShieldImages[i].SetActive(false);
            }
        }
    }

    IEnumerator BlinkPlayerCoroutine()
    {
        mbIsBlinking = true;
        while (mCurrentBlinkCount <= BlinkCount)
        {
            mPlayerMeshRenderer.enabled = !mPlayerMeshRenderer.enabled;
            yield return new WaitForSeconds(BlinkSpeedSec);
            ++mCurrentBlinkCount;
        }

        mPlayerMeshRenderer.enabled = true;
        mCurrentBlinkCount = 0;
        mbIsBlinking = false;
    }
}
