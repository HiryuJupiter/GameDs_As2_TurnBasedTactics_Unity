using System.Collections;
using UnityEngine;

public class PlayerCardPlacementControl : MonoBehaviour
{
    #region Field and ctor
    //Status

    //Raycast hit objects

    //Ref
    RealPlayer player;
    GameSettings settings;

    public PlayerCardPlacementControl(RealPlayer player)
    {
        //Ref
        this.player = player;
        settings = GameSettings.Instance;
    }
    #endregion

    public void TickUpdate()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            //CancelCardSelection();
        }
    }

    //void CancelCardSelection()
    //{
    //    //Card
    //    ReturnSelectionSlotCardToHand();
    //    UnhighlightCards();
    //    Hand.RefreshHandCardPositions();

    //    //Status change
    //    ControlState = ControlStates.Standby;
    //    Hand.RaiseHand();
    //}


    //void PlaceCard()
    //{
    //    //Clear highlight
    //    UnhighlightCards();

    //    //Card
    //    DiscardPile.AddToDiscardPile(SelectionSlot.Card);

    //    //Status change
    //    ControlState = ControlStates.Standby;
    //    Hand.RaiseHand();
    //}
}



