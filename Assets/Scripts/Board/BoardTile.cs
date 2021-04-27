using System.Collections;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    [SerializeField] bool isPlayer1Tile;

    Material material;
    Color defaultColor;
    public PlayerUnitOldSchool Unit { get; private set; }
    public Vector2Int Index { get; private set; }
    public bool IsMainPlayer => isPlayer1Tile;
    public bool IsOccupied => Unit != null;
    public Vector2Int SetIndex { set { Index = value;} }

    public void SetUnitPiece (PlayerUnitOldSchool unit)
    {
        Unit = unit;
    }

    public void ToggleAttackHighlight(bool isOn)
    {
        material.color = isOn ? Color.red : defaultColor;
    }

    public void ToggleHoverHighlight(bool isOn)
    {
        material.color = isOn ? Color.white : defaultColor;
    }

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        defaultColor = material.color;
    }
}