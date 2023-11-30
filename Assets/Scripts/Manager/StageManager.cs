using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject[] GoStageArray = null;
    GameObject mCurrentStage;
    Transform[] mInvisivlePlateTransforms;

    int mStepCount = 0;
    int mTotalPlateCount;

    [SerializeField] float OffsetY = -3f;
    [SerializeField] float PlateUpSpeed = 10f;


    // Start is called before the first frame update
    public void InitStage(int songIndex)
    {
        Debug.Assert(songIndex < GoStageArray.Length);
        mStepCount = 0;
        mCurrentStage = Instantiate(GoStageArray[songIndex], Vector3.zero, Quaternion.identity);
        mInvisivlePlateTransforms = mCurrentStage.GetComponent<Stage>().InvisivlePlateTransforms;
        Debug.Assert(mInvisivlePlateTransforms != null);
        mTotalPlateCount = mInvisivlePlateTransforms.Length;

        for (int i = 0; i < mTotalPlateCount; ++i)
        {
            mInvisivlePlateTransforms[i].position = new Vector3(mInvisivlePlateTransforms[i].position.x,
                                                                mInvisivlePlateTransforms[i].position.y + OffsetY,
                                                                mInvisivlePlateTransforms[i].position.z);
        }

    }

    public void DestroyCurrentStage()
    {
        if (mCurrentStage != null)
        {
            Destroy(mCurrentStage);
        }
    }
    public void ShowNextPlate()
    {
        if (mStepCount < mTotalPlateCount)
        {
            StartCoroutine(MoveUpPlateCoroutine(mStepCount++));
        }
    }

    IEnumerator MoveUpPlateCoroutine(int stepCount)
    {
        mInvisivlePlateTransforms[stepCount].gameObject.SetActive(true);
        Vector3 destPos = new Vector3(mInvisivlePlateTransforms[stepCount].position.x,
                                      mInvisivlePlateTransforms[stepCount].position.y - OffsetY,
                                      mInvisivlePlateTransforms[stepCount].position.z);
        while (Vector3.SqrMagnitude(mInvisivlePlateTransforms[stepCount].position - destPos) >= 0.001f)
        {
            mInvisivlePlateTransforms[stepCount].position = Vector3.Lerp(mInvisivlePlateTransforms[stepCount].position, 
                                                                         destPos, 
                                                                         PlateUpSpeed * Time.deltaTime);
            yield return null;
        }
        mInvisivlePlateTransforms[stepCount].position = destPos;
    }
}
