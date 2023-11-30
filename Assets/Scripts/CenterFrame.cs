using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CenterFrame : MonoBehaviour
{
    public bool IsMusicStart { get; set; } = false;
    public string BgmName { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsMusicStart)
        {
            if (collision.CompareTag("Note"))
            {
                AudioManager.Instance.PlayBGM(BgmName);
                IsMusicStart = true;
            }
        }

    }
}
