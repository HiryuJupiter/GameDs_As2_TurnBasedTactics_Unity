using System.Collections;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    [SerializeField] bool isPlayer1Tile;

    Material material;
    Color defaultColor;
    public Vector2Int Index { get; private set; }
    public bool IsMainPlayer => isPlayer1Tile;
    public Vector2Int SetIndex { set { Index = value;} }


    public void EnterHighlight()
    {
        material.color = Color.white;
    }

    public void ExitHighlight()
    {
        material.color = defaultColor;
    }

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        defaultColor = material.color;
    }
}