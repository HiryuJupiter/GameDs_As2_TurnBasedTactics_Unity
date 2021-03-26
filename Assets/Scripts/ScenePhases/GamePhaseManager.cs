using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame
{
    public class GamePhaseManager : MonoBehaviour
    {
        //Const
        private const float TimeBeforeGameStart = 0.5f;

        //Variables
        public static GamePhaseManager Instance { get; private set; }

        [SerializeField] private Player player1;
        [SerializeField] private Player player2;
        private Dictionary<GamePhases, Action> StateMethod;

        //Status
        private bool p1DeckFilled;
        private bool p2DeckFilled;
        private bool p1Drawn;
        private bool p2Drawn;

        //Properties
        public static GamePhases Phase { get; private set; } = GamePhases.GameStartFillDeck;
        public static int Turn { get; private set; } = 0;

        #region Mono
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            //Initialization
            //Note: I'm just experimenting with a weird form of state machine, just replace this if
            //you have a different idea.
            StateMethod = new Dictionary<GamePhases, Action>()
            {
                {GamePhases.DrawCard,              () => Phase1Start_DrawCards()},
                {GamePhases.PlayingHand,           () => Phase2Start_PlayingHand()},
                {GamePhases.UnitMoving,            () => Phase3Start_UnitMoving()},
                {GamePhases.UnitFighting,          () => Phase4Start_UnitFighting()},
                {GamePhases.TurnCompleteEvaluation,() => Phase5Start_TurnComplete()},
            };

            //Delay before start game
            StartCoroutine(Phase0Start_FillDeck());
        }
        #endregion
       
        private void GoToPhase(GamePhases newState)
        {
            if (Phase != newState)
            {
                Debug.Log("--- Switched from [" + Phase + "] to [" + newState + "] ---");
                Phase = newState;
                StateMethod[newState].Invoke();
            }
        }

        #region Phase 0 - Game Start Fill Deck
        private IEnumerator Phase0Start_FillDeck()
        {
            yield return new WaitForSeconds(TimeBeforeGameStart);
            StartCoroutine(WaitForP1ToFillDeck());
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(WaitForP2ToFillDeck());

            while (!p1DeckFilled|| !p2DeckFilled)
                yield return null;

            GoToPhase(GamePhases.DrawCard);
        }

        private IEnumerator WaitForP1ToFillDeck()
        {
            p1DeckFilled = false;
            yield return player1.PlayerDeck.FillDeck();
            p1DeckFilled = true;
        }

        private IEnumerator WaitForP2ToFillDeck()
        {
            p2DeckFilled = false;
            yield return player2.PlayerDeck.FillDeck();
            p2DeckFilled = true;
        }
        #endregion

        #region Phase 1 - Draw cards
        private void Phase1Start_DrawCards() => StartCoroutine(WaitForCardsToBeDrawn());

        private IEnumerator WaitForCardsToBeDrawn()
        {
            StartCoroutine(WaitForP1ToDraw());
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(WaitForP2ToDraw());

            while (!p1Drawn || !p2Drawn)
                yield return null;

            GoToPhase(GamePhases.PlayingHand);
        }

        private IEnumerator WaitForP1ToDraw()
        {
            p1Drawn = false;
            yield return player1.PlayerHand.WaitForHandToBeDrawn();
            p1Drawn = true;
        }

        private IEnumerator WaitForP2ToDraw()
        {
            p2Drawn = false;
            yield return player2.PlayerHand.WaitForHandToBeDrawn();
            p2Drawn = true;
        }
        #endregion

        #region Phase 2 - Playing hand
        private void Phase2Start_PlayingHand()
        {

        }
        #endregion

        private void Phase3Start_UnitMoving()
        {
            //Disable highlight mode
        }

        private void Phase4Start_UnitFighting()
        {

        }

        private void Phase5Start_TurnComplete()
        {

        }

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
        #region States
        #endregion

        private void OnGUI()
        {
            GUI.Label(new Rect(Screen.width - 200, 20, 200, 20), "State: " + Phase);
        }
    }
}