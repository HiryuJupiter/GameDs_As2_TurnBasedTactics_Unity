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

        #region Public
        #endregion

        #region Mono
        private IEnumerator Start()
        {
            //Initialization
            //Note: I'm just experimenting with a weird form of state machine, just replace this if
            //you have a different idea.
            StateMethod = new Dictionary<ScenePhases, Action>()
            {
                {ScenePhases.DrawCard,              () => Phase1Start_DrawCards()},
                {ScenePhases.PlayingHand,           () => Phase2Start_PlayingHand()},
                {ScenePhases.UnitMoving,            () => Phase3Start_UnitMoving()},
                {ScenePhases.UnitFighting,          () => Phase4Start_UnitFighting()},
                {ScenePhases.TurnCompleteEvaluation,() => Phase5Start_TurnComplete()},

                {ScenePhases.DrawCard,              () => Phase1Start_DrawCards()},
                {ScenePhases.PlayingHand,           () => Phase2Start_PlayingHand()},
                {ScenePhases.UnitMoving,            () => Phase3Start_UnitMoving()},
                {ScenePhases.UnitFighting,          () => Phase4Start_UnitFighting()},
                {ScenePhases.TurnCompleteEvaluation,() => Phase5Start_TurnComplete()},
            };

            //Delay before start game
            yield return new WaitForSeconds(TimeBeforeGameStart);
            GoToState(ScenePhases.DrawCard);
        }
        #endregion

        #region States
        private void GoToState(ScenePhases newState)
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
            //StartCoroutine(player.dodraw
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

/*
  
 */