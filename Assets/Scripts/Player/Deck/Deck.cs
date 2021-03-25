using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.CardManagement;
using TurnBasedGame.PlayerManagement;
using TurnBasedGame.HandManagement;

namespace TurnBasedGame.DeckManagement
{
    public class Deck : MonoBehaviour
    {
        private const int DeckSize = 30;

        [SerializeField] private Transform drawCardLocation;
        [SerializeField] private DiscardPile discardPile;

        private List<Card> cards;
        private Player player;
        private Hand hand;

        private List<Transform> cardPositions;

        private CardDirectory cardDir;

        #region Public
        public void Initialize(Player player)
        {
            //Initialize
            cards = new List<Card>();

            //Reference
            cardDir = CardDirectory.Instance;
            this.player = player;
            hand = player.Hand;
        }

        public Card DrawCard (CardTypes cardType)
        {
            Card c = cardDir.DrawCard(cardType, 
                drawCardLocation.position + new Vector3(0f, Random.Range(-0.1f, 0.1f), 0f), 
                drawCardLocation.rotation);
            c.Initialize(player);
            return c;
        }
        #endregion
    }
}