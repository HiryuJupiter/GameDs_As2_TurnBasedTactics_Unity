using System.Collections;
using UnityEngine;

public class RealPlayer : Player
{
    PlayerCardSelectionControl selectionControl;
    PlayerCardPlacementControl placementControl;
    PlayerUnitMovingControl unitMoveControl;

    #region Raycast Targets
    //Currently raycast hit unit piece
    public PlayerUnit CurrUnit { get; protected set; }
    public PlayerUnit PrevUnit { get; protected set; }
    //Currently raycast hit hand-card
    public Card CurrCard { get; protected set; }
    public Card PrevCard { get; protected set; }
    //Currently raycast hit tile of player 1 (for spawning pieces)
    public BoardTile CurrSpawnTile { get; protected set; }
    public BoardTile PrevSpawnTile { get; protected set; }
    //Tile of any player
    public BoardTile CurrAnyTile { get; protected set; }
    public BoardTile PrevAnyTile { get; protected set; }
    #endregion

    #region Card selection 
    public bool MouseEntersNewCard => OnCard && CurrCard != PrevCard;
    public bool MouseExitsAllCards => PrevOnCard && !OnCard;
    public bool OnCard => CurrCard != null;
    public bool PrevOnCard => PrevCard != null;

    public void ExitHighlightOnAllCards() => Hand.ExitHighlightOnAllCards();

    public void EnterHighlightOnNewCard()
    {
        if (CurrCard != null)
        {
            CurrCard.EnterHighlight();
        }
    }
    #endregion

    #region Unit selection 
    public bool MouseEntersNewUnit => OnUnit && CurrUnit != PrevUnit;
    public bool MouseExitsAllUnits => PrevOnUnit && !OnUnit;
    public bool OnUnit => CurrUnit != null;
    public bool PrevOnUnit => PrevUnit != null;

    public void EnterHighlightOnNewUnit()
    {
        if (CurrUnit != null)
        {
            CurrUnit.TogglehoverHighlight(true);
        }
    }

    public void ExitHighlightOnPrevUnit()
    {
        if (PrevOnUnit)
        {
            PrevUnit.TogglehoverHighlight(false);
        }
    }
    #endregion

    #region Spawn tile selection
    public bool MouseEntersNewTile => OnTile && CurrSpawnTile != PrevSpawnTile;
    public bool MouseExitsAllTiles => PrevOnTile && !OnTile;
    public bool OnTile => CurrSpawnTile != null;
    public bool PrevOnTile => PrevSpawnTile != null;

    public void ExitHighlightOnPrevTile()
    {
        if (PrevOnTile)
            PrevSpawnTile.ToggleHoverHighlight(false);
    }

    public void EnterHighlightOnNewTile()
    {
        if (CurrSpawnTile != null)
        {
            CurrSpawnTile.ToggleHoverHighlight(true);
        }
    }
    #endregion

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
        return;
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
        GUI.Label(new Rect(700f, 40f, 200f, 20f), "PrevTile: " + PrevSpawnTile);
        GUI.Label(new Rect(700f, 60f, 200f, 20f), "CurrUnit: " + CurrUnit);
        GUI.Label(new Rect(700f, 80f, 200f, 20f), "CurrCard: " + CurrCard);
        GUI.Label(new Rect(700f, 100f, 200f, 20f), "CurrTile: " + CurrSpawnTile);

    }

    #region Raycast hit
    Ray ray => Camera.main.ScreenPointToRay(Input.mousePosition);

    void CheckForRaycastHitObject()
    {
        //PrevMouseOverObject = MouseOverObject;
        PrevUnit = CurrUnit;
        PrevCard = CurrCard;
        PrevSpawnTile = CurrSpawnTile;
        PrevAnyTile = CurrAnyTile;

        //MouseOverObject = MouseOverObjects.None;
        CurrUnit = null;
        CurrCard = null;
        CurrSpawnTile = null;
        CurrAnyTile = null;

        if (Physics.Raycast(ray, out RaycastHit hitCard, 100f, settings.CardLayer)) //Hand card
        {
            Card c = hitCard.collider.GetComponent<Card>();
            if (c != null && c.IsMainPlayer && c.IsHandcard)
            {
                CurrCard = c;
            }
        }

        if (Physics.Raycast(ray, out RaycastHit hitTile, 100f, settings.TileLayer)) //Tile
        {
            BoardTile t = hitTile.collider.GetComponent<BoardTile>();
            if (t != null)
            {
                CurrAnyTile = t;
                if (t.IsMainPlayer && !t.IsOccupied)
                {
                    CurrSpawnTile = t;
                }
            }
        }
        if (Physics.Raycast(ray, out RaycastHit hitUnit, 100f, settings.UnitPieceLayer)) //Unit piece
        {
            PlayerUnit unit = hitUnit.collider.GetComponent<PlayerUnit>();
            if (unit != null && unit.IsMainPlayer)
            {
                CurrUnit = unit;
            }
        }
    }
    #endregion
}

/*
      //if (Physics.Raycast(ray, out RaycastHit hit, 100f))
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
                    if (t.IsMainPlayer && !t.IsOccupied)
                    {
                        CurrSpawnTile = t;
                    }
                }
            }
            else if (settings.IsOnUnitPieceLayer(hit.collider)) //Unit piece
            {
                UnitPiece unit = hit.collider.GetComponent<UnitPiece>();
                if (unit != null && unit.IsMainPlayer)
                {
                    CurrUnit = unit;
                }
            }
        }
 */