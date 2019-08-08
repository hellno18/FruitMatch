using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class Board : MonoBehaviour
{
    //enum gamestatus
    public enum GameStatus
    {
        Begin,
        Pause,
        Resume,
        Finished
    }
    private GameStatus status = GameStatus.Begin;

    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private GameObject tileNormalPrefab;
    [SerializeField] private GameObject tileObstaclePrefap;

    [SerializeField] private GameObject[] dotPrefaps;
    [SerializeField] private int borderSize;

    [SerializeField] private float swapTime = 0.5f;
    [SerializeField] private int fillOffset = 10;

    int m_scoreMultiplier = 0;

    Tile[,] m_allTiles;
    Dot[,] m_allDots;

    Tile m_clickedTile;
    Tile m_targetTile;

    bool m_playerInput = true;

    private ParticleManager m_particleManager;
    private SFX sfx;

    //obstacle tile class
    [SerializeField] private StartingTile[] startingTile;
    [SerializeField] private StartingTile[] startingDots;

    [System.Serializable]
    public class StartingTile
    {
        public GameObject tilePrefap;
        public int x;
        public int y;
        public int z;
        // [NOT YET] setter getter startingTile
    }

    private void Start()
    {
        sfx = GameObject.Find("SFX").GetComponent<SFX>();
        //array
        m_allTiles = new Tile[width, height];
        m_allDots = new Dot[width, height];
        SetupTiles();
        SetupDots();
        SetupCamera();
        FillRandom(fillOffset, swapTime);
        m_particleManager = GameObject.FindWithTag("ParticleManager").GetComponent<ParticleManager>();
    }

    private void Update()
    {
        switch (status)
        {
           case GameStatus.Finished:
                break;
        }
    }

    void MakeTile(GameObject prefap,int x, int y,int z=0)
    {
        if (prefap != null)
        {
            //spawn tile
            GameObject tile = Instantiate(prefap, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            m_allTiles[x, y] = tile.GetComponent<Tile>();
            //cloning naming
            tile.name = "Tile (" + x + "," + y + ")";
            //transform position
            tile.transform.parent = transform;
            //store tile on x or y index
            m_allTiles[x, y].Init(x, y, this);
        }
    }

    //init
    void SetupTiles()
    {
        //play sfx
        sfx.GetSFX(0);

        foreach (StartingTile sTile in startingTile)
        {
            if (sTile != null)
            {
                MakeTile(sTile.tilePrefap, sTile.x, sTile.y, sTile.z);
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(m_allTiles[i,j] == null)
                {
                    MakeTile(tileNormalPrefab, i, j);
                }
                
            }
        }
    }


    void SetupDots()
    {
        foreach (StartingTile sDot in startingDots)
        {
            if(sDot != null)
            {
                GameObject dot = Instantiate(sDot.tilePrefap, new Vector3(sDot.x, sDot.y, 0), Quaternion.identity)
                    as GameObject;
                MakeGameDot(dot, sDot.x, sDot.y, fillOffset, swapTime);
            }
        }
    }
    

    void SetupCamera()
    {
        //make center camera
        Camera.main.transform.position = new Vector3((float)width / 2.5f, (float)height / 2.5f, -10f);
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float verticalSize = (float)height / 2f + (float)borderSize;
        float horizontalSize = ((float)width /2f +(float)borderSize)/ aspectRatio;
        
        //compare size of camera
        if (verticalSize > horizontalSize)
        {
            Camera.main.orthographicSize = verticalSize;
        }
        else
        {
            Camera.main.orthographicSize = horizontalSize;
        }
    }

    //random dots

    GameObject GetRandomGenerator()
    {
        int random = UnityEngine.Random.Range(0, dotPrefaps.Length);

        if (dotPrefaps[random]==null)
        {
            print("error");
        }

        return dotPrefaps[random];
    }

    //placing dot and set the cordinates
    public void PlaceDotGame(Dot dot,int x, int y)
    {
        if (dot == null)
        {
            Debug.Log("Board = invalid piece");
            return;
        }
        dot.transform.position = new Vector3(x, y, 0);
        dot.transform.rotation = Quaternion.identity;
        if (IsWithinBounds(x, y))
        {
            m_allDots[x, y] = dot;
        }
        dot.SetCord(x, y);
    }

    //Checking inside of border
    bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < width && y < height && y >= 0);
    }

    void MakeGameDot (GameObject prefap,int x, int y, int falseOfset = 0,float moveTime = 0.1f)
    {
        if (prefap != null&& IsWithinBounds(x,y))
        {
            prefap.GetComponent<Dot>().Init(this);
            PlaceDotGame(prefap.GetComponent<Dot>(), x, y);

            if (falseOfset != 0)
            {
                prefap.transform.position = new Vector3(x, y + falseOfset, 0);
                prefap.GetComponent<Dot>().Move(x, y, moveTime);
            }
            prefap.transform.parent = transform;
        }
    }

    Dot FillRandomAt(int x, int y, int falseOfset = 0,float moveTime =0.1f)
    {
        if (IsWithinBounds(x, y))
        {
            GameObject randomDot = Instantiate(GetRandomGenerator(), Vector3.zero, Quaternion.identity) as GameObject;
            MakeGameDot(randomDot, x, y, falseOfset, moveTime);
            return randomDot.GetComponent<Dot>();
        }
       

        return null;
    }

    bool HasMatchonFill ( int x, int y , int minLength = 3)
    {
        List<Dot> leftMatches = FindMatches(x, y, new Vector2(-1, 0), minLength);
        List<Dot> downMatches = FindMatches(x, y, new Vector2(0, -1), minLength);
        if (leftMatches == null)
        {
            leftMatches = new List<Dot>();
        }
        if (downMatches == null)
        {
            downMatches = new List<Dot>();
        }
        return (leftMatches.Count>0 || downMatches.Count>0);
    }

    //call getrandomgenerator function and fill it into border
    void FillRandom(int falseOfset = 0,float moveTime=0.1f)
    {
        int maxInter = 100;
        int inter = 0;

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                //fill only empty space
                if (m_allDots[i, j] == null)
                {
                    Dot dot = FillRandomAt(i, j,falseOfset,moveTime);
                    inter = 0;
                    //check dot 
                    while (HasMatchonFill(i, j))
                    {
                        ClearDotsAt(i, j);
                        dot = FillRandomAt(i, j,falseOfset,moveTime);
                        inter++;
                        //pretend to loop forever
                        if (inter >= maxInter)
                        {
                            break;
                        }
                    }
                }
                
            }
        }
    }

   

    public void ClickTile(Tile tile)
    {
        if (m_clickedTile == null)
        {
            m_clickedTile = tile;
            //Debug.Log("clicked tile:" + tile.name);
        }
    }

    public void DragToTile(Tile tile)
    {
        if (m_clickedTile != null&& IsNextTo(tile,m_clickedTile))
        {
            m_targetTile = tile;
        }
    }

    public void ReleaseTile()
    {
        if (m_clickedTile != null && m_targetTile != null)
        {
            SwitchTiles(m_clickedTile, m_targetTile);
        }
        m_targetTile = null;
        m_clickedTile = null;

    }

    void SwitchTiles(Tile clickedTile, Tile targetTile)
    {
        StartCoroutine(SwitchTilesRoutine(clickedTile, targetTile));
    }

    IEnumerator SwitchTilesRoutine(Tile clickedTile,Tile targetTile)
    {
        if (m_playerInput)
        {
            Dot clickedDots = m_allDots[clickedTile.GetXIndex(), clickedTile.GetYIndex()];
            Dot targetDots = m_allDots[targetTile.GetXIndex(), targetTile.GetYIndex()];

            //switch dots
            if (targetDots != null && clickedDots != null)
            {
                clickedDots.Move(targetTile.GetXIndex(), targetTile.GetYIndex(), swapTime);
                targetDots.Move(clickedTile.GetXIndex(), clickedTile.GetYIndex(), swapTime);

                yield return new WaitForSeconds(swapTime);

                List<Dot> clickedDotMatches = FindMatchesAt(clickedTile.GetXIndex(), clickedTile.GetYIndex());
                List<Dot> targetDotMatches = FindMatchesAt(targetTile.GetXIndex(), targetTile.GetYIndex());

                //doesn't matches dot
                if (targetDotMatches.Count == 0 && clickedDotMatches.Count == 0)
                {
                    //play sfx
                    sfx.GetSFX(6);
                    clickedDots.Move(clickedTile.GetXIndex(), clickedTile.GetYIndex(), swapTime);
                    targetDots.Move(targetTile.GetXIndex(), targetTile.GetYIndex(), swapTime);
                }
                else
                {
                    yield return new WaitForSeconds(swapTime);

                    ClearAndRefillBoard(clickedDotMatches.Union(targetDotMatches).ToList());

                }

            }
        }     
    }



    //while checking near of dots,it can be drag
    bool IsNextTo(Tile start,Tile end)
    {
        if(Math.Abs(start.GetXIndex()- end.GetXIndex()) ==1&&start.GetYIndex() == end.GetYIndex())
        {
            return true;
        }
        if (Math.Abs(start.GetYIndex() - end.GetYIndex()) == 1 && start.GetXIndex() == end.GetXIndex())
        {
            return true;
        }
        return false;
    }

    //searching  method
    List<Dot> FindMatches(int startX,int startY,Vector2 searchDirection, int minLength=3)
    {
        List<Dot> macthes = new List<Dot>();
        Dot startDot=null;
        if (IsWithinBounds(startX, startY))
        {
            startDot = m_allDots[startX, startY];
        }
        if (startDot != null)
        {
            macthes.Add(startDot);
        }
        else
        {
            return null;
        }

        int nextX;
        int nextY;

        int maxValue = (width > height) ? width : height;
        for(int i = 1; i < maxValue - 1; i++)
        {
            nextX = startX + (int)Mathf.Clamp(searchDirection.x, -1, 1) * i;
            nextY = startY + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i;

            if (!IsWithinBounds(nextX, nextY))
            {
                break;
            }
            Dot nextDot = m_allDots[nextX, nextY];

            if (nextDot == null)
            {
                break;
            }
            else
            {
                //checking next dot, if there's any same color. and then add to list
                if (nextDot.matchvalue == startDot.matchvalue && !macthes.Contains(nextDot))
                {
                    macthes.Add(nextDot);
                }
                else
                {   
                    break;
                }
            }
          

        }
        if (macthes.Count >= minLength)
        {
            return macthes;
        }
        return null;
    }

    //Matches for vertical
    List<Dot> FindVerticalMatches(int startX, int startY, int minLength= 3)
    {
        List<Dot> upwardMatches = FindMatches(startX, startY, new Vector2(0, 1), 2);
        List<Dot> downwardMatches = FindMatches(startX, startY, new Vector2(0, -1), 2);
        if (upwardMatches == null)
        {
            upwardMatches = new List<Dot>();
        }
        if (downwardMatches == null)
        {
            downwardMatches = new List<Dot>();
        }
        //combine using linq 
        var combineMatches = upwardMatches.Union(downwardMatches).ToList();
        return (combineMatches.Count >= minLength) ? combineMatches : null;
    }

    //Matches for horizontal
    List<Dot> FindHorizontalMatches(int startX, int startY, int minLength = 3)
    {
        List<Dot> rightMatches = FindMatches(startX, startY, new Vector2(1, 0), 2);
        List<Dot> leftMatches = FindMatches(startX, startY, new Vector2(-1, 0), 2);
       
        if (rightMatches == null)
        {
            rightMatches = new List<Dot>();
        }
        if (leftMatches == null)
        {
            leftMatches = new List<Dot>();
        }
        //combine 2 list using system.linq 
        var combineMatches = rightMatches.Union(leftMatches).ToList();
        return (combineMatches.Count >= minLength) ? combineMatches : null;
    }


    List<Dot> FindMatchesAt(int x, int y, int minLength = 3)
    {
        List<Dot> horizMatches = FindHorizontalMatches(x, y, minLength);
        List<Dot> vertMatches = FindVerticalMatches(x, y, minLength);
        
        if (horizMatches == null)
        {
            horizMatches = new List<Dot>();
        }
        if (vertMatches == null)
        {
            vertMatches = new List<Dot>();
        }
        //combine 2 list using system.linq 
        var combineMatches = horizMatches.Union(vertMatches).ToList();
        return combineMatches;
    }

    //overload FindMatchesAt
    List<Dot> FindMatchesAt(List<Dot>dots,int minLength=3)
    {
        List<Dot> matches = new List<Dot>();
        foreach (Dot dot in dots)
        {
            matches = matches.Union(FindMatchesAt(dot.xIndex, dot.yIndex, minLength)).ToList();
        }
        return matches;
    }

    private List<Dot> FindAllMatches()
    {
        List<Dot> combineMatches = new List<Dot>();
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                List<Dot> matches = FindMatchesAt(i, j);
                combineMatches = combineMatches.Union(matches).ToList();
            }
        }
        return combineMatches;
    }

    //default tile to give color transparent
    void HighlistOff(int x, int y)
    {
        if(m_allTiles[x,y].GetTileType() != TileType.Breakable)
        {
            SpriteRenderer spriterender = m_allTiles[x, y].GetComponent<SpriteRenderer>();
            spriterender.color = new Color(spriterender.color.r, spriterender.color.g, spriterender.color.b, 0);
        }

    }
    
    //change color while highlight 3 matches
    void HighlistTileOn(int x, int y,Color col)
    {
        if (m_allTiles[x, y].GetTileType() != TileType.Breakable)
        {
            SpriteRenderer spriterender = m_allTiles[x, y].GetComponent<SpriteRenderer>();
            spriterender.color = col;
        }
    }

    void HighlightMatchesAt(int x, int y)
    {
        HighlistOff(x, y);

        var combineMatches = FindMatchesAt(x, y);

        //while found matches
        if (combineMatches.Count > 0)
        {
            foreach (Dot dot in combineMatches)
            {
                HighlistTileOn(dot.xIndex, dot.yIndex, dot.GetComponent<SpriteRenderer>().color);
            }
        }
    }

    void HighlightMatches()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                HighlightMatchesAt(i, j);
            }
        }
    }

    void HighlightDots(List<Dot> dots)
    {
        foreach(Dot dot in dots)
        {
            if (dot != null)
            {
                HighlistTileOn(dot.xIndex, dot.yIndex, dot.GetComponent<SpriteRenderer>().color);
            }
        }
    }

    //clear while matched
    void ClearDotsAt(int x, int y)
    {
        Dot dotToClear = m_allDots[x, y];

        if (dotToClear != null)
        {
            m_allDots[x, y] = null;
            Destroy(dotToClear.gameObject);
        }
        
    }

    //clear board
    void ClearBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                ClearDotsAt(i, j);
            }
        }
    }

    void ClearDotsAt(List<Dot> dots)
    {
        foreach(Dot dot in dots)
        {
            if (dot != null)
            {
                ClearDotsAt(dot.xIndex, dot.yIndex);
                //add value score
                dot.ScorePoint();
                
                if (m_particleManager != null)
                {
                   m_particleManager.ClearDotFxAt(dot.xIndex, dot.yIndex);
                }
            }
        }
        //play sfx
        sfx.GetSFX(2);
    }

    void BreakTileAt(int x, int y)
    {
        Tile tileToBreak = m_allTiles[x, y];

        if (tileToBreak != null&& tileToBreak.GetTileType() == TileType.Breakable)
        {
            if (m_particleManager != null)
            {
                m_particleManager.BreakDotFxAT(tileToBreak.GetBreakAbleValue(), x, y, 0);
            }
            tileToBreak.BreakTile();
        }
        //play sfx
        sfx.GetSFX(3);
    }

    void BreakTileAt(List<Dot> dots)
    {
        foreach(Dot dot in dots)
        {
            if (dot != null)
            {
                BreakTileAt(dot.xIndex, dot.yIndex);
            }
        }
    }


    List<Dot> CollapseCollumn(int collumn, float CollapseTime = 0.1f)
    {
        List<Dot> movingDot = new List<Dot>();
        
        for(int i = 0; i < height - 1; i++)
        {
            //check upper of dot
            if (m_allDots[collumn, i] == null)
            {
                for(int j = i + 1; j < height; j++)
                {
                    //check upper of dot
                    if (m_allDots[collumn, j] != null)
                    {
                        //move dot to empty space
                        m_allDots[collumn, j].Move(collumn, i, CollapseTime * (j-i));

                        m_allDots[collumn, i] = m_allDots[collumn, j];
                        //set cordinate 
                        m_allDots[collumn, i].SetCord(collumn, i);

                        if (!movingDot.Contains(m_allDots[collumn, i]))
                        {
                            movingDot.Add(m_allDots[collumn, i]);
                        }
                        m_allDots[collumn, j] = null;
                        break;
                    }
                }
            }
        }
        return movingDot;

    }

    //overlap CollapseCollumn method
    List<Dot> CollapseCollumn(List<Dot> dots)
    {
        List<Dot> movingDots = new List<Dot>();
        List<int> columnsTocollapse = GetCollumns(dots);

        foreach (int collumn in columnsTocollapse)
        {
            movingDots = movingDots.Union(CollapseCollumn(collumn)).ToList();
        }
        return movingDots;
    }

    List<int> GetCollumns(List<Dot> dots)
    {
        List<int> columns = new List<int>();
        foreach (Dot dot in dots)
        {
            if (!columns.Contains(dot.xIndex))
            {
                columns.Add(dot.xIndex);
            }
        }
        return columns;
    }

    void ClearAndRefillBoard(List<Dot> dots)
    {
        StartCoroutine(ClearAndRefillBoardRoutine(dots));
    }

    IEnumerator ClearAndRefillBoardRoutine(List<Dot> dots)
    {
        m_playerInput = false;
        List<Dot> matches = dots;

        
        //do{
            //clear and collapse
            yield return StartCoroutine(ClearAndCollapseRoutine(dots));
            yield return null;

            //refill
            yield return StartCoroutine(RefillRoutine());
            matches = FindAllMatches();
            //refill time
            yield return new WaitForSeconds(0.5f);
        //} while (matches.Count != 0);
        m_playerInput = true;

    }

    IEnumerator RefillRoutine()
    {
        FillRandom(fillOffset, swapTime);
        yield return null;
    }

    //Match dot and clear it
    IEnumerator ClearAndCollapseRoutine(List<Dot> dots)
    {
        //List Matches dot
        List<Dot> movingDots = new List<Dot>();
        List<Dot> matches = new List<Dot>();

        //HighlightDots(dots);

        //delay before destroy
        //yield return new WaitForSeconds(0.25f);
        bool isFinished = false;
        while (!isFinished)
        {
            //find dot affected by bombs
            List<Dot> bombDots = GetBombedDots(dots);
            dots = dots.Union(bombDots).ToList();

            //normal
            ClearDotsAt(dots);
            BreakTileAt(dots);

            //effect time delay
            yield return new WaitForSeconds(0.1f);

            movingDots = CollapseCollumn(dots);
            while (!isCollapsed(movingDots))
            {
                yield return null;
            }

            //delay after collapse
            yield return new WaitForSeconds(0.01f);
            matches = FindMatchesAt(movingDots);

            //After collapse and then matches empty
            if (matches.Count == 0)
            {
                isFinished = true;
                break;
            }
            else{
                yield return StartCoroutine(ClearAndCollapseRoutine(matches));
            }

        }
        yield return null;
    }


    bool isCollapsed (List<Dot> dots)
    {
        foreach(Dot dot in dots)
        {
            if (dot.transform.position.y - (float)dot.yIndex > 0.001f)
            {
                return false;
            }
        }
        return true;
    }

    //Check 1 row dot
    List<Dot> GetRowDot(int row)
    {
        List<Dot> dots = new List<Dot>();
        for (int i = 0; i < width; i++)
        {
            if (m_allDots[i, row] != null)
            {
                dots.Add(m_allDots[i, row]);
            }
        }
        return dots;
    }

    //Check 1 column dot
    List<Dot> GetColumnDot(int column)
    {
        List<Dot> dots = new List<Dot>();
        for (int i = 0; i < height; i++)
        {
            if (m_allDots[column,i ] != null)
            {
                dots.Add(m_allDots[column,i]);
            }
        }
        return dots;
    }

    //Check Adjacent 3x3
    List<Dot> GetAdjacent(int x, int y, int offset = 1)
    {
        List<Dot> dots = new List<Dot>();
        for(int i = x - offset; i <= x + offset; i++)
        {
            for(int j = y - offset; j <= y + offset; j++)
            {
                if (IsWithinBounds(i, j))
                {
                    dots.Add(m_allDots[i,j]);
                }
            }
        }
        return dots;
    }

    List<Dot> GetBombedDots(List<Dot> dots)
    {
        List<Dot> allDotsToClear = new List<Dot>();
        foreach (Dot dot in dots)
        {
            if (dot != null)
            {
                List<Dot> dotsToClear = new List<Dot>();
                Bomb bomb = dot.GetComponent<Bomb>();
                if (bomb != null)
                {
                    switch (bomb.bombType)
                    {
                        case BombType.Column:
                            dotsToClear = GetColumnDot(bomb.yIndex);
                            break;
                        case BombType.Row:
                            dotsToClear = GetRowDot(bomb.xIndex);
                            break;
                        case BombType.Adjacent:
                            dotsToClear = GetAdjacent(bomb.xIndex, bomb.yIndex, 1);
                            break;
                    }
                    allDotsToClear = allDotsToClear.Union(dotsToClear).ToList();
                    //play sfx
                    sfx.GetSFX(4);
                }
            }
          
        }
        return allDotsToClear;
       
    }

    public GameStatus GetGameStatus()
    {
        return status;
    }
    
    public void SetGameStatus(GameStatus value)
    {
        status = value;
    }
}
