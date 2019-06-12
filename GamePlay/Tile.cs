using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Normal,
    Breakable
}

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    Board m_board;

    public TileType tileType = TileType.Normal;
    SpriteRenderer m_spriterender;

    public int breakAbleValue = 0;
    public Sprite[] breakableSprite;
    public Color normalColor;
    // Start is called before the first frame update
    void Awake()
    {
        m_spriterender = GetComponent<SpriteRenderer>();
    }

    public void Init(int x, int y,Board board)
    {
        xIndex = x;
        yIndex = y;
        m_board = board;

        if (tileType == TileType.Breakable)
        {
            if (breakableSprite[breakAbleValue] != null)
            {
                m_spriterender.sprite = breakableSprite[breakAbleValue];
            }
        }
       
    }

    void OnMouseDown()
    {
        if (m_board.status == Board.GameStatus.Begin || m_board.status == Board.GameStatus.Resume)
        {
            if (m_board != null)
            {
                m_board.ClickTile(this);
            }
        }
       
    }
    void OnMouseEnter()
    {
        if (m_board.status == Board.GameStatus.Begin || m_board.status == Board.GameStatus.Resume)
        {
            if (m_board != null)
            {
                m_board.DragToTile(this);
            }
        }
           
    }
    void OnMouseUp()
    {
        if (m_board.status == Board.GameStatus.Begin || m_board.status == Board.GameStatus.Resume)
        {
            if (m_board != null)
            {
                m_board.ReleaseTile();
            }
        }
    }

    public void BreakTile()
    {
        if (tileType != TileType.Breakable)
        {
            return;
        }
        StartCoroutine(BreakTileRoutine());
    }
    
    IEnumerator BreakTileRoutine()
    {
        breakAbleValue = Mathf.Clamp(breakAbleValue--, 0, breakAbleValue);
        yield return new WaitForSeconds(0.25f);

        if (breakableSprite[breakAbleValue] != null)
        {
            ScorePoint();
            m_spriterender.sprite = breakableSprite[breakAbleValue];
        }

        if (breakAbleValue == 0)
        {
            ScorePoint();
            tileType = TileType.Normal;
            m_spriterender.color = normalColor;
        }
    }
    

    //break able get score
    public void ScorePoint()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.addScore(30);
        }
    }
}
