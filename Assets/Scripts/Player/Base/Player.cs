using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    #region Fields
    [SerializeField] PlayerHand hand;
    [SerializeField] Deck deck;
    [SerializeField] DiscardPile discardPile;
    [SerializeField] SelectionSlot selectionSlot;

    protected UIManager uiM;
    protected GamePhaseManager phaseManager;
    protected GameSettings settings;

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
    }

    private void Start()
    {
        phaseManager = GamePhaseManager.Instance;
        uiM = UIManager.Instance;
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
    }
    #endregion

    #region Public - Phase transitions
    public void EnterUnitControl() //Also invoked by UI button click
    {
        Hand.HideHand();
        Hand.RefreshHandCardPositions();
        phaseManager.ToP5_UnitControlMode();
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

        phaseManager.ToP3_CardSelection();
    }

    public void FinishedCardSelection()
    {
        phaseManager.ToP4_CardPlacementPhase();
    }
    #endregion
}