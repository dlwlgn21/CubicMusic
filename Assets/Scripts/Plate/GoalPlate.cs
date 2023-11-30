using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    AudioSource mGoalAudio;
    NoteManager mNoteManager;
    Result mResult;

    void Start()
    {
        mGoalAudio = GetComponent<AudioSource>();
        Debug.Assert(mGoalAudio != null);
        mNoteManager = FindObjectOfType<NoteManager>();
        Debug.Assert(mNoteManager != null);
        mResult = FindObjectOfType<Result>();
        Debug.Assert(mResult != null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mGoalAudio.Play();
            PlayerController.sIsCanPressKey = false;
            mNoteManager.RemoveAllNote();
            mResult.ShowResult();
        }

    }
}
