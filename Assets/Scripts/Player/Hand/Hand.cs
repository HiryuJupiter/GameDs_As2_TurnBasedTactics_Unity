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
        [Header("Positional references")]
        [SerializeField] private Transform centuralCardPosition;
        [SerializeField] private Transform leftLimit;
        [SerializeField] private Transform facingTarget; //The target that the cards are facing

        private Player player;
        private Deck deck;
        private HandSpreader_Ver6 spreader;

        //Status
        private Vector3 mouseOffset;

        //Cache
        private CardSettings setting;
        private int handSize;
        private Vector3 startingPos;

        public List<Card> Cards { get; private set; }

        #region Mono
        private void Update()
        {
            if (player.IsMainPlayer)
            {
                PanningUpdate();
            }
        }

        //private void OnGUI()
        //{
        //    GUI.Label(new Rect(200, 200, 200, 20), "mouseXOffset: " + mouseXOffset);
        //    GUI.Label(new Rect(200, 220, 200, 20), "Input.mousePosition: " + Input.mousePosition);
        //    GUI.Label(new Rect(200, 240, 200, 20), "world.mousePosition: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //}
        #endregion

        #region Public
        public void Initialize(Player player)
        {
            //Initialize
            Cards = new List<Card>();

            //Reference
            this.player = player;
            setting = CardSettings.Instance;
            deck = player.PlayerDeck;
            spreader = new HandSpreader_Ver6(player, centuralCardPosition, leftLimit, facingTarget.position);

            //Cache
            handSize = CardSettings.Instance.HandSize;
            startingPos = transform.position;
        }

        public IEnumerator WaitForHandToBeDrawn()
        {
            while (Cards.Count < handSize)
            {
                if (deck.TryDrawCardFromDeck(out Card card))
                {
                    Cards.Add(card);
                }
                else
                    yield break; //No more cards in deck
            }

            for (int i = 0; i < Cards.Count; i++)
            {
                spreader.UpdateSingleCardPosition(i);
                yield return new WaitForSeconds(0.02f);
            }
        }

        public bool TryRemoveCardFromHand(Card card) //A verbose name but makes it distinct from Deck's DrawCard method
        {
            if (Cards.Contains(card))
            {
                Cards.Remove(card);
                deck.AddToDiscardPile(card);
                spreader.UpdateAllCardsPositions();
                return true;
            }
            return false;
        }

        public void SetHighlightCard (Card card)
        {
            if (Cards.Contains(card))
            {
                int index = Cards.IndexOf(card);
                for (int i = 0; i < Cards.Count; i++)
                {
                    if (i < index)
                    {
                        Cards[i].HighlightOffsetMove(true);

                    }
                    else if (i == index)
                    {
                        Cards[i].EnterHighlight();
                    }
                    else
                    {
                        Cards[i].HighlightOffsetMove(false);
                    }
                }
            }
        }
        public void ExitHighlight ()
        {
            foreach (var c in Cards)
            {
                c.ExitHighlight();
            }
        }
        #endregion

        #region Panning
        private void PanningUpdate()
        {
            var rawMouse = Input.mousePosition;
            rawMouse.z = 10f;
            mouseOffset = Camera.main.ScreenToWorldPoint(rawMouse);
            transform.position = startingPos +
                new Vector3(mouseOffset.x * setting.MousePanSensitivity, 0f, 0f);
            //transform.position = startingPos +
            //    new Vector3(mouseOffset.x * setting.MousePanSensitivity,
            //    mouseOffset.y * setting.MousePanSensitivity, 0f);
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