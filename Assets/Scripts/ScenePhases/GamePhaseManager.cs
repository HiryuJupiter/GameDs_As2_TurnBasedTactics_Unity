using System;
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GamePhaseManager : MonoBehaviour
{
    //Const
    private const float TimeBeforeGameStart = 0.5f;

    #region Reference
    UIManager uiM;
    #endregion

    //Variables
    public static GamePhaseManager Instance { get; private set; }

    [SerializeField] private Player player1;
    [SerializeField] private Player player2;

    //Status

    //Properties
    public static GamePhases Phase { get; private set; } = GamePhases.Standby;
    public static int Turn { get; private set; } = 0;

    #region Mono
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        uiM = UIManager.Instance;
        GoToPhase(GamePhases.p1_GameStartFillDeck);
    }
    #endregion

    #region Public - phase transition requests
    public void PlayerFinishedControlPhase()
    {
        GoToPhase(GamePhases.p4_AIControlPhase);
    }

    public void AIFinishedControlPhase()
    {
        GoToPhase(GamePhases.TurnCompleteEvaluation);
    }
    #endregion

    #region Phase transition 

    void GoToPhase(GamePhases newState)
    {
        if (Phase != newState)
        {
            Debug.Log("--- Game phase switch from [" + Phase + "] to [" + newState + "] ---");
            Phase = newState;
            uiM.SetPhase(Phase.ToString());

            switch (Phase)
            {
                case GamePhases.p1_GameStartFillDeck:
                    StartCoroutine(P1_FillDeck());
                    break;
                case GamePhases.p2_DrawCard:
                    StartCoroutine(P2_DrawHand());
                    break;
                case GamePhases.p3_PlayerControl:
                    break;
                case GamePhases.p4_AIControlPhase:
                    break;
                case GamePhases.TurnCompleteEvaluation:
                    StartCoroutine(TurnCompleteEvaluation());
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #region Phases
    IEnumerator P1_FillDeck()
    {
        player1.FillDeck();
        player2.FillDeck();

        yield return new WaitForSeconds(0.6f);
        while (!IsDeckStill)
            yield return null;

        GoToPhase(GamePhases.p2_DrawCard);
    }

    IEnumerator P2_DrawHand()
    {
        player1.DrawHand();
        player2.DrawHand();

        yield return new WaitForSeconds(0.5f);
        while (!IsHandStill)
            yield return null;

        GoToPhase(GamePhases.p3_PlayerControl);
    }

    IEnumerator TurnCompleteEvaluation()
    {
        GoToPhase(GamePhases.p1_GameStartFillDeck);
        yield return null;
    }
    #endregion

    /*
     * PHASE 0 - GameStartStandby
     * - Play "Game Start" splash
     * 
     * PHASE 1 - Drawing
     * - Player 1 automatically draw cards
     * - Player 2 automatically draw cards
     * 
     * PHASE 3 - UnitMoving
     * PHASE 4 - UnitFighting
     * PHASE 5 - TurnCompleteEvaluation
     * 
     */

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 200, 20, 200, 20), "State: " + Phase);
    }

    #region Minor expressions
    bool IsHandStill => player1.Hand.AreCardsStill() && player2.Hand.AreCardsStill();
    bool IsDeckStill => player1.PlayerDeck.AreCardsInDeckStill() && player2.PlayerDeck.AreCardsInDeckStill();
    #endregion
}