using System.Collections;
using UnityEngine;

namespace TurnBasedGame
{
    public class CardSettings : MonoBehaviour
    {
        public static CardSettings Instance { get; private set; }

        [Header("Card spawning")]
        [SerializeField] private int handSize = 5;
        [SerializeField] private int deckSize = 30;
        [SerializeField] private float deckSpawnInterval = 0.025f;
        [SerializeField] private float cardDrawInterval = 0.8f;
        public int HandSize => handSize;
        public int DeckSize => deckSize;
        public float DeckSpawnInterval => deckSpawnInterval;
        public float CardDrawInterval => cardDrawInterval;

        [Header("Move parameters")]
        [SerializeField] private float moveLerpSpeed = 0.2f;
        [SerializeField] private float rotLerpSpeed = 0.5f;
        [SerializeField] private float scaleLerpSpeed = 2.5f;
        [SerializeField] private float parabolicHeight = 2f; //The magnitude of the parabolic effect when moving cards
        public float MoveLerpSpeed => moveLerpSpeed;
        public float RotLerpSpeed => rotLerpSpeed;
        public float ScaleLerpSpeed => scaleLerpSpeed;
        public float ParabolicHeight => parabolicHeight;

        [Header("Hand spreading"), Tooltip("All of the following are unsigned values")]
        [SerializeField] private float cardSpacing = 1f;
        [SerializeField] private float zRotationStart = 15f;
        [SerializeField] private float zRotationOffset = 2f;
        [SerializeField] private float verticalOffset = 0.1f;
        [SerializeField] private float mousePanSensitivity = -0.1f;
        public float spacing => cardSpacing;
        public float ZRotationStart => zRotationStart;
        public float ZRotationOffset => zRotationOffset;
        public float VerticalOffset => verticalOffset;
        public float MousePanSensitivity => mousePanSensitivity;

        [Header("Card highlight")]
        [SerializeField] private float highlightOffsetX = 0.1f;
        [SerializeField] private float highlightOffsetY = 1.2f;
        [SerializeField] private float highlightScale = 0.1f;
        public float HighlightOffsetX => highlightOffsetX;
        public float HighlightOffsetY => highlightOffsetY;
        public Vector3 HighlightScale => new Vector3(highlightScale, highlightScale, highlightScale);

        private void Awake()
        {
            Instance = this;    
        }
    }
}
