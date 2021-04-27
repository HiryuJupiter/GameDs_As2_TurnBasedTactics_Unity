using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] Text P1HP;
    [SerializeField] Text Wave;
    [SerializeField] Text Gold;
    [SerializeField] Text Phase;
    [SerializeField] GameObject Button_CancelCardPlacement;
    [SerializeField] GameObject Button_BuyCard;
    [SerializeField] GameObject Button_ToCombat;
    [SerializeField] GameObject GameOverScreen;
    [SerializeField] GameObject GameWonScreen;

    private void Awake()
    {
        Instance = this;
    }

    public void SetP1HP (int amount)
    {
        P1HP.text = "HP = " + amount;
    }

    public void SetScore(int wave)
    {
        Wave.text = "KILLS = " + wave;
    }

    public void SetGold(int gold)
    {
        Gold.text = "$ = " + gold;
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
        Button_ToCombat.SetActive(true);
        Button_CancelCardPlacement.SetActive(false);
    }

    public void EnterCardPlacement ()
    {
        Button_ToCombat.SetActive(false);
        Button_CancelCardPlacement.SetActive(true);
        ToggleBuyCardButton(false);
    }

    public void EnterUnitControl()
    {
        Button_ToCombat.SetActive(false);
        ToggleBuyCardButton(false);
    }

    public void ToggleBuyCardButton (bool isVisible)
    {
        Button_BuyCard.SetActive(isVisible);
    }


    private void Update()
    {
        //debug end screen
        if (Input.GetKeyDown(KeyCode.Alpha9))
            GameOver();
        if (Input.GetKeyDown(KeyCode.Alpha0))
            GameWon();
    }
}