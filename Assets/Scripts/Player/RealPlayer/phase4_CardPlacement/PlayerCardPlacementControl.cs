using System.Collections;
using UnityEngine;

public class PlayerCardPlacementControl : MonoBehaviour
{
    #region Field and ctor
    //Ref
    RealPlayer player;
    GameSettings settings;
    BoardManager board;
    UnitPieceManager unitSpawner;

    public PlayerCardPlacementControl(RealPlayer player)
    {
        //Ref
        this.player = player;
        settings = GameSettings.Instance;
        board = BoardManager.Instance;
        unitSpawner = UnitPieceManager.Instance;
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
        if (player.MouseExitsAllTiles)
        {
            player.ExitHighlightOnPrevTile();
        }

        if (player.MouseEntersNewTile)
        {
            player.ExitHighlightOnPrevTile();
            player.EnterHighlightOnNewTile();
        }
    }

    void ClickUpdate ()
    {
        if (player.OnTile && Input.GetMouseButton(0))
        {
            //Remove selection slot card
            if (player.SelectionSlot.TryRemoveCard(out Card card))
            {
                //Clean up highlight
                player.CurrSpawnTile.ToggleHoverHighlight(false);
                player.Hand.HideHand();
                player.Hand.RefreshHandCardPositions();

                //Spawn unit
                PlayerUnit unit =  unitSpawner.SpawnUnit(card.CardType, player.CurrSpawnTile.Index, player);

                //Set unit
                player.CurrSpawnTile.SetUnitPiece(unit);

                //Phase transition
                player.EnterCardSelection();

                //Discard card
                player.DiscardPile.AddToDiscardPile(card);
            }
            else
                Debug.LogError("Something wrong");
        }
    }
}