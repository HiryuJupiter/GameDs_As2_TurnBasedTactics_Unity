using System.Collections;
using UnityEngine;

namespace TurnBasedGame
{
    public class ScenePhaseEventListener : MonoBehaviour
    {
        //MonoBehaviour methods
        protected virtual void Awake ()
        {
            RegisterEvent();
        }

        protected virtual void OnDestroy ()
        {
            DeregisterEvents();
        }

        //Events registration / deregistration
        protected void RegisterEvent ()
        {
            ScenePhaseManager.OnPhase1Start_DrawCards         += PhaseStart_DrawCards;
            ScenePhaseManager.OnPhase2Start_PlayingHand     += PhaseStart_PlayingHand;
            ScenePhaseManager.OnPhase3Start_UnitMoving      += PhaseStart_UnitMoving;
            ScenePhaseManager.OnPhase4Start_UnitFighting    += PhaseStart_UnitFighting;
            ScenePhaseManager.OnPhase5Start_TurnComplete    += PhaseStart_TurnComplete;
        }

        protected void DeregisterEvents () 
        {
            ScenePhaseManager.OnPhase1Start_DrawCards         -= PhaseStart_DrawCards;
            ScenePhaseManager.OnPhase2Start_PlayingHand     -= PhaseStart_PlayingHand;
            ScenePhaseManager.OnPhase3Start_UnitMoving      -= PhaseStart_UnitMoving;
            ScenePhaseManager.OnPhase4Start_UnitFighting    -= PhaseStart_UnitFighting;
            ScenePhaseManager.OnPhase5Start_TurnComplete    -= PhaseStart_TurnComplete;
        }

        protected virtual void PhaseStart_DrawCards()
        {
        }

        protected virtual void PhaseStart_PlayingHand()
        {
        }

        protected virtual void PhaseStart_UnitMoving()
        {
        }

        protected virtual void PhaseStart_UnitFighting()
        {
        }

        protected virtual void PhaseStart_TurnComplete()
        {
        }
    }
}
