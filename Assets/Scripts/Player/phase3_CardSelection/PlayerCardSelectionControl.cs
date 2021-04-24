
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
        if (OnCard && Input.GetMouseButtonDown(0))
        {
            Card card = player.CurrCard;

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

    #endregion

    #region Minor methods
    bool MouseEntersNewCard => OnCard && player.CurrCard != player.PrevCard;
    bool MouseExitsAllCards => PrevOnCard && !OnCard;
    bool OnCard => player.CurrCard != null;
    bool PrevOnCard => player.PrevCard != null;

    void ExitHighlightOnAllCards () => player.Hand.ExitHighlightOnAllCards();

    void EnterHighlightOnNewCard ()
    {
        if (player.CurrCard != null)
        {
            player.CurrCard.EnterHighlight();
        }
    }
    #endregion
}