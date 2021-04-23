using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum ControlStates { Standby, CardSelected, UnitMoving }

    #region Fields
    [SerializeField] PlayerHand hand;
    [SerializeField] Deck deck;
    [SerializeField] DiscardPile discardPile;
    [SerializeField] SelectionSlot selectionSlot;

    protected Card HighlightedCard;
    protected DummyPiece selectedUnit;

    public bool IsMainPlayer { get; protected set; } = false;
    public PlayerHand Hand => hand;
    public Deck PlayerDeck => deck;
    public DiscardPile DiscardPile => discardPile;
    public SelectionSlot SelectionSlot => selectionSlot;
    public ControlStates ControlState { get; protected set; } = ControlStates.Standby;
    #endregion

    #region Mono
    protected virtual void Awake()
    {
        hand.Initialize(this);
        deck.Initialize(this);
        selectionSlot.Initialize(this);
        discardPile.Initialize(this);
    }
    #endregion

    public virtual void RunAISequence() { }

    #region Public - Card draw
    public void FillDeck()
    {
        PlayerDeck.Fill();
    }

    public void DrawHand()
    {
        ControlState = ControlStates.Standby;
        Hand.RaiseHand();
        Hand.DrawHand();
    }
    #endregion

    // Mouse highlight
    public virtual void MouseEnterCard(Card card) {}
    public virtual void MouseExitsCard(Card card) {}
    public virtual void UnhighlightCards() {}

    // Card selection and placement
    public virtual void ClickedOnCard(Card card) {}
    public virtual void PlaceCard() {}

    // Move pieces
    public virtual void SelectPiece (DummyPiece piece) {}
    public virtual void UnselectUnit () {}
}