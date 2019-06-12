using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFX : MonoBehaviour
{
    
    public AudioClip[] sfxGroup;
    AudioSource m_audio;

    // Start is called before the first frame update
    void Awake()
    {
        //get component
        m_audio = GetComponent<AudioSource>();
    }



    public void GetSFX(int sfx)
    {
        switch (sfx)
        {
            //0. kira kira
            //1. button, 2. clearSFX 3. breakSFX 4. bombSFX 
            //5. times up 6. false dot
            case 0:
                m_audio.PlayOneShot(sfxGroup[0]);
                break;
            case 1:
                m_audio.PlayOneShot(sfxGroup[1]);
                break;
            case 2:
                m_audio.PlayOneShot(sfxGroup[2]);
                break;
            case 3:
                m_audio.PlayOneShot(sfxGroup[3]);
                break;
            case 4:
                m_audio.PlayOneShot(sfxGroup[4]);
                break;
            case 5:
                m_audio.PlayOneShot(sfxGroup[5]);
                break;
            case 6:
                m_audio.PlayOneShot(sfxGroup[6]);
                break;
        }
    }

}
