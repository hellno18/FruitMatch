using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BGM : MonoBehaviour
{
    static public BGM instance;
    public AudioClip[] audioClip;
    AudioSource m_audio;

    // Start is called before the first frame update
    void Awake()
    {
        //get component
        m_audio = GetComponent<AudioSource>();

        //select audioclip
        m_audio.clip = audioClip[0];
        //play BGM
        m_audio.Play();
        m_audio.playOnAwake = true;
        m_audio.loop = true;
        
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
       
          
       
       
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "ScreenOverLay")
        {
            //change bgm
               m_audio.clip = audioClip[1];
               m_audio.Play();
        }
    }

    public IEnumerator FadeOut(float fadetime)
    {
        float startVolume = m_audio.volume;
        while (m_audio.volume > 0)
        {
            m_audio.volume -= startVolume * Time.deltaTime / fadetime;

            yield return null;
        }

        m_audio.Stop();
        //change bgm
        m_audio.clip = audioClip[2];
        m_audio.volume = startVolume;
        m_audio.Play();
    }

    public IEnumerator FadeOutTitle(float fadetime)
    {
       float startVolume = m_audio.volume;
       while (m_audio.volume > 0)
       {
           m_audio.volume -= startVolume * Time.deltaTime / fadetime;

           yield return null;
       }
       print("a");
       m_audio.Stop();
       //change bgm
       m_audio.clip = audioClip[0];
       m_audio.volume = startVolume;
       m_audio.Play();
        
    }


    
}
