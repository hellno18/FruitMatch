using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : Singleton<Menu>
{

    SFX sound;
    Animator anim;
    private void Awake()
    {
        sound = GameObject.Find("SFX").GetComponent<SFX>();
        anim = GameObject.Find("SettingBoard").GetComponent<Animator>();
    }

    
    public void PlayButton()
    {
        //play sfx
        sound.GetSFX(1);
        //delay 1f
        StartCoroutine(delay());
        SceneManager.LoadScene("ScreenOverLay");
    }

    public void OptionButton()
    {
        //play sfx
        sound.GetSFX(1);
        anim.SetBool("isOpen", true);
        anim.Play("BorderOpen");
    }

    public void OptionClose()
    {
        //play sfx
        sound.GetSFX(1);
        anim.SetBool("isOpen", false);
        anim.Play("BorderClose");
    }

    public void ExitButton()
    {
        sound.GetSFX(1);
        Application.Quit();
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);
    }
}
