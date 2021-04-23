using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    [Header("Card spawning")]
    [SerializeField] int handSize = 5;
    [SerializeField] int deckSize = 30;
    [SerializeField] float deckSpawnInterval = 0.025f;
    [SerializeField] float cardDrawInterval = 0.2f;
    public int HandSize => handSize;
    public int DeckSize => deckSize;
    public float DeckSpawnInterval => deckSpawnInterval;
    public float CardDrawInterval => cardDrawInterval;

    [Header("Move parameters")]
    [SerializeField] float moveLerpSpeed = 1f;
    [SerializeField] float rotLerpSpeed = 1f;
    [SerializeField] float scaleLerpSpeed = 2.5f;
    [SerializeField] float parabolicHeight = .3f; //The magnitude of the parabolic effect when moving cards
    public float MoveLerpSpeed => moveLerpSpeed;
    public float RotLerpSpeed => rotLerpSpeed;
    public float ScaleLerpSpeed => scaleLerpSpeed;
    public float ParabolicHeight => parabolicHeight;

    [Header("Hand spreading"), Tooltip("All of the following are unsigned values")]
    [SerializeField] float cardSpacing = 1f;
    [SerializeField] float zRotationStart = 15f;
    [SerializeField] float zRotationOffset = 2f;
    [SerializeField] float verticalOffset = 0.1f;
    public float spacing => cardSpacing;
    public float ZRotationStart => zRotationStart;
    public float ZRotationOffset => zRotationOffset;
    public float VerticalOffset => verticalOffset;

    [Header("Panning and hand raising")]
    [SerializeField] float mousePanSensitivity = -0.01f;
    public float MousePanSensitivity => mousePanSensitivity;


    [Header("Card highlight")]
    [SerializeField] float highlightOffsetX = 0.1f;
    [SerializeField] float highlightOffsetY = 1.2f;
    [SerializeField] float highlightScale = 0.1f;
    public float HighlightOffsetX => highlightOffsetX;
    public float HighlightOffsetY => highlightOffsetY;
    public Vector3 HighlightScale => new Vector3(highlightScale, highlightScale, highlightScale);

    void Awake()
    {
        Instance = this;
    }
}