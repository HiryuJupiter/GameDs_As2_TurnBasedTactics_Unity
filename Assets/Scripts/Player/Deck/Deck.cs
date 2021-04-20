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
        #region Fields
        [SerializeField] private Transform spawnLocation;
        [SerializeField] private Transform stationaryLocation;
        [SerializeField] private Transform discardLocation;

        private Player player;
        private Hand hand;
        private CardDirectory cardDir;

        private List<Transform> cardPositions;
        private int deckSize;

        //Cache
        private float deckSpawnInterval;


        public List<Card> DeckPile { get; private set; }
        public List<Card> DiscardPile { get; private set; }
        public bool AreAllDeckCardsStill() => AreCardsStill(DeckPile);
        public bool AreAllHandCardsStill() => AreCardsStill(hand.Cards);
        #endregion

        #region Public - Initialize
        public void Initialize(Player player)
        {
            //Initialize
            DeckPile = new List<Card>();
            DiscardPile = new List<Card>();

            //Reference
            cardDir = CardDirectory.Instance;
            this.player = player;
            hand = player.PlayerHand;

            //Cache
            deckSize = CardSettings.Instance.DeckSize;
            deckSpawnInterval = CardSettings.Instance.DeckSpawnInterval;
        }
        #endregion

        #region Public - Fill deck, Draw card from deck, discard pile
        public IEnumerator FillDeck ()
        {
            InitializeDeckList();

            //for (int i = 0; i < deckSize; i++)
            //{
            //    Card c = cardDir.DrawRandomCard(
            //        stationaryLocation.position + new Vector3(0f, i * 0.01f, 0f),
            //        spawnLocation.rotation,
            //        transform);
            //    c.Initialize(player);
            //    //c.SetTargetPosition();
            //    c.SetTargetRotation(stationaryLocation.rotation);
            //    DeckPile.Add(c);
            //    yield return new WaitForSeconds(0.025f);
            //}

            for (int i = 0; i < deckSize; i++)
            {
                Card c = cardDir.DrawRandomCard(
                    spawnLocation.position,
                    spawnLocation.rotation,
                    transform);
                c.Initialize(player);
                c.SetTargetPosition(stationaryLocation.position + new Vector3(0f, i * 0.01f, 0f));
                c.SetTargetRotation(stationaryLocation.rotation, true);
                DeckPile.Add(c);
                yield return new WaitForSeconds(deckSpawnInterval);
            }
        }

        public bool TryDrawCardFromDeck(out Card card)
        {
            card = null;
            if (DeckPile.Count > 0f)
            {
                card = DeckPile[DeckPile.Count - 1];
                DeckPile.RemoveAt(DeckPile.Count - 1);
                card.transform.parent = hand.transform;
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
            card.SetTargetPosition(discardLocation.position + new Vector3(0f, DiscardPile.Count * 0.01f));
            card.SetTargetRotation(discardLocation.rotation, false);
            card.transform.parent = transform;
        }
        #endregion

        #region MinorMethods
        private bool AreCardsStill(List<Card> cards)
        {
            foreach (var card in cards)
            {
                if (card.InMovingAnimation)
                {
                    //Debug.Log("still moving: " + card.name);
                    return false;
                }
            }
            return true;
        }

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