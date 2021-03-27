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

        [Header("Move speed")]
        [SerializeField] private float moveLerpSpeed = 0.2f;
        [SerializeField] private float rotLerpSpeed = 0.5f;
        [SerializeField] private float scaleLerpSpeed = 2.5f;
        public float MoveLerpSpeed => moveLerpSpeed;
        public float RotLerpSpeed => rotLerpSpeed;
        public float ScaleLerpSpeed => scaleLerpSpeed;

        [Header("Hand spreading"), Tooltip("All of the following are unsigned values")]
        [SerializeField] private float cardSpacing = 1f;
        [SerializeField] private float zRotationStart = 15f;
        [SerializeField] private float zRotationOffset = 2f;
        //[SerializeField] private float yRotationStart = -10f;
        //[SerializeField] private float yRotationOffset = 2f;
        [SerializeField] private float verticalOffset = 0.05f;
        [SerializeField] private float mousePanSensitivity = -0.1f;
        public float spacing => cardSpacing;
        public float ZRotationStart => zRotationStart;
        public float ZRotationOffset => zRotationOffset;
        //public float YRotationStart => yRotationStart;
        //public float YRotationOffset => yRotationOffset;
        public float VerticalOffset => verticalOffset;
        public float MousePanSensitivity => mousePanSensitivity;

        [Header("Card highlight")]
        [SerializeField] private float highlightOffsetX = 0.2f;
        [SerializeField] private float highlightOffsetY = 1.5f;
        [SerializeField] private float highlightScale = 0.2f;
        public float HighlightOffsetX => highlightOffsetX;
        public float HighlightOffsetY => highlightOffsetY;
        public Vector3 HighlightScale => new Vector3(highlightScale, highlightScale, highlightScale);


        private void Awake()
        {
            Instance = this;    
        }

    }
}
