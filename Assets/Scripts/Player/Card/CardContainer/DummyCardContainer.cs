using UnityEngine;
using System;
using System.Collections.Generic;

namespace TurnBasedGame.CardManagement
{
    public class DummyCardContainer : Card
    {
        private static Card HighlightedCard;

        private void OnMouseEnter()
        {
            if (!player.IsMainPlayer || GamePhaseManager.Phase != GamePhases.PlayingHand)
                return;

            if (HighlightedCard != this)
            {
                HighlightedCard = this;

                //Enable highlight mode
            }
        }


        private void OnMouseExit()
        {
            if (HighlightedCard == this)
            {
                HighlightedCard = null;

                //Disable highlight mode
            }
        }

        private void OnMouseDown()
        {
            if (player.IsMainPlayer && GamePhaseManager.Phase == GamePhases.PlayingHand)
            {
                Debug.Log("clicked on" + name);
                player.RemoveCard(this);
            }
        }
    }
}
