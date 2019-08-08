using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    Board m_board;
    private bool m_isMoving = false;


    public Interpolation interpolation = Interpolation.SmootherStep;
    public enum Interpolation
    {
        Linear,
        EaseOut,
        EaseIn,
        SmoothStep,
        SmootherStep
    };
    public MatchValue matchvalue;

    public int scoreValue = 20;

    public enum MatchValue
    {
        BlueBerry,
        Coconut,
        Apple,
        Banana,
        Orange,
        WaterMelon,
        Wild
    };



    //initialize 初期化する
    public void Init(Board board)
    {
        m_board = board;
        
    }

    //Cordinate 初期化する
    public void SetCord(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    //Move dot 初期化する
    public void Move(int x,int y,float moveTime)
    {
        if (!m_isMoving)
        {
            StartCoroutine(MoveRoutine(new Vector3(x, y, 0), moveTime));
        }
     
    }

    //Dot will Change position while dragged
    IEnumerator MoveRoutine(Vector3 destination, float moveTime)
    {
        Vector3 startPosition = transform.position;
        bool reachPosition = false;
        float timeElapsed = 0f;
        m_isMoving = true;
        while (!reachPosition)
        {
            //close enough to destination
            if (Vector3.Distance(transform.position, destination) < 0.01f)
            {
                reachPosition = true;
                
                if (m_board != null)
                {
                    m_board.PlaceDotGame(this, (int)destination.x, (int)destination.y);
                }
                break;
            }
            timeElapsed += Time.deltaTime;
            float time = Mathf.Clamp(timeElapsed / moveTime,0f,1f);

            //if failed to change position
            //give natural movement (interpolation)
            switch (interpolation)
            {
                case Interpolation.Linear:
                    break;
                case Interpolation.EaseIn:
                    time = Mathf.Sin(time * Mathf.PI * 0.5f);
                    break;
                case Interpolation.EaseOut:
                    time = 1- Mathf.Cos(time * Mathf.PI * 0.5f);
                    break;
                case Interpolation.SmoothStep:
                    time = time * time * (3 - 2 * time);
                    break;
                case Interpolation.SmootherStep:
                    time = time * time * time *(time*(time*6-15)+10);
                    break;

            }
           
            //move to destination 
            transform.position = Vector3.Lerp(startPosition, destination, time);

            //wait until next frame
            yield return null; 
        }
        m_isMoving = false;
    }

    //Add score  
    public void ScorePoint()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.addScore(scoreValue);
        }
    }
}
