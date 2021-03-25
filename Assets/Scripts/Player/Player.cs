using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame;
using TurnBasedGame.DeckManagement;
using TurnBasedGame.HandManagement;
using TurnBasedGame.CardManagement;

namespace TurnBasedGame.PlayerManagement
{
    public class Player : ScenePhaseEventListener
    {
        [SerializeField] private bool isMainPlayer;
        [SerializeField] private Hand hand;
        [SerializeField] private Deck deck;

        public bool IsMainPlayer => isMainPlayer;
        public Hand Hand => hand;
        public Deck Deck => deck;

        #region Mono
        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            hand.Initialize(this);
            deck.Initialize(this);
        }
        #endregion

        #region Public
        protected override void PhaseStart_DrawCards() 
        { 
            if (isMainPlayer)
            {
                hand.DrawCards();
            }
        }

        protected override void PhaseStart_PlayingHand()
        {
        }

        protected override void PhaseStart_UnitMoving()
        {
        }

        protected override void PhaseStart_UnitFighting()
        {
        }

        protected override void PhaseStart_TurnComplete()
        {
        }
        #endregion
    }
}
