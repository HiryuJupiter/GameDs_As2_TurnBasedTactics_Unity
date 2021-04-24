
using System.Collections;
using UnityEngine;

public class PlayerUnitMovingControl : MonoBehaviour
{
    //Ref
    RealPlayer player;
    GameSettings settings;

    Object hitObject;

    public enum RaycastObjectTypes { None, HandCard, Piece, Tile}

    public PlayerUnitMovingControl(RealPlayer player)
    {
        this.player = player;
        settings = GameSettings.Instance;
    }

    public void PerformRaycast ()
    {
        //Check if raycast hits A. a valid hand card B. Tile C. Unit piece
        
    }

    public void TickUpdate()
    {

    }


    bool TryCheckForUnitPiece (out UnitPiece unitPiece)
    {
        unitPiece = null;
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, settings.UnitPieceLayer))
        {
            unitPiece = hit.collider.GetComponent<UnitPiece>();
            if (unitPiece != null && unitPiece.IsMainPlayer)
            {
                hitObject = unitPiece;
                return true;
            }
        }
        return false;
    }

    bool TryCheckForHandCard (out Card card)
    {
        card = null;
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, settings.CardLayer))
        {
            card = hit.collider.GetComponent<Card>();
            if (card != null && card.IsMainPlayer)
            {
                hitObject = card;
                return true;
            }
        }
        return false;
    }

    bool TryCheckForTile (out DummyBoardTile tile)
    {
        tile = null;
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, settings.TileLayer))
        {
            tile = hit.collider.GetComponent<DummyBoardTile>();
            if (tile != null && tile.IsMainPlayer)
            {
                hitObject = tile;
                return true;
            }
        }
        return false;
    }

    #region Expression bodies
    Ray ray => Camera.main.ScreenPointToRay(Input.mousePosition);
    #endregion
}