using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{

    [SerializeField] GameObject GoStageMenuUI = null;

    public void OnPlayBtnCliked()
    {
        GoStageMenuUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
