using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.CardManagement;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.HandManagement
{
    /*
     Things to keep in mind: player 1 have their card running from left to right    
     the opponent, Player 2, needs to have their card running from right to left

    ZRotation: negative number is clockwise, positive number is anticlockwise
     */

    public class HandSpreader_Ver6
    {
        //Status
        private int cardCount;
        private float startXPos;
        private float zRotationStart;
        private float zRotationOffset;
        private float spacing;
        private float startXPos_Squared;
        private float maxVerticalOffset;

        //Cache
        private readonly Player player;
        private readonly Hand hand;
        private readonly bool isRealPlayer;
         
        private readonly float leftExtent; //Distance from middle to the left edge
        private readonly float sign;
        private readonly float baseSpacing; //X position spacing
        private readonly Vector3 centerPos;
        private readonly Vector3 facingPos;
         
        private readonly float baseZRotationStart;
        private readonly float baseZRotationOffset;
        private readonly Quaternion baseYRot;
        private readonly float baseVerticalOffset; //Y position offset

        //Properties
        private float TotalLayoutWidth => baseSpacing * (cardCount - 1);

        #region Constructor
        public HandSpreader_Ver6(Player player, Transform centuralCardReference, Transform leftLimit, Vector3 facingPos)
        {
            //Reference
            this.player = player;
            hand = player.PlayerHand;
            CardSettings setting = CardSettings.Instance;

            //Cache
            isRealPlayer = player.IsMainPlayer;

            leftExtent = leftLimit.position.x;
            sign = Mathf.Sign(leftExtent);
            baseSpacing = sign * -setting.spacing;
            centerPos = centuralCardReference.position;
            this.facingPos = facingPos;

            baseZRotationStart = sign * setting.ZRotationStart;
            baseZRotationOffset = -sign * setting.ZRotationOffset;
            baseYRot = Quaternion.Euler(0f, isRealPlayer ? -15f : -5f, 0f);
            baseVerticalOffset = setting.VerticalOffset;
        }
        #endregion

        #region Public
        public void UpdateAllCardsPositions()
        {
            cardCount = hand.Cards.Count; //Cache

            if (cardCount <= 0) //Guard
                return;

            CalculateLayoutParameters();
            for (int i = 0; i < cardCount; i++)
            {
                UpdateCardPosition(i);
            }
        }

        public void UpdateSingleCardPosition (int index)
        {
            cardCount = hand.Cards.Count;
            CalculateLayoutParameters();
            UpdateCardPosition(index);
        }

        private void CalculateLayoutParameters()
        {
            startXPos = -TotalLayoutWidth / 2f;

            if (!LayoutBeyondExtent(startXPos))
            {
                zRotationStart = -baseZRotationOffset * (cardCount - 1) / 2f;
                zRotationOffset = baseZRotationOffset;
                spacing = baseSpacing;
            }
            else
            {
                startXPos = leftExtent;
                zRotationStart = baseZRotationStart;
                zRotationOffset = -baseZRotationStart * 2f / cardCount;
                spacing = (-leftExtent * 2f) / cardCount;
            }

            startXPos_Squared = startXPos * startXPos;
            maxVerticalOffset = baseVerticalOffset * cardCount / 2f;
        }

        private void UpdateCardPosition (int index)
        {
            Card card = hand.Cards[index];

            //Position based on index
            Vector3 pos = centerPos;
            pos.x = startXPos + spacing * index;

            //Rotation
            Quaternion rot = RotationTowardsCamera(pos)  * baseYRot * zRot(index);
            card.SetTargetRotation(rot, true);

            //Vertical positional offset based on rotation
            float distToCenterSquared = pos.x * pos.x; //The central x-position is 0, so pos.x is naturally the distance.
            float perc = (startXPos == 0) ? 1 : 1 - (distToCenterSquared / startXPos_Squared);
            pos = pos + rot.normalized * new Vector3(0f, maxVerticalOffset * perc, 0f);

            card.SetTargetPosition(pos);
        }
        #endregion

        #region Minor methods
        Quaternion zRot(int index) => Quaternion.Euler(0f, 0f, zRotationStart + (index * zRotationOffset)); //Z-axis rotation of card
        Quaternion RotationTowardsCamera(Vector3 pos) => Quaternion.LookRotation((facingPos - pos), Vector3.up); //Rotation towards the facing object.


        bool LayoutBeyondExtent (float startXPos)  => isRealPlayer ? (startXPos < leftExtent) : (startXPos > leftExtent);
        #endregion
    }
}
