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
    private int xIndex;
    private int yIndex;
    Board m_board;
     
    SpriteRenderer m_spriterender;

    [SerializeField] private TileType tileType = TileType.Normal;
    [SerializeField] int breakAbleValue = 0;
    [SerializeField] private Sprite[] breakableSprite;
    [SerializeField] private Color normalColor;
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
        if (m_board.GetGameStatus() == Board.GameStatus.Begin || m_board.GetGameStatus() == Board.GameStatus.Resume)
        {
            if (m_board != null)
            {
                m_board.ClickTile(this);
            }
        }
       
    }
    void OnMouseEnter()
    {
        if (m_board.GetGameStatus() == Board.GameStatus.Begin || m_board.GetGameStatus() == Board.GameStatus.Resume)
        {
            if (m_board != null)
            {
                m_board.DragToTile(this);
            }
        }
           
    }
    void OnMouseUp()
    {
        if (m_board.GetGameStatus() == Board.GameStatus.Begin || m_board.GetGameStatus() == Board.GameStatus.Resume)
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

    //getter tileType
    public TileType GetTileType()
    {
        return tileType;
    }

    //setter tileType
    public void SetTileType(TileType value)
    {
        tileType = value;
    }

    //getter BreakAbleValue
    public int GetBreakAbleValue()
    {
        return breakAbleValue;
    }

    //getter xIndex
    public int GetXIndex()
    {
        return xIndex;
    }

    //getter yIndex
    public int GetYIndex()
    {
        return yIndex;
    }
}
