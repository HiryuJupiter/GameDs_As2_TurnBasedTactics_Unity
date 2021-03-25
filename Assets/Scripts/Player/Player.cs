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
        public Hand Hand => hand;
        public Deck Deck => deck;

        private void Awake()
        {

        }
        private void Start()
        {
            hand.Initialize(this);
            deck.Initialize(this);
        }

        public IEnumerator WaitForCardsToBeDrawn ()
        {
            yield return hand.DrawCard();
        }
    }
}
