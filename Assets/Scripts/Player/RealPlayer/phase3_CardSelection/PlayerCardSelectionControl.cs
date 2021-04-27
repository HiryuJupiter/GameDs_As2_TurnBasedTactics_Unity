
using System.Collections;
using UnityEngine;
using Units;


public class PlayerCardSelectionControl : MonoBehaviour
{
    #region Field and ctor
    //Status

    //Raycast hit objects

    //Ref
    RealPlayer player;
    GameSettings settings;
    UnitManager unitManager;
    PrefabDirectory prefabDirectory;
    
    public PlayerCardSelectionControl(RealPlayer player)
    {
        //Ref
        this.player = player;
        settings = GameSettings.Instance;
    }
    #endregion

    public void TickUpdate ()
    {
        HighlightUpdate();
       
    }

    #region Card highlight
    void HighlightUpdate()
    {
        if (player.MouseExitsAllCards)
        {
            player.ExitHighlightOnAllCards();
        }

        if (player.MouseEntersNewCard)
        {
            player.ExitHighlightOnAllCards();
            player.EnterHighlightOnNewCard();
        }
    }
    #endregion

    #region Public - Card selection and placement
    void SelectionUpdate()
    {
        if (player.OnCard && Input.GetMouseButtonDown(0))
        {
            Card card = player.CurrCard;

            if (player.Hand.TryRemoveCardFromHand(card))
            {

                
                //Where ive been trying to link - Ryan
                unitManager.EnterSpawnMode(card);

                //Exit highlight before card swap
                player.ExitHighlightOnAllCards();

                //Card swap
                //player.ReturnSelectionSlotCardToHand();
                player.SelectionSlot.SetAsSelectedCard(card);

                //Peripheral
                player.Hand.HideHand();
                player.Hand.RefreshHandCardPositions();

                //Phase change
                player.GoToCardPlacement();
            }
            else
            {
                Debug.LogError("Shouldn't happen. Card " + card);
            }
        }
    }

    #endregion
}