using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Text P1HP;
    public Text P2HP;
    public Text Phase;
    public GameObject CancelPlacementButton;

    private void Awake()
    {
        Instance = this;
    }

    public void SetP1HP (string amount)
    {
        P1HP.text = amount;
    }

    public void SetP2HP(string amount)
    {
        P2HP.text = amount;
    }

    public void SetPhase(string phase)
    {
        Phase.text = phase;
    }

    public void SetActive_PlacementButton (bool isActive) => 
        CancelPlacementButton.SetActive(isActive);
}