using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string ClipName;
    public AudioClip Clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] Sound[] SFXSounds = null;
    [SerializeField] Sound[] BGMSounds = null;

    [SerializeField] AudioSource BgmPlayer = null;
    [SerializeField] AudioSource[] SfxPlayers = null;

    public static string TOUCH_SFX_CLIP_NAME = "Touch";
    public static string CLAP_SFX_CLIP_NAME = "Clap";
    public static string FALLING_SFX_CLI_NAME = "Falling";

    public static string BGM0_CLIP_NAME = "BGM0";
    public static string BGM1_CLIP_NAME = "BGM1";
    public static string BGM2_CLIP_NAME = "BGM2";

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void PlayBGM(string bgmName)
    {
        for (int i = 0; i < BGMSounds.Length; ++i)
        {
            if (bgmName == BGMSounds[i].ClipName)
            {
                BgmPlayer.clip = BGMSounds[i].Clip;
                BgmPlayer.Play();
                return;
            }
        }
    }
    public void StopBGM()
    {
        BgmPlayer.Stop();
    }

    public void PlaySFX(string sfxName)
    {
        for (int i = 0; i < SFXSounds.Length; ++i)
        {
            if (sfxName == SFXSounds[i].ClipName)
            {
                for (int j = 0; j < SfxPlayers.Length; ++j)
                {
                    if (!SfxPlayers[j].isPlaying)
                    {
                        SfxPlayers[j].clip = SFXSounds[i].Clip;
                        SfxPlayers[j].Play();
                        return;
                    }
                }
                Debug.Assert(false, "All SfxPlayer Is Playing Auidos!!! Add AudioSource!!");
                return;
            }
        }
        Debug.Assert(false, $"There is No {sfxName} file! Wrong Paremeter!" );
    }


}
