using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame
{
    public class ScenePhaseManager : MonoBehaviour
    {
        //Const
        private const float TimeBeforeGameStart = 0.2f;

        //Private variable
        [SerializeField] private Player player1;
        private Dictionary<ScenePhases, Action> StateMethod;

        //Properties
        public static ScenePhases State { get; private set; } = ScenePhases.GameStartStandby;
        public static int Turn{ get; private set; } = 0;

        //Events
        public delegate void Phase1Start_DrawCards();
        public delegate void Phase2Start_PlayingHand();
        public delegate void Phase3Start_UnitMoving();
        public delegate void Phase4Start_UnitFighting();
        public delegate void Phase5Start_TurnComplete();

        public static event Phase1Start_DrawCards OnPhase1Start_DrawCards;
        public static event Phase2Start_PlayingHand OnPhase2Start_PlayingHand;
        public static event Phase3Start_UnitMoving OnPhase3Start_UnitMoving;
        public static event Phase4Start_UnitFighting OnPhase4Start_UnitFighting;
        public static event Phase5Start_TurnComplete OnPhase5Start_TurnComplete;

        #region Public
        public void GoToState(ScenePhases newState)
        {
            if (State != newState)
            {
                State = newState;
                StateMethod[newState].Invoke();
                Debug.Log("--- Switched from [" + State + "] to [" + newState + "] ---");
            }
        }
        #endregion

        #region Mono
        private IEnumerator Start()
        {
            //Initialization
            //Note: I'm just experimenting with a weird form of state machine, just replace this if
            //you have a different idea.
            StateMethod = new Dictionary<ScenePhases, Action>()
            {
                {ScenePhases.DrawCard,                () => OnPhase1Start_DrawCards?.Invoke()},
                {ScenePhases.PlayingHand,            () => OnPhase2Start_PlayingHand?.Invoke()},
                {ScenePhases.UnitMoving,             () => OnPhase3Start_UnitMoving?.Invoke()},
                {ScenePhases.UnitFighting,           () => OnPhase4Start_UnitFighting?.Invoke()},
                {ScenePhases.TurnCompleteEvaluation, () => OnPhase5Start_TurnComplete?.Invoke()},
            };

            //Delay before start game
            yield return new WaitForSeconds(TimeBeforeGameStart);
            GoToState(ScenePhases.DrawCard);
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(Screen.width - 200, 20, 200, 20), "State: " + State);
        }
        #endregion

        #region States
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
    }
}