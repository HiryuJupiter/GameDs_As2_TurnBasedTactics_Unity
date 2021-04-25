using System.Collections;
using UnityEngine;

public class PlayerUnitMovingControl : MonoBehaviour
{
    UnitPiece movingUnit;
    UnitPiece targetUnit;
    BoardTile targetTile;

    enum Mode { SelectUnit, MoveUnit, AttackUnit}

    //Ref
    RealPlayer player;
    GameSettings settings;
    BoardManager board;

    Mode mode = Mode.SelectUnit;

    public PlayerUnitMovingControl(RealPlayer player)
    {
        this.player = player;
        settings = GameSettings.Instance;
        board = BoardManager.Instance;
    }

    public void TickUpdate()
    {
        HighlightUpdate();
        switch (mode)
        {
            case Mode.SelectUnit:
                UnitSelectionCheck();
                break;
            case Mode.MoveUnit:
                UnitMoveUpdate();
                break;
            case Mode.AttackUnit:
                break;
            default:
                break;
        }
        
    }

    void HighlightUpdate()
    {
        if (player.MouseExitsAllUnits)
        {
            player.ExitHighlightOnPrevUnit();
        }

        if (player.MouseEntersNewUnit)
        {
            player.ExitHighlightOnPrevUnit();
            player.EnterHighlightOnNewUnit();
        }
    }

    void UnitSelectionCheck()
    {
        if (Input.GetMouseButton(0))
        {
            if (player.OnUnit)
            {
                if (movingUnit != player.CurrUnit)
                {
                    if (movingUnit != null)
                        movingUnit.ToggleSelectionHighlight(false);

                    movingUnit = player.CurrUnit;
                    player.CurrUnit.ToggleSelectionHighlight(true);
                    //mode = Mode.MoveUnit;
                }
            }
            else
            {
                if (player.PrevUnit != null)
                    player.PrevUnit.ToggleSelectionHighlight(false);
            }
        }
    }

    void UnitMoveUpdate ()
    {

    }
}