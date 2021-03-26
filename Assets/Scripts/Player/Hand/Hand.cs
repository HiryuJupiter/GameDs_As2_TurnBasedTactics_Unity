using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.CardManagement;
using TurnBasedGame.DeckManagement;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.HandManagement
{
    [RequireComponent(typeof(HandCardScroller))]
    public class Hand : MonoBehaviour
    {
        private const int HandSize = 5;

        private Player player;
        private Deck deck;
        private HandCardScroller scroller;

        public List<Card> Cards { get; private set; }

        #region Public - Initialize
        public void Initialize(Player player)
        {
            //Initialize
            Cards = new List<Card>();

            //Reference
            this.player = player;
            deck = player.PlayerDeck;
            scroller = GetComponent<HandCardScroller>();
            scroller.Initilize(player);
        }
        #endregion

        #region Public
        public IEnumerator WaitForHandToBeDrawn()
        {
            //Find all empty slots in the hand and put a card in each slot

            while (Cards.Count < HandSize)
            {
                if (deck.TryDrawCard(out Card card))
                {
                    Cards.Add(card);
                    scroller.UpdateCardPositions();

                    //Have a small delay between drawing each card.
                    yield return new WaitForSeconds(0.1f);
                }
                else
                    yield break;
            }
        }

        public bool TryRemoveCard(Card card)
        {
            if (Cards.Contains(card))
            {
                Cards.Remove(card);
                deck.AddToDiscardPile(card);
                scroller.UpdateCardPositions();
                return true;
            }
            return false;
        }
        #endregion

        #region Minor classes


        private void PrintAllCards()
        {
            string s = "";
            for (int i = 0; i < Cards.Count; i++)
            {
                s += Cards[i].ToString() + ", ";
            }
            Debug.Log(s);
        }
        #endregion
    }
}

/*
         // Organize the cards array so that all empty indexes are near the end of the array, after the indexes of cards.
        private void PushUpCardsIndex()
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i] == null)
                {
                    for (int j = i + 1; j < hand.Count; j++)
                    {
                        if (hand[j] != null)
                        {
                            hand[i] = hand[j];
                            hand[j] = null;
                            break;
                        }
                    }
                }
            }
        }
 */