using System.Collections;
using UnityEngine;

public class PlayerCardPlacementControl : MonoBehaviour
{
    #region Field and ctor
    //Status
    BoardTile highlightedTile;

    //Raycast hit objects

    //Ref
    RealPlayer player;
    GameSettings settings;
    BoardManager board;

    public PlayerCardPlacementControl(RealPlayer player)
    {
        //Ref
        this.player = player;
        settings = GameSettings.Instance;
        board = BoardManager.Instance;
    }
    #endregion

    public void TickUpdate()
    {
        //Cancel mode
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            player.CancelCardPlacement();
        }

        HighlightUpdate();
        ClickUpdate();
    }

    void HighlightUpdate ()
    {
        if (MouseExitsAllTiles)
        {
            ExitHighlightOnPrevTile();
        }

        if (MouseEntersNewTile)
        {
            ExitHighlightOnPrevTile();
            EnterHighlightOnNewTile();
        }
    }

    void ClickUpdate ()
    {
        if (OnTile && Input.GetMouseButton(0))
        {
            //Clean up highlight
            player.CurrP1Tile.ExitHighlight();

            //Remove selection slot card
            if (player.SelectionSlot.TryRemoveCard(out Card card))
            {
                player.DiscardPile.AddToDiscardPile(card);
            }

            //Spawn unit

            //Phase transition
            player.SpawnedAUnitPiece();
        }
    }

    #region Minor methods
    bool MouseEntersNewTile => OnTile && player.CurrP1Tile != player.PrevP1Tile;
    bool MouseExitsAllTiles => PrevOnTile && !OnTile;
    bool OnTile => player.CurrP1Tile != null;
    bool PrevOnTile => player.PrevP1Tile != null;

    void ExitHighlightOnPrevTile()
    {
        if (PrevOnTile)
            player.PrevP1Tile.ExitHighlight();
    }

    void EnterHighlightOnNewTile()
    {
        if (player.CurrP1Tile != null)
        {
            player.CurrP1Tile.EnterHighlight();
        }
        else
            Debug.LogError("Something wrong");
    }
    #endregion
}