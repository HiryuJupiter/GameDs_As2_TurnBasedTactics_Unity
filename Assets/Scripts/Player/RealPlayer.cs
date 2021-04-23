using System.Collections;
using UnityEngine;

public class RealPlayer : Player
{
    #region Field and mono
    PlayerCardPlacement cardPlacer;
    bool CanUpdate => GamePhaseManager.Phase == GamePhases.p3_PlayerControl;

    bool CanMouseSelectCard(Card card) =>
        GamePhaseManager.Phase == GamePhases.p3_PlayerControl &&
        ControlState != ControlStates.UnitMoving &&
        card != SelectionSlot.Card;

    protected override void Awake()
    {
        IsMainPlayer = true;
        cardPlacer = new PlayerCardPlacement(this);
        base.Awake();
    }

    private void Update()
    {
        if (CanUpdate)
        {
            switch (ControlState)
            {
                case ControlStates.Standby:
                    break;
                case ControlStates.CardSelected:
                    cardPlacer.TickUpdate();
                    
                    break;
                case ControlStates.UnitMoving:
                    break;
                default:
                    break;
            }
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 200, 20), "ControlState: " + ControlState);

        GUI.Label(new Rect(100f, 0f, 200f, 20f), "=== Deck pile === ");
        for (int i = 0; i < PlayerDeck.Cards.Count; i++)
        {
            GUI.Label(new Rect(100f, 20 + 20f * i, 200f, 20f),
                    i + ": " + PlayerDeck.Cards[i]);
        }

        GUI.Label(new Rect(250f, 0f, 200f, 20f), "=== Hand cards === ");
        for (int i = 0; i < Hand.Cards.Count; i++)
        {
            GUI.Label(new Rect(250f, 20 + 20f * i, 200f, 20f),
                    i + ": " + Hand.Cards[i]);
        }

        GUI.Label(new Rect(400f, 0f, 200f, 20f), "=== Selection slot === ");
        GUI.Label(new Rect(400f, 20, 200f, 20f),
                    "Selected: " + SelectionSlot.Card);

        GUI.Label(new Rect(550f, 0f, 200f, 20f), "=== Discard pile === ");
        for (int i = 0; i < DiscardPile.Cards.Count; i++)
        {
            GUI.Label(new Rect(400f, 20 + 20f * i, 200f, 20f),
                    i + ": " + DiscardPile.Cards[i]);
        }

        if (HighlightedCard != null)
        {
            GUI.Label(new Rect(700f, 0f, 200f, 20f), "highlighted: " + HighlightedCard);
        }
    }
    #endregion

    #region Public - Mouse highlight
    public override void MouseEnterCard(Card card)
    {
        if (!CanMouseSelectCard(card))
            return;

        //If highlighting a new card
        if (HighlightedCard != card)
        {
            HighlightedCard = card;
            Hand.SetHighlightCard(card);
        }
    }

    public override void MouseExitsCard(Card card)
    {
        if (!CanMouseSelectCard(card))
            return;

        //If exiting the already highlighted card
        if (HighlightedCard == card)
        {
            UnhighlightCards();
        }
    }

    public override void UnhighlightCards()
    {
        Hand.ExitHighlight();
        HighlightedCard = null;
    }
    #endregion

    #region Public - Card selection and placement
    public override void ClickedOnCard(Card card)
    {
        if (!CanMouseSelectCard(card))
            return;

        if (Hand.TryRemoveCardFromHand(card))
        {
            //Card
            ReturnSelectionSlotCardToHand();
            UnhighlightCards();
            SelectionSlot.SetAsSelectedCard(card);
            Hand.RefreshHandCardPositions();

            //Status change
            ControlState = ControlStates.CardSelected;
            Hand.LowerHand();
        }
        else
        {
            Debug.LogError("Shouldn't happen. Card " + card);
        }
    }

    public override void PlaceCard()
    {
        //Clear highlight
        UnhighlightCards();

        //Card
        DiscardPile.AddToDiscardPile(SelectionSlot.Card);

        //Status change
        ControlState = ControlStates.Standby;
        Hand.RaiseHand();
    }
    public void CancelCardSelection()
    {
        //Card
        ReturnSelectionSlotCardToHand();
        UnhighlightCards();
        Hand.RefreshHandCardPositions();

        //Status change
        ControlState = ControlStates.Standby;
        Hand.RaiseHand();
    }

    void ReturnSelectionSlotCardToHand ()
    {
        if (SelectionSlot.TryRemoveCard(out Card card))
        {
            Hand.AddCard(card);
        }
    }
    #endregion

    #region Public - move pieces
    public override void SelectPiece(DummyPiece piece)
    {
        ControlState = ControlStates.UnitMoving;
    }

    public override void UnselectUnit()
    {

    }
    #endregion
}