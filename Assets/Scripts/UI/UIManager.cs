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
    public GameObject Button_CancelCardPlacement;
    public GameObject Button_EnterUnitBattle;
    public GameObject GameOverScreen;
    public GameObject GameWonScreen;

    private void Awake()
    {
        Instance = this;
    }

    public void SetP1HP (int amount)
    {
        P1HP.text = "P1HP = " + amount;
    }

    public void SetP2HP(int amount)
    {
        P2HP.text = "P2HP = " + amount;
    }

    public void SetPhase(string phase)
    {
        Phase.text = phase;
    }

    public void GameOver ()
    {
        GameOverScreen.SetActive(true);
    }

    public void GameWon()
    {
        GameWonScreen.SetActive(true);
    }

    public void EnterCardSelection ()
    {
        Button_EnterUnitBattle.SetActive(true);
        Button_CancelCardPlacement.SetActive(false);
    }

    public  void EnterCardPlacement ()
    {
        Button_EnterUnitBattle.SetActive(false);
        Button_CancelCardPlacement.SetActive(true);
    }

    public void EnterUnitControl ()
    {
        Button_EnterUnitBattle.SetActive(false);
    }
}