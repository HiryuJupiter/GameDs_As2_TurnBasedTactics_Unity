using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.CardManagement;
using TurnBasedGame.DeckManagement;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.HandManagement
{
    public class Hand : MonoBehaviour
    {
        private const int HandSize = 5;

        [SerializeField] private Transform centralCardPos;

        private List<Card> cards;
        private Player player;
        private Deck deck;

        #region Public - Initialize
        public void Initialize(Player player)
        {
            //Initialize
            cards = new List<Card>();
            for (int i = 0; i < HandSize; i++)
            {
                cards.Add(null);
            }
            PrintAllCards();

            //Reference
            this.player = player;
            deck = player.Deck;
        }
        #endregion

        #region Public - Add Card
        public void DrawHand ()
        {
            //if (player.IsMainPlayer)
            //    TestDraw();
        }

        private void TestDraw()
        {
            for (int i = 0; i < 1; i++)
            {
                TryAddCard(CardTypes.Dummy);
            }
        }

        public bool TryAddCard(CardTypes cardType)
        {
            for (int i = 0; i < HandSize; i++)
            {
                if (cards[i] == null)
                {
                    Card card = deck.DrawCard(CardTypes.Dummy);
                    cards[i] = card;
                    UpdateCardDisplay();
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Public - Remove card
        public bool TryRemoveCard(Card card)
        {
            if (cards.Contains(card))
            {
                cards[cards.IndexOf(card)] = null;
                UpdateCardDisplay();
                return true;
            }
            return false;
        }
        #endregion

        #region Display cards in hand
        private void UpdateCardDisplay()
        {
            //Debug.Log("---Upadate Card Display---");
            //PrintAllCards();
            //PushUpCardsIndex();
            //PrintAllCards();
            //Debug.Log("---Begin arranging cards---");

        }

        private void CalculateCardPositions()
        {
            List<Card> validCards = new List<Card>();
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i]!= null)
                {

                }
            }
        }
        #endregion

        #region Minor classes
        /// <summary>
        /// Organize the cards array so that all empty indexes are near the end of the array, after the indexes of cards.
        /// </summary>
        private void PushUpCardsIndex()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] == null)
                {
                    for (int j = i + 1; j < cards.Count; j++)
                    {
                        if (cards[j] != null)
                        {
                            cards[i] = cards[j];
                            cards[j] = null;
                            break;
                        }
                    }
                }
            }
        }

        private bool HasEmptySlotInHand()
        {
            for (int i = 0; i < HandSize; i++)
            {
                if (cards[i] == null)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Debug tests
        private void PrintAllCards()
        {
            string s = "";
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null)
                    s += cards[i].ToString() + ", ";
                else
                    s += "null, ";
            }
            Debug.Log(s);
        }
        #endregion
    }
}
