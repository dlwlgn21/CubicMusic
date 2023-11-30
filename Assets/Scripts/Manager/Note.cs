using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Note : MonoBehaviour
{
    public float NoteSpeed = 400f;
    UnityEngine.UI.Image mNoteImage;

    void OnEnable()
    {
        if (mNoteImage == null)
        {
            mNoteImage = GetComponent<UnityEngine.UI.Image>();
        }
        Debug.Assert(mNoteImage != null);
        mNoteImage.enabled = true;
    }

    void Update()
    {
        transform.localPosition += Vector3.right * NoteSpeed * Time.deltaTime;
    }
    public void HideNote()
    {
        mNoteImage.enabled = false;
    }

    public bool GetNoteVisibleFlag()
    {
        return mNoteImage.enabled;
    }
}
