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
        #region Fields
        [Header("Positional references")]
        [SerializeField] private Transform centuralCardPosition;
        [SerializeField] private Transform leftLimit;
        [SerializeField] private Transform facingTarget; //The target that the cards are facing

        private Player player;
        private HandSpreader spreader;

        //Status
        private Vector3 mousePanOffset;
        private float xPos;
        private float yPos;

        //Cache
        private CardSettings setting;
        private int handSize;
        private Vector3 startingPos;
        private float cardDrawInterval;

        public List<Card> Cards { get; private set; }
        private Deck deck => player.PlayerDeck;
        #endregion

        #region Public - Initialization
        public void Initialize(Player player)
        {
            //Initialize
            Cards = new List<Card>();

            //Reference
            this.player = player;
            setting = CardSettings.Instance;
            spreader = new HandSpreader(player, centuralCardPosition, leftLimit, facingTarget.position);

            //Cache
            handSize = CardSettings.Instance.HandSize;
            startingPos = transform.position;
            cardDrawInterval = CardSettings.Instance.CardDrawInterval;
        }
        #endregion

        #region Mono
        private void Update()
        {
            if (player.IsMainPlayer)
            {
                PanningUpdate();
                HandRaiseUpdate();
            }

            transform.position = Vector3.Lerp(transform.position, startingPos + new Vector3(xPos, yPos, 0f), 10f * Time.deltaTime);
        }
        #endregion

        #region Card position modification
        private void PanningUpdate()
        {
            var rawMouse = Input.mousePosition;
            rawMouse.z = 10f;
            mousePanOffset = Camera.main.ScreenToWorldPoint(rawMouse);
            xPos = mousePanOffset.x * setting.MousePanSensitivity;
        }

        bool handRaised;
        private void HandRaiseUpdate()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    handRaised = !handRaised;
            //    yPos = handRaised ? 0f : -3f;
            //}
        }
        #endregion

        #region Public - Drawing and removing cards
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
                yield return new WaitForSeconds(cardDrawInterval);
            }
        }

        public bool TryRemoveCardFromHand(Card card) //A verbose name but makes it distinct from Deck's DrawCard method
        {
            if (Cards.Contains(card))
            {
                Cards.Remove(card);
                deck.AddToDiscardPile(card);
                spreader.UpdateAllCardPositions();
                return true;
            }
            return false;
        }
        #endregion

        #region Public - card highlight
        public void SetHighlightCard(Card card)
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
        public void ExitHighlight()
        {
            foreach (var c in Cards)
            {
                c.ExitHighlight();
            }
        }
        #endregion
    }
}