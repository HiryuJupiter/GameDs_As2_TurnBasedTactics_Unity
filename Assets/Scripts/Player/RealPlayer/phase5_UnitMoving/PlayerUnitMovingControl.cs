using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitMovingControl : MonoBehaviour
{
    BoardUnit movingUnit;
    BoardUnit targetUnit;
    BoardTile targetTile;

    public enum Mode { SelectUnit, MoveUnit, AttackUnit }

    //Ref
    RealPlayer player;
    GameSettings settings;
    //BoardManager board;

    public Mode mode { get; private set; } = Mode.SelectUnit;

     

    public PlayerUnitMovingControl(RealPlayer player)
    {
        this.player = player;
        settings = GameSettings.Instance;
        //board = BoardManager.Instance;
    }

    public void TickUpdate()
    {
        HighlightUpdate();
        switch (mode)
        {
            case Mode.SelectUnit:
                UnitSelectionCheck();
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
                {
                    mode = Mode.SelectUnit;
                    player.GoToCardSelection();
                }
                break;
            case Mode.MoveUnit:
                UnitMoveUpdate2();
                if (movingUnit == null)
                {
                    mode = Mode.SelectUnit;
                    //player.GoToCardSelection();
                }
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
                    EnterMoveUpdate();
                }
            }
            else
            {
                if (player.PrevUnit != null)
                    player.PrevUnit.ToggleSelectionHighlight(false);
            }
        }
    }

    void UnitMoveUpdate2()
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

        if (player.OnTile && Input.GetMouseButtonDown(0))
        {
            movingUnit.MoveToPosition(player.CurrSpawnTile.attachPoint.position);
            //movingUnit.MoveToPosition(player.CurrSpawnTile.transform.position);
            player.CurrSpawnTile.ToggleHoverHighlight(false);
            movingUnit.ToggleSelectionHighlight(false);

            player.GoToCardSelection();
        }
    }

    List<GameObject> movableHexes;
    Hex attachedHex;
    int currentX;
    int currentY;

    void EnterMoveUpdate ()
    {
        movableHexes = new List<GameObject>();
        mode = Mode.MoveUnit;
        attachedHex = movingUnit.attachedHex;
        currentX = attachedHex.horiPoint;
        currentY = attachedHex.vertPoint;
    }

    void UnitMoveUpdate()
    {
        if (attachedHex.oddLane == true)
        {
            if (currentY < 7)
            {
                movableHexes.Add(Layout.LayoutManager.hexPoints[currentY + 1, currentX]);
                if (currentX < 5)
                {
                    movableHexes.Add(Layout.LayoutManager.hexPoints[currentY + 1, currentX + 1]);
                }
                if (currentX > 0)
                {
                    movableHexes.Add(Layout.LayoutManager.hexPoints[currentY + 1, currentX - 1]);
                }
            }

            if (currentY > 0)
            {
                movableHexes.Add(Layout.LayoutManager.hexPoints[currentY - 1, currentX]);
            }
            if (currentX > 0)
            {
                movableHexes.Add(Layout.LayoutManager.hexPoints[currentY, currentX - 1]);
            }
            if (currentX < 5)
            {
                movableHexes.Add(Layout.LayoutManager.hexPoints[currentY, currentX + 1]);
            }


        }
        else
        {
            if (currentY > 0)
            {
                movableHexes.Add(Layout.LayoutManager.hexPoints[currentY - 1, currentX]);


                if (currentX > 0)
                {
                    movableHexes.Add(Layout.LayoutManager.hexPoints[currentY - 1, currentX - 1]);
                }
            }
            if (currentX > 0)
            {
                movableHexes.Add(Layout.LayoutManager.hexPoints[currentY, currentX - 1]);
            }

            if (currentY < 7)
            {
                movableHexes.Add(Layout.LayoutManager.hexPoints[currentY + 1, currentX]);


            }
            if (currentX < 5)
            {
                movableHexes.Add(Layout.LayoutManager.hexPoints[currentY, currentX + 1]);
                if (currentY > 0)
                {
                    movableHexes.Add(Layout.LayoutManager.hexPoints[currentY - 1, currentX + 1]);
                }
            }
        }


        foreach (GameObject highlighedHex in movableHexes)
        {

            Hex newHex = highlighedHex.GetComponent<Hex>();
            if (newHex.attachedObject != null)
            {
                newHex.movable = false;

                if (newHex.attachedObject.GetComponent<BoardUnit>().blue == false)
                {
                    newHex.movable = false;
                    newHex.attachedRed = true;
                }
            }
            else
            {
                newHex.movable = true;
            }
        }
    }
}