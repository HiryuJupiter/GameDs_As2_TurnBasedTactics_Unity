using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.DeckManagement;
using TurnBasedGame.HandManagement;
using TurnBasedGame.CardManagement;

namespace TurnBasedGame.PlayerManagement
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private bool isMainPlayer;
        [SerializeField] private Hand hand;
        [SerializeField] private Deck deck;

        public bool IsMainPlayer => isMainPlayer;
        public Hand PlayerHand => hand;
        public Deck PlayerDeck => deck;

        private void Awake()
        {

        }

        private void Start()
        {
            hand.Initialize(this);
            deck.Initialize(this);
        }
        private void OnGUI()
        {
            if (!IsMainPlayer)
                return;

            GUI.Label(new Rect(100f, 0f, 200f, 20f), "=== Deck pile === ");
            for (int i = 0; i < PlayerDeck.DeckPile.Count; i++)
            {
                GUI.Label(new Rect(100f, 20 + 20f * i, 200f, 20f),
                        i + ": " + PlayerDeck.DeckPile[i]);
            }

            GUI.Label(new Rect(250f, 0f, 200f, 20f), "=== Hand cards === ");
            for (int i = 0; i < hand.Cards.Count; i++)
            {
                GUI.Label(new Rect(250f, 20 + 20f * i, 200f, 20f),
                        i + ": " + hand.Cards[i]);
            }

            GUI.Label(new Rect(400f, 0f, 200f, 20f), "=== Discard pile === ");
            for (int i = 0; i < PlayerDeck.DiscardPile.Count; i++)
            {
                GUI.Label(new Rect(400f, 20 + 20f * i, 200f, 20f),
                        i + ": " + PlayerDeck.DiscardPile[i]);
            }
        }

        public void RemoveCard (Card card)
        {
            if (hand.TryRemoveCard(card))
            {
            }    
        }
    }
}
