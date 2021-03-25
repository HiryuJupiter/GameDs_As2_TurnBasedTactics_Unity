using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.CardManagement;

namespace TurnBasedGame.DeckManagement
{
    public class DiscardPile : MonoBehaviour
    {
        [SerializeField] private Transform discardLocation;
        private List<Card> cards;

        public Transform DiscardLocation => discardLocation;

        public void AddToPile (Card card)
        {
            cards.Add(card);
        }
    }
}
