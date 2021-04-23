using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBoardManager : MonoBehaviour
{
    [SerializeField] DummyBoardTile playerTile;
    [SerializeField] DummyBoardTile enemyTile;
    [SerializeField] float tileSize = 1f;
    [SerializeField] int xCount = 4;
    [SerializeField] int zCount = 4;
    Vector3 startPoint;
    float halfTileSize;
    int halfXCount;
    int halfZCount;
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

    void FillBoard()
    {
        for (int x = 0; x < xCount; x++)
        {
            for (int z = 0; z < zCount; z++)
            {
                DummyBoardTile pf = z < halfZCount ? playerTile : enemyTile;
                Instantiate(pf, GetTileWorldPos(x, z), playerTile.transform.rotation);
            }
        }
    }

    Vector3 GetTileWorldPos(int x, int z)
    {
        return startPoint + new Vector3(halfTileSize + tileSize * x, 0f,
            halfTileSize + tileSize * z);
    }
}