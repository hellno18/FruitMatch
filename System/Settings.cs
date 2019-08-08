using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : Singleton<Settings>
{
    GameObject settingBoardButton;
    Board board;
    Animator anim;
    GameObject settingButton;
    SFX sfx;
    BGM bgm;
   

    private void Awake()
    {
        sfx = GameObject.Find("SFX").GetComponent<SFX>();
        bgm = GameObject.Find("BGM").GetComponent<BGM>();
        anim = GameObject.Find("SettingBoard").GetComponent<Animator>();

        settingButton = GameObject.Find("SettingButton");
        settingBoardButton = GameObject.Find("CanvasSetting");
       
        board = GameObject.Find("Board").GetComponent<Board>();
        anim.Play("Border");
    }

    public void SettingButtonClicked()
    {
        settingBoardButton.SetActive(true);
        board.SetGameStatus(Board.GameStatus.Pause);
        //play sfx
        sfx.GetSFX(1);
        anim.SetBool("isOpen", true);
        anim.Play("BorderOpen");
    }

    public void Retry()
    {
        //change bgm song
        ChangeBGM(1);
        settingButton.GetComponent<Button>().interactable=true;
        SceneManager.LoadScene("Main");
    }

    public void SettingButtonClear()
    {
        StartCoroutine(SettingRoutine());
    }

    public void SettingQuit()
    {
        settingButton.GetComponent<Button>().interactable = true;
        float startVolume = bgm.GetComponent<AudioSource>().volume;
        while (bgm.GetComponent<AudioSource>().volume > 0)
        {
            bgm.GetComponent<AudioSource>().volume -= startVolume * Time.deltaTime;
        }
        //change bgm song
        ChangeBGM(0);
        bgm.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Title");
    }

    void ChangeBGM(int audio)
    {
        bgm.GetComponent<AudioSource>().Stop();
        //change bgm
        bgm.GetComponent<AudioSource>().clip = bgm.GetAudioClip()[audio];
        bgm.GetComponent<AudioSource>().volume = bgm.GetComponent<AudioSource>().volume;
        bgm.GetComponent<AudioSource>().Play();
    }

    IEnumerator SettingRoutine()
    {
        board.SetGameStatus(Board.GameStatus.Resume);
        //play sfx
        sfx.GetSFX(1);

        anim.SetBool("isOpen", false);
        anim.Play("BorderClose");
        yield return new WaitForSeconds(0.4f);
    }
}
