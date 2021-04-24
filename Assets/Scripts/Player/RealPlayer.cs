using System.Collections;
using UnityEngine;

public class RealPlayer : Player
{
    #region Field and mono
    PlayerCardSelectionControl selectionControl;
    PlayerCardPlacementControl placementControl;
    PlayerUnitMovingControl unitMoveControl;

    //Currently raycast hit unit piece
    public UnitPiece CurrUnit { get; private set; }
    public UnitPiece PrevUnit { get; private set; }
    //Currently raycast hit hand-card
    public Card CurrCard { get; private set; }
    public Card PrevCard { get; private set; }
    //Currently raycast hit tile of player 1 (for spawning pieces)
    public BoardTile CurrP1Tile { get; private set; }
    public BoardTile PrevP1Tile { get; private set; }
    //Tile of any player
    public BoardTile CurrAnyTile { get; private set; }
    public BoardTile PrevAnyTile { get; private set; }

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
            GUI.Label(new Rect(550f, 20 + 20f * i, 200f, 20f),
                    i + ": " + DiscardPile.Cards[i]);
        }

        GUI.Label(new Rect(700f, 0f, 200f, 20f), "PrevUnit: " + PrevUnit);
        GUI.Label(new Rect(700f, 20f, 200f, 20f), "PrevCard: " + PrevCard);
        GUI.Label(new Rect(700f, 40f, 200f, 20f), "PrevTile: " + PrevP1Tile);
        GUI.Label(new Rect(700f, 60f, 200f, 20f), "CurrUnit: " + CurrUnit);
        GUI.Label(new Rect(700f, 80f, 200f, 20f), "CurrCard: " + CurrCard);
        GUI.Label(new Rect(700f, 100f, 200f, 20f), "CurrTile: " + CurrP1Tile);

    }
    #endregion

    #region Public - Phase transitions
    public void FinishedSelectingCard()
    {
        phaseManager.ToP4_CardPlacementPhase();
    }

    public void SpawnedAUnitPiece()
    {
        EnterHandCardSelection();
    }

    public void EnterUnitControlMode() //Also invoked by UI button click
    {
        phaseManager.ToP5_UnitControlMode();
    }

    public void CancelCardPlacement() //Also invoked by UI button click
    {
        //Return selection slot card to hand
        if (SelectionSlot.TryRemoveCard(out Card card))
        {
            Hand.AddCard(card);
        }

        EnterHandCardSelection();
    }

    void EnterHandCardSelection()
    {
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
        //PrevMouseOverObject = MouseOverObject;
        PrevUnit = CurrUnit;
        PrevCard = CurrCard;
        PrevP1Tile = CurrP1Tile;
        PrevAnyTile = CurrAnyTile;

        //MouseOverObject = MouseOverObjects.None;
        CurrUnit = null;
        CurrCard = null;
        CurrP1Tile = null;
        CurrAnyTile = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (settings.IsOnCardLayer(hit.collider)) //Hand card
            {
                Card c = hit.collider.GetComponent<Card>();
                if (c != null && c.IsMainPlayer && c.IsHandcard)
                {
                    CurrCard = c;
                }
            }
            else if (settings.IsOnTileLayer(hit.collider)) //Tile
            {
                BoardTile t = hit.collider.GetComponent<BoardTile>();
                if (t != null)
                {
                    CurrAnyTile = t;
                    if (t.IsMainPlayer)
                    {
                        CurrP1Tile = t;
                    }
                }
            }
            else if (settings.IsOnUnitPieceLayer(hit.collider)) //Unit piece
            {
                CurrUnit = hit.collider.GetComponent<UnitPiece>();
                //if (HitUnitPiece != null)
                //    MouseOverObject = MouseOverObjects.UnitPiece;
                //else
                //    Debug.Log("Missing unit piece script on " + hit.collider.gameObject);
            }
        }
    }
    #endregion
}