using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.CardManagement;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.DeckManagement
{
    public class Deck : MonoBehaviour
    {
        private const int DeckSize = 30;

        [SerializeField] private Transform placementLocation;
        [SerializeField] private DiscardPile discardPile;

        private CardDirectory cardDir;
        private Card[] cards;
        private Player player;


        #region Public
        public void Initialize(Player player)
        {
            //Initialize
            cards = new Card[DeckSize];
            //Reference
            cardDir = CardDirectory.Instance;
            this.player = player;
    }
        #endregion
    }
}
