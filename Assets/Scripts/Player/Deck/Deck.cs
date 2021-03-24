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

        [SerializeField] private Transform drawCardLocation;
        [SerializeField] private DiscardPile discardPile;

        private List<Card> cards;
        private Player player;

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
        }

        public Card DrawCard (CardTypes cardType)
        {
            return cardDir.DrawCard(cardType, 
                drawCardLocation.position + new Vector3(0f, Random.Range(-0.1f, 0.1f), 0f), 
                drawCardLocation.rotation);
        }
        #endregion
    }
}