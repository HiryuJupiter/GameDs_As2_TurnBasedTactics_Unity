using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.CardManagement;

namespace TurnBasedGame.Hand
{

    public class Hand : MonoBehaviour
    {
        const int HandSize = 5;

        private CardDirectory cardDir;
        private Card[] cards;


        private void Awake()
        {
            cards = new Card[HandSize];
        }


        void Start()
        {
            cardDir = CardDirectory.Instance;
        }

        void Update()
        {

        }

        /// <summary>
        /// Organize the cards array so that all empty indexes are near the end of the array, after the indexes of cards.
        /// </summary>
        private void ShuffleUpCardsIndex (Card[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i] == null)
                {
                    for (int j = i + 1; j < cards.Length; j++)
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

        //rivate Card GetCard(int id) => cardDir.GetDummyCard(id);

        #region Debug tests
        private class TestCard : Card
        {
            public TestCard(int id) : base(id) {}
        }

        private void CardOrganizationTest ()
        {
            Card[] testcards = new Card[8];
            testcards[0] = new TestCard(0);
            testcards[1] = null;
            testcards[2] = new TestCard(10);
            testcards[3] = null;
            testcards[4] = null;
            testcards[5] = new TestCard(20);
            testcards[6] = null;

            Debug.Log(testcards[0]);
            Debug.Log(testcards[0] == null);
            Debug.Log(testcards[1]);

            PrintAllCards(testcards);
            ShuffleUpCardsIndex(testcards);
            PrintAllCards(testcards);
        }


        private void PrintAllCards(Card[] cards)
        {
            string s = "";
            for (int i = 0; i < cards.Length; i++)
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
