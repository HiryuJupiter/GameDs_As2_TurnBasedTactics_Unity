using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.CardManagement;

namespace TurnBasedGame.DeckManagement
{
    public class DiscardPile : MonoBehaviour
    {
        [SerializeField] private Transform discardLocation;
        public Transform DiscardLocation => discardLocation;
    }
}
