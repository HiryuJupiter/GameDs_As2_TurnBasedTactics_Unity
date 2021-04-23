using System.Collections;
using UnityEngine;

public class DummyBoardTile : MonoBehaviour
{
    [SerializeField] bool isPlayer1Tile;

    Material material;
    Color defaultColor;
    public bool IsPlayerOneSide => isPlayer1Tile;

    public void SetSide(bool isPlayer1Tile) =>
        this.isPlayer1Tile = isPlayer1Tile;

    public void Highlight (bool requestedByPlayer1)
    {
        if (requestedByPlayer1 == isPlayer1Tile)
        {
            material.color = Color.white;
        }
    }

    public void ExitHighlight(bool requestedByPlayer1)
    {
        if (requestedByPlayer1 == isPlayer1Tile)
        {
            material.color = defaultColor;
        }
    }

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        defaultColor = material.color;
    }
}