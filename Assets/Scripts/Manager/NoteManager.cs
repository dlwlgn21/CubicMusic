using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Assertions;

public class NoteManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int BeatPerMin { get; set; } = 0;
    double CurrentTime = 0d;

    [SerializeField] Transform NoteAppaerPosition = null;
    TimingManager mTimingManager;
    EffectManager mEffectManager;
    ComboManager mComboManager;


    void Start()
    {
        mTimingManager = GetComponent<TimingManager>();
        mEffectManager = FindObjectOfType<EffectManager>();
        mComboManager = FindObjectOfType<ComboManager>();
        Debug.Assert(NoteAppaerPosition != null);
        Debug.Assert(mTimingManager != null );
        Debug.Assert(mEffectManager != null);
        Debug.Assert(mComboManager != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsStartGame)
        {
            CurrentTime += Time.deltaTime;
            // 60Second / BPM == 1 Beat 시간.
            // 만약에 120 BPM이면, 60 / 120, 1Bit당 0.5초
            // 다시 말해, 0.5초가 지나면
            if (CurrentTime >= 60d / BeatPerMin)
            {

                GameObject note = ObjectPool.instance.NoteQue.Dequeue();
                note.transform.position = NoteAppaerPosition.position;
                note.SetActive(true);
                mTimingManager.BoxNoteList.Add(note);
                CurrentTime -= 60d / BeatPerMin; // 왜 이렇게 뺴주는지는 강의1, 7분경을 보면 됨. 오차때문에 그렇다고 함.
            }
        }
    }

    // When Miss Note
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            if (collision.GetComponent<Note>().GetNoteVisibleFlag())
            {
                // Miss
                mEffectManager.PlayJudmentEffectAnimation(TimingManager.eTimingType.MISS);

                // Combo
                mComboManager.ResetComboScore();

                // Score
                mTimingManager.IncreaseMissTimingScore();
            }
            mTimingManager.BoxNoteList.Remove(collision.gameObject);
            ObjectPool.instance.NoteQue.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }

    public void RemoveAllNote()
    {
        GameManager.Instance.IsStartGame = false;
        for (int i = 0; i < mTimingManager.BoxNoteList.Count; ++i)
        {
            mTimingManager.BoxNoteList[i].gameObject.SetActive(false);
            ObjectPool.instance.NoteQue.Enqueue(mTimingManager.BoxNoteList[i]);
        }
        mTimingManager.BoxNoteList.Clear();
    }
}
