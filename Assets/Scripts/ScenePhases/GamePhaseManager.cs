using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame
{
    [DefaultExecutionOrder(-100)]
    public class GamePhaseManager : MonoBehaviour
    {
        //Const
        private const float TimeBeforeGameStart = 0.5f;
        private const float P1P2PhaseDelay = 0.01f;

        //Variables
        public static GamePhaseManager Instance { get; private set; }

        [SerializeField] private Player player1;
        [SerializeField] private Player player2;

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
                switch (Phase)
                {
                    //case GamePhases.GameStartFillDeck:
                    //    break;
                    case GamePhases.DrawCard:
                        StartCoroutine(WaitForCardsToBeDrawn());
                        break;
                    case GamePhases.PlayingHand:
                        break;
                    case GamePhases.UnitMoving:
                        break;
                    case GamePhases.UnitFighting:
                        break;
                    case GamePhases.TurnCompleteEvaluation:
                        break;
                }
            }
        }

        #region Phase 0 - Game Start Fill Deck
        private IEnumerator Phase0Start_FillDeck()
        {
            yield return new WaitForSeconds(TimeBeforeGameStart);
            StartCoroutine(WaitForP1ToFillDeck());
            yield return new WaitForSeconds(P1P2PhaseDelay);
            StartCoroutine(WaitForP2ToFillDeck());

            //while (!p1DeckFilled)
            while (!p1DeckFilled || !p2DeckFilled)
                yield return null;

            while (!player2.PlayerDeck.AreAllDeckCardsStill())
                yield return null;

            //yield return new WaitForSeconds(0.5f);
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
        private IEnumerator WaitForCardsToBeDrawn()
        {
            StartCoroutine(WaitForP1ToDraw());
            yield return new WaitForSeconds(P1P2PhaseDelay);
            StartCoroutine(WaitForP2ToDraw());

            //while (!p1Drawn)
            while (!p1Drawn || !p2Drawn)
                yield return null;

            while (!player2.PlayerDeck.AreAllHandCardsStill())
                yield return null;

            //yield return new WaitForSeconds(0.5f);

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