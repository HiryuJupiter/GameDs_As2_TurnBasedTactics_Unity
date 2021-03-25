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
        private const float TimeBeforeGameStart = 0.2f;

        //Private variable
        [SerializeField] private Player player1;
        private Dictionary<GamePhases, Action> StateMethod;

        //Properties
        public static GamePhases State { get; private set; } = GamePhases.GameStartStandby;
        public static int Turn{ get; private set; } = 0;

        #region Public
        #endregion

        #region Mono
        private IEnumerator Start()
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
            yield return new WaitForSeconds(TimeBeforeGameStart);
            GoToPhase(GamePhases.DrawCard);
        }
        #endregion

        #region States
        private void GoToPhase(GamePhases newState)
        {
            if (State != newState)
            {
                State = newState;
                StateMethod[newState].Invoke();
                Debug.Log("--- Switched from [" + State + "] to [" + newState + "] ---");
            }
        }

        private void Phase1Start_DrawCards()
        {
            StartCoroutine(WaitForCardsToBeDrawn());
        }

        IEnumerator WaitForCardsToBeDrawn ()
        {
            yield return player1.WaitForCardsToBeDrawn();
            GoToPhase(GamePhases.PlayingHand);
        }

        private void Phase2Start_PlayingHand()
        {

        }

        private void Phase3Start_UnitMoving()
        {

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
        #endregion

        private void OnGUI()
        {
            GUI.Label(new Rect(Screen.width - 200, 20, 200, 20), "State: " + State);
        }
    }
}