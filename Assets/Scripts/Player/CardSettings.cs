using System.Collections;
using UnityEngine;

namespace TurnBasedGame
{
    public class CardSettings : MonoBehaviour
    {
        public static CardSettings Instance { get; private set; }

        [Header("Card counts")]
        [SerializeField] private int handSize = 5;
        [SerializeField] private int deckSize = 30;
        public int HandSize => handSize;
        public int DeckSize => deckSize;

        [Header("Hand spreading"), Tooltip("All of the following are unsigned values")]
        [SerializeField] private float cardSpacing = 1f;
        [SerializeField] private float startingRotation = 15f;
        [SerializeField] private float rotationOffset = 2f;
        [SerializeField] private float verticalOffset = 0.04f;
        public float spacing => cardSpacing;
        public float StartingRotation => startingRotation;
        public float RotationOffset => rotationOffset;
        public float VerticalOffset => verticalOffset;

        private void Awake()
        {
            Instance = this;    
        }

    }
}
