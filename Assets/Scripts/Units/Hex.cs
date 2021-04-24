using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    [SerializeField]
    public bool blue;
    [SerializeField]
    public bool spawnable;
    public bool highlighted;
    public Material highlight;
    public Material teamColour;
    public Transform attachPoint;


    private void Update()
    {
        if (highlighted)
        {
            this.GetComponent<Renderer>().material = highlight;
        }
        else
        {
            this.GetComponent<Renderer>().material = teamColour;
        }
    }
    private void OnMouseExit()
    {
        highlighted = false;

    }



}
