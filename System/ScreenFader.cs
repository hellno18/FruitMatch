using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//libary of fade system
[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour
{
    public float solid = 1f;
    public float clear = 0f;
    public float delay = 0f;
    public float timeToFade = 1f;

    MaskableGraphic m_graphic;


    // Start is called before the first frame update
    void Start()
    {
        m_graphic = GetComponent<MaskableGraphic>();
    }

    private void Update()
    {
        if(Input.anyKeyDown){
            FadeOut();
        }
    }

    IEnumerator FadeRoutine ( float alpha)
    {
        yield return new WaitForSeconds(delay);
        m_graphic.CrossFadeAlpha(alpha, timeToFade, true);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Main");
    }

    public void FadeOn()
    {
        StartCoroutine(FadeRoutine(solid));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeRoutine(clear));
    }

    
}
