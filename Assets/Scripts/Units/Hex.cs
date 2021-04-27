using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;

public class Hex : MonoBehaviour
{
    [SerializeField]
    public bool blue;
    [SerializeField]
    public bool spawnable;
    public bool highlighted;
    public bool movable;
    public bool selected;
    public bool attachedRed;
    public bool oddLane;

    public Material highlight;
    public Material teamColour;
    public Material selectedColour;
    public Material attachedColour;

    public Transform attachPoint;
    public GameObject attachedObject;
    public int horiPoint;
    public int vertPoint;

    public Material material;


    public PlayerUnitOldSchool Unit { get; private set; }
    public Vector2Int Index { get; private set; }
    public bool IsMainPlayer => blue;
    public bool IsOccupied => Unit != null;
    public Vector2Int SetIndex { set { Index = value; } }


    private void Awake()
    {
        //material = GetComponent<Renderer>().material;
    }

    //private void Update()
    //{
    //    if (highlighted || movable)
    //    {
    //        material = highlight;
    //    }
    //    else if (selected)
    //    {
    //        material = selectedColour;
    //    }
    //    else if (attachedRed)
    //    {
    //        material = attachedColour;
    //    }
    //    else
    //    {
    //        material = teamColour;
    //    }
    //}

    public void SetUnitPiece(PlayerUnitOldSchool unit)
    {
        Unit = unit;
    }

    //public void ToggleAttackHighlight(bool isOn)
    //{
    //    material.color = isOn ? Color.red : defaultColor;
    //}

    public void ToggleHoverHighlight(bool isOn)
    {
        GetComponent<Renderer>().material = isOn ? highlight : teamColour;
    }


    //private void OnMouseExit()
    //{
    //    highlighted = false;
    //}
}