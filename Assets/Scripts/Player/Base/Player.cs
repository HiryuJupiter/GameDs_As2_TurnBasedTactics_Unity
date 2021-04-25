using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public static Player Instance;

    #region Fields
    [SerializeField] PlayerHand hand;
    [SerializeField] Deck deck;
    [SerializeField] DiscardPile discardPile;
    [SerializeField] SelectionSlot selectionSlot;

    protected UIManager uiM;
    protected GamePhaseManager phaseManager;
    protected GameSettings settings;
    protected GameState gameState;

    public bool IsMainPlayer { get; protected set; } = false;
    public PlayerHand Hand => hand;
    public Deck PlayerDeck => deck;
    public DiscardPile DiscardPile => discardPile;
    public SelectionSlot SelectionSlot => selectionSlot;


    #endregion

    #region Mono
    protected virtual void Awake()
    {
        hand.Initialize(this);
        deck.Initialize(this);
        selectionSlot.Initialize(this);
        discardPile.Initialize(this);

        settings = GameSettings.Instance;
        Instance = this;
    }

    private void Start()
    {
        phaseManager = GamePhaseManager.Instance;
        uiM = UIManager.Instance;
        gameState = GameState.Instance;

        
    }
    #endregion

    #region Public - Card draw
    public void FillDeck()
    {
        PlayerDeck.Fill();
    }

    public void DrawHand()
    {
        Hand.RaiseHand();
        Hand.DrawHand();

        CheckIfCanDisplayBuyCardButton();
    }
    #endregion

    #region Public - Phase transitions
    public void BuyCard() //Also invoked by UI button click
    {
        if (gameState.TryBuyCard())
        {
            Hand.CustomDrawCard();
            Hand.RefreshHandCardPositions();
            CheckIfCanDisplayBuyCardButton();
        }
    }

    public void CancelCardPlacement() //Also invoked by UI button click
    {
        //Return selection slot card to hand
        if (SelectionSlot.TryRemoveCard(out Card card))
        {
            Hand.AddCard(card);
        }

        EnterCardSelection();
    }

    public void EnterCardSelection()
    {
        //Display hand
        Hand.RaiseHand();
        Hand.RefreshHandCardPositions();

        //UI
        CheckIfCanDisplayBuyCardButton();

        phaseManager.ToP3_CardSelection();
    }

    public void GoToCardPlacement()
    {
        phaseManager.ToP4_CardPlacementPhase();
    }
    #endregion

    public void CheckIfCanDisplayBuyCardButton ()
    {
        if (gameState.HasMoneyForCard && 
            hand.Cards.Count < 15f && 
            deck.Cards.Count > 0)
            uiM.ToggleBuyCardButton(true);
        else
            uiM.ToggleBuyCardButton(false);
    }
}