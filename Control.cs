using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    SFX sfx;
    BGM bgm;
    GameObject soundOn;
    GameObject soundOff;

    // Start is called before the first frame update
    void Awake()
    {
        sfx = GameObject.Find("SFX").GetComponent<SFX>();
        bgm = GameObject.Find("BGM").GetComponent<BGM>();
        //soundOn Component
        soundOn = GameObject.Find("SoundOn");
        //soundOff Component
        soundOff = GameObject.Find("SoundOff");

        if(PlayerPrefs.GetFloat("music") == null)
        {
            sfx.GetComponent<AudioSource>().volume = 1;
            bgm.GetComponent<AudioSource>().volume = 1;
        }
        else
        {
            bgm.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("music");
            // sound on
            if (PlayerPrefs.GetFloat("music") == 1f)
            {
                soundOn.SetActive(true);
                soundOff.SetActive(false);
            }
            // sound off
            if (PlayerPrefs.GetFloat("music") == 0f)
            {
                soundOn.SetActive(false);
                soundOff.SetActive(true);

            }
        }

        
    }


    public void PlaySound()
    {
        soundOn.SetActive(true);
        soundOff.SetActive(false);
        sfx.GetComponent<AudioSource>().volume = 1;
        bgm.GetComponent<AudioSource>().volume = 1;
        PlayerPrefs.SetFloat("music", 1f);
        PlayerPrefs.Save();
    }


    public void DisableSound()
    {
        soundOn.SetActive(false);
        soundOff.SetActive(true);
        sfx.GetComponent<AudioSource>().volume = 0;
        bgm.GetComponent<AudioSource>().volume = 0;
        PlayerPrefs.SetFloat("music", 0f);
        PlayerPrefs.Save();
    }
}
