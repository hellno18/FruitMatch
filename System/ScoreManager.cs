using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : Singleton<ScoreManager>
{
    int m_currentScore = 0;
    int m_counterValue = 0;
    int m_icrement = 5;


    [SerializeField] private Text timeText;
    [SerializeField] private Text scoreText;
    [SerializeField] private float countTime = 1f;
    [SerializeField] private float timeLimit = 60f;
    Board board;
    BGM bgm;
    SFX sfx;
    GameObject settingButton;
    TextMeshProUGUI scoreTotal;

    private void Awake()
    {
        bgm = GameObject.Find("BGM").GetComponent<BGM>();
        sfx = GameObject.Find("SFX").GetComponent<SFX>();
        board = GameObject.Find("Board").GetComponent<Board>();
        scoreTotal = GameObject.Find("ScoreTotal").GetComponent<TextMeshProUGUI>();
        settingButton = GameObject.Find("SettingButton");
        
    }
    // Update is called once per frame
    void Update()
    {
        UpdateScore(m_currentScore);
        //update score result
        scoreTotal.SetText("{0}", m_currentScore);
        if (board.GetGameStatus() == Board.GameStatus.Begin|| board.GetGameStatus() == Board.GameStatus.Resume)
        {
            TimeCountDown();
        }
        

    }

    public void TimeCountDown()
    {
        timeLimit -= Time.deltaTime;
        timeText.text = ((int)timeLimit + 1).ToString();
        if (timeLimit <= 0f)
        {
            settingButton.GetComponent<Button>().interactable = false;
            timeText.text = "0";  
            board.SetGameStatus(Board.GameStatus.Finished);
            //play sfx
            sfx.GetSFX(5);
            //change bgm
            StartCoroutine(bgm.FadeOut(1f));
            //show Border Retry
            Animator anim = GameObject.Find("RetryBoard").GetComponent<Animator>();
            anim.Play("Border");
            anim.SetBool("isOpen", true);
            anim.Play("BorderOpen");
        }
    }

    public void UpdateScore(int scoreValue)
    {
        if(scoreText != null)
        {
            scoreText.text = scoreValue.ToString();
        }
    }

    public void addScore(int value)
    {
        m_currentScore += value;
        StartCoroutine(CountScoreRoutine());
    }

   

    IEnumerator CountScoreRoutine()
    {
        int inter = 0;
        while(m_counterValue < m_currentScore&& inter < 100000)
        {
            m_counterValue += m_icrement;
            UpdateScore(m_counterValue);
            inter++;
            yield return null;
        }
        m_counterValue = m_currentScore;
        UpdateScore(m_currentScore);
    }

    
}
