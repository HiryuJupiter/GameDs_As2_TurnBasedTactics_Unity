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

        [SerializeField] private Transform spawnLocation;
        [SerializeField] private Transform stationaryLocation;
        [SerializeField] private Transform discardLocation;

        private Player player;
        private Hand hand;

        private List<Transform> cardPositions;

        private CardDirectory cardDir;

        public List<Card> DeckPile { get; private set; }
        public List<Card> DiscardPile { get; private set; }

        #region Public
        public void Initialize(Player player)
        {
            //Initialize
            DeckPile = new List<Card>();
            DiscardPile = new List<Card>();

            //Reference
            cardDir = CardDirectory.Instance;
            this.player = player;
            hand = player.PlayerHand;
        }

        public IEnumerator FillDeck ()
        {
            InitializeDeckList();

            for (int i = 0; i < DeckSize; i++)
            {
                Card c = cardDir.DrawRandomCard(
                    spawnLocation.position,
                    spawnLocation.rotation);
                c.SetTargetPositional(stationaryLocation.position + new Vector3(0f, i * 0.01f, 0f));
                c.SetTargetRotation(stationaryLocation.rotation);
                c.Initialize(player);
                DeckPile.Add(c);
                yield return new WaitForSeconds(0.01f);
            }
        }

        public bool TryDrawCard(out Card card)
        {
            card = null;
            if (DeckPile.Count > 0f)
            {
                card = DeckPile[DeckPile.Count - 1];
                DeckPile.RemoveAt(DeckPile.Count - 1);
                return true;
            }
            return false;
           
            //Card c = cardDir.DrawCard(cardType,
            //    spawnLocation.position + new Vector3(0f, Random.Range(-0.1f, 0.1f), 0f),
            //    spawnLocation.rotation);
            //c.Initialize(player);
            //return c;
        }

        public void AddToDiscardPile (Card card)
        {
            DiscardPile.Add(card);
            card.SetTargetPositional(discardLocation.position + new Vector3(0f, DiscardPile.Count * 0.01f));
            card.SetTargetRotation(discardLocation.rotation);
        }
        #endregion

        #region MinorMethods
        void InitializeDeckList ()
        {
            if (DeckPile == null || DeckPile.Count > 0)
            {
                Debug.LogError("Deck already exist");
                DeckPile = new List<Card>();
            }
        }
        #endregion
    }
}