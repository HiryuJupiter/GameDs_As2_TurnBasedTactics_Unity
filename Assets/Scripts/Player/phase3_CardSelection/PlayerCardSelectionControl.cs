
using System.Collections;
using UnityEngine;

public class PlayerCardSelectionControl : MonoBehaviour
{
    #region Field and ctor
    //Status

    //Raycast hit objects

    //Ref
    RealPlayer player;
    GameSettings settings;

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
        SelectionUpdate();
    }

    #region Card highlight
    void HighlightUpdate()
    {
        if (MouseExitsAllCards)
        {
            ExitHighlightOnAllCards();
        }

        if (MouseEntersNewCard)
        {
            ExitHighlightOnAllCards();
            EnterHighlightOnNewCard();
        }
    }
    #endregion

    #region Public - Card selection and placement
    void SelectionUpdate()
    {
        if (HoveringOverHandCard && Input.GetMouseButtonDown(0))
        {
            if (player.MouseOverObject == MouseOverObjects.HandCard)
            {
                Card card = player.HitHandCard;

                if (player.Hand.TryRemoveCardFromHand(card))
                {
                    //Exit highlight before card swap
                    ExitHighlightOnAllCards();

                    //Card swap
                    //player.ReturnSelectionSlotCardToHand();
                    player.SelectionSlot.SetAsSelectedCard(card);
                    
                    //Peripheral
                    player.Hand.RefreshHandCardPositions();
                    player.Hand.HideHand();

                    //Phase change
                    player.FinishedSelectingCard();
                }
                else
                {
                    Debug.LogError("Shouldn't happen. Card " + card);
                }
            }
        }
    }

    #endregion

    #region Minor methods
    bool MouseEntersNewCard => HoveringOverHandCard &&
            (player.HitHandCard != player.PrevHitHandCard || player.PrevMouseOverObject != MouseOverObjects.HandCard);
    bool HoveringOverHandCard => player.MouseOverObject == MouseOverObjects.HandCard;
    bool MouseExitsAllCards => player.PrevMouseOverObject == MouseOverObjects.HandCard &&
        player.MouseOverObject != MouseOverObjects.HandCard;

    void ExitHighlightOnAllCards () => player.Hand.ExitHighlightOnAllCards();

    void EnterHighlightOnNewCard ()
    {
        if (player.HitHandCard != null)
        {
            player.HitHandCard.EnterHighlight();
        }
    }
    #endregion
}