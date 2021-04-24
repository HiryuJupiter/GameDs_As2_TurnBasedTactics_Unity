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

    [SerializeField] private RealPlayer player1;
    [SerializeField] private Player player2;

    //Status

    //Properties
    public RealPlayer Player1 => player1;
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
        GoToPhase(GamePhases.phase1_GameStartFillDeck);
    }
    #endregion

    #region Public - phase transition requests
    public void ToP3_CardSelection()
    {
        uiM.EnterCardSelection();
        GoToPhase(GamePhases.phase3_CardSelection);
    }
    public void ToP4_CardPlacementPhase()
    {
        uiM.EnterCardPlacement();
        GoToPhase(GamePhases.phase4_Placement);
    }
    public void ToP5_UnitControlMode()
    {
        uiM.EnterUnitControl();
        GoToPhase(GamePhases.phase5_UnitControl);
    }

    public void ToP6_AISequence() => GoToPhase(GamePhases.phase6_AIControlPhase);
    public void To_Evaluation() => GoToPhase(GamePhases.TurnCompleteEvaluation);
    #endregion

    #region Phase transition 
    void GoToPhase(GamePhases newPhase)
    {
        if (Phase != newPhase)
        {
            //Debug.Log("[" + Phase + "] to [" + newPhase + "] ---");
            Phase = newPhase;
            uiM.SetPhase(Phase.ToString());

            switch (Phase)
            {
                case GamePhases.phase1_GameStartFillDeck:
                    StartCoroutine(Phase1_FillDeck());
                    break;
                case GamePhases.phase2_DrawCard:
                    StartCoroutine(Phase2_DrawHand());
                    break;
                case GamePhases.phase3_CardSelection:
                    break;
                case GamePhases.phase5_UnitControl:
                    break;
                case GamePhases.phase6_AIControlPhase:
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
    IEnumerator Phase1_FillDeck()
    {
        player1.FillDeck();
        player2.FillDeck();

        yield return new WaitForSeconds(0.6f);
        while (!IsDeckStill)
            yield return null;

        GoToPhase(GamePhases.phase2_DrawCard);
    }

    IEnumerator Phase2_DrawHand()
    {
        player1.DrawHand();
        player2.DrawHand();

        yield return new WaitForSeconds(0.5f);
        while (!IsHandStill)
            yield return null;

        GoToPhase(GamePhases.phase3_CardSelection);
    }

    IEnumerator TurnCompleteEvaluation()
    {
        GoToPhase(GamePhases.phase1_GameStartFillDeck);
        yield return null;
    }
    #endregion

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 200, 20, 200, 20), "State: " + Phase);
    }

    #region Minor expressions
    bool IsHandStill => player1.Hand.AreCardsStill() && player2.Hand.AreCardsStill();
    bool IsDeckStill => player1.PlayerDeck.AreCardsInDeckStill() && player2.PlayerDeck.AreCardsInDeckStill();
    #endregion
}