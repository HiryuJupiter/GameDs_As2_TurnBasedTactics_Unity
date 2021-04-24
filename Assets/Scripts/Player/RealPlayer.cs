using System.Collections;
using UnityEngine;

public class RealPlayer : Player
{
    #region Field and mono
    
    
    PlayerCardSelectionControl selectionControl;
    PlayerCardPlacementControl placementControl;
    PlayerUnitMovingControl unitMoveControl;


    public UnitPiece HitUnitPiece { get; private set; }
    public UnitPiece PrevHitUnitPiece { get; private set; }
    public Card HitHandCard { get; private set; }
    public Card PrevHitHandCard { get; private set; }
    public DummyBoardTile HitTile { get; private set; }
    public DummyBoardTile PrevHitTile { get; private set; }

    public MouseOverObjects MouseOverObject { get; private set; } =
    MouseOverObjects.None;
    public MouseOverObjects PrevMouseOverObject { get; private set; } =
    MouseOverObjects.None;

    protected override void Awake()
    {
        IsMainPlayer = true;
        selectionControl = new PlayerCardSelectionControl(this);
        placementControl = new PlayerCardPlacementControl(this);
        unitMoveControl = new PlayerUnitMovingControl(this);

        base.Awake();
    }

    private void Update()
    {
        CheckForRaycastHitObject();
        switch (GamePhaseManager.Phase)
        {
            case GamePhases.phase3_CardSelection:
                selectionControl.TickUpdate();
                break;
            case GamePhases.phase4_Placement:
                placementControl.TickUpdate();
                break;
            case GamePhases.phase5_UnitControl:
                unitMoveControl.TickUpdate();
                break;
        }
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(20, 20, 200, 20), "ControlState: " + ControlState);

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
    }
    #endregion

    #region Public 
    public void FinishedSelectingCard()
    {
        phaseManager.ToP4_CardPlacementPhase();
    }

    public void CancelPlacement ()
    {
        //Return selection slot card to hand, if there is one.
        if (SelectionSlot.TryRemoveCard(out Card card))
        {
            Hand.AddCard(card);
        }

        //Display hand
        Hand.RaiseHand();
        Hand.RefreshHandCardPositions();

        phaseManager.ToP3_CardSelection();
    }
    #endregion

    #region Raycast hit
    Ray ray => Camera.main.ScreenPointToRay(Input.mousePosition);

    void CheckForRaycastHitObject()
    {
        PrevMouseOverObject = MouseOverObject;
        PrevHitUnitPiece = HitUnitPiece;
        PrevHitHandCard = HitHandCard;
        PrevHitTile = HitTile;

        MouseOverObject = MouseOverObjects.None;
        HitUnitPiece = null;
        HitHandCard = null;
        HitTile = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (settings.IsOnCardLayer(hit.collider)) //Hand card
            {
                HitHandCard = hit.collider.GetComponent<Card>();
                if (HitHandCard != null)
                {
                    if (HitHandCard.IsMainPlayer && HitHandCard.IsHandcard)
                        MouseOverObject = MouseOverObjects.HandCard;
                }
                else
                    Debug.Log("Missing card script on " + hit.collider.gameObject);
            }
            else if (settings.IsOnTileLayer(hit.collider)) //Tile
            {
                HitTile = hit.collider.GetComponent<DummyBoardTile>();
                if (HitTile != null)
                    MouseOverObject = MouseOverObjects.Tile;
                else
                    Debug.Log("Missing tile script on " + hit.collider.gameObject);
            }
            else if (settings.IsOnUnitPieceLayer(hit.collider)) //Unit piece
            {
                HitUnitPiece = hit.collider.GetComponent<UnitPiece>();
                if (HitUnitPiece != null)
                    MouseOverObject = MouseOverObjects.UnitPiece;
                else
                    Debug.Log("Missing unit piece script on " + hit.collider.gameObject);
            }
        }
    }
    #endregion
}