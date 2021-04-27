using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    #region Field and mono
    //public static BoardManager Instance;

    [SerializeField] BoardTile playerTile;
    [SerializeField] BoardTile enemyTile;
    [SerializeField] float tileSize = 1f;
    [SerializeField] int xCount = 4;
    [SerializeField] int zCount = 4;
    Vector3 startPoint;
    float halfTileSize;
    int halfXCount;
    int halfZCount;
    public BoardTile[,] tiles { get; private set; }

    private void Awake()
    {
        //Instance = this;
        tiles = new BoardTile[xCount, zCount];
    }

    private void Start()
    {
        //Cache
        halfTileSize = tileSize / 2f;
        halfXCount = xCount / 2;
        halfZCount = zCount / 2;
        float startx = -halfXCount * tileSize;
        float startz = -halfZCount * tileSize;
        startPoint = new Vector3(startx, 0f, startz);
        Debug.DrawLine(Vector3.zero, startPoint, Color.white, 10f);
        Debug.DrawLine(Vector3.zero, -startPoint, Color.red, 10f);

        //Start
        FillBoard();
    }
    #endregion

    #region Public
    public bool TryGetTile (int x, int y, out BoardTile tile)
    {
        tile = null;
        if (x >= 0 && x < tiles.GetLength(0) &&
            y >= 0 && y < tiles.GetLength(1))
        {
            tile = tiles[x, y];
            return true;
        }
        else
        {
            Debug.LogError("tile index out of bounds");
            return false;
        }
    }
    #endregion

    #region Board generation
    void FillBoard()
    {
        for (int x = 0; x < xCount; x++)
        {
            for (int z = 0; z < zCount; z++)
            {
                BoardTile pf = z < halfZCount ? playerTile : enemyTile;
                BoardTile t = Instantiate(pf, GetTileWorldPos(x, z), 
                    playerTile.transform.rotation);
                t.SetIndex = new Vector2Int(x, z);

                tiles[x, z] = t;
            }
        }
    }
    #endregion

    #region Minor
    public Vector3 GetTileWorldPos(int x, int z)
    {
        return startPoint + new Vector3(halfTileSize + tileSize * x, 0f,
            halfTileSize + tileSize * z);
    }
    #endregion
}

/*
 for (int x = 0; x < board.tiles.GetLength(0); x++)
        {
            for (int z = 0; z < board.tiles.GetLength(1); z++)
            {
            }
        }
 */