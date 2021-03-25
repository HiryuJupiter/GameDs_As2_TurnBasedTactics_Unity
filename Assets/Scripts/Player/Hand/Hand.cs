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

        private List<Card> hand;
        private Player player;
        private Deck deck;

        public List<Card> Cards => hand;

        #region Public - Initialize
        public void Initialize(Player player)
        {
            //Initialize
            hand = new List<Card>();
            for (int i = 0; i < HandSize; i++)
            {
                hand.Add(null);
            }

            //Reference
            this.player = player;
            deck = player.Deck;
        }
        #endregion

        #region Public - Draw card
        public IEnumerator DoDrawCards()
        {
            //Draw cards with a small delay between drawing each card
            for (int i = 0; i < HandSize; i++)
            {
                //If there is an empty slot, then 
                if (hand[i] == null)
                {
                    Card card = deck.DrawCard(CardTypes.Dummy);
                    hand[i] = card;

                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(DoDrawCards());
                }
            }
        }
        #endregion

        #region Public - Remove card
        public bool TryRemoveCard(Card card)
        {
            if (hand.Contains(card))
            {
                //Destroy(cards[cards.Count - 1].gameObject);
                //cards.RemoveAt(cards.Count - 1);
                hand[hand.IndexOf(card)] = null;
                PushUpCardsIndex();
                return true;
            }
            return false;
        }

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
        #endregion

        #region Minor classes
        private void OnGUI()
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i] == null)
                {
                    GUI.Label(new Rect(20f + (player.IsMainPlayer ? 0f : 300f), 200f + 20f * i, 200f, 20f), 
                        i + ": " + hand[i]);
                }
            }
        }

        private void PrintAllCards()
        {
            string s = "";
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i] != null)
                    s += hand[i].ToString() + ", ";
                else
                    s += "null, ";
            }
            Debug.Log(s);
        }
        #endregion
    }
}