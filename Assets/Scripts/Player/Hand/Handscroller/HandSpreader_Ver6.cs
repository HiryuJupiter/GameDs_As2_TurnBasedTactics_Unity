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
        int cardCount;
        float startXPos;
        float zRotationStart;
        float zRotationOffset;
        float yRotationStart;
        float yRotationOffset;
        float spacing;
        float startXPos_Squared;
        float maxVerticalOffset;

        //Cache
        readonly Player player;
        readonly Hand hand;
        readonly bool isRealPlayer;

        readonly float leftExtent; //Distance from middle to the left edge
        readonly float sign;
        readonly float baseSpacing; //X position spacing
        readonly Vector3 centerPos;
        readonly Vector3 facingPos;

        //readonly Quaternion baseFacingRot;
        readonly float baseZRotationStart;
        readonly float baseZRotationOffset;
        readonly Quaternion baseYRot;
        readonly float baseVerticalOffset; //Y position offset

        //Quaternion yRot; //Y axis tilt to make cards not intersect into each other

        //Properties
        float TotalLayoutWidth => baseSpacing * (cardCount - 1);

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

            //baseFacingRot = Quaternion.LookRotation((facingPos - centerPos), Vector3.up);
            baseZRotationStart = sign * setting.ZRotationStart;
            baseZRotationOffset = -sign * setting.ZRotationOffset;
            //baseYRotationStart = setting.YRotationStart;
            //baseYRotationOffset = setting.YRotationOffset;
            baseVerticalOffset = setting.VerticalOffset;

            baseYRot = Quaternion.Euler(0f, isRealPlayer ? -15f : -5f, 0f);
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

        private void UpdateCardPosition (int index)
        {
            Card card = hand.Cards[index];

            //Position based on index
            Vector3 pos = centerPos;
            pos.x = startXPos + spacing * index;

            //Rotation
            //Quaternion rot = RotationTowardsCamera(pos) * yRot;
            //Quaternion yRot = Quaternion.Euler(0f, yRotationStart + yRotationOffset * index, 0f);
            Quaternion rot = RotationTowardsCamera(pos)  * baseYRot * zRot(index);
            card.SetTargetRotation(rot);

            //Vertical positional offset based on rotation
            float distToCenterSquared = pos.x * pos.x;
            float perc = (startXPos == 0) ? 1 : 1 - (distToCenterSquared / startXPos_Squared);
            pos = pos + rot.normalized * new Vector3(0f, maxVerticalOffset * perc, 0f);

            card.SetTargetPosition(pos);
        }
        #endregion

        #region Minor methods
        Quaternion zRot(int index) => Quaternion.Euler(0f, 0f, zRotationStart + (index * zRotationOffset)); //Z-axis rotation of card
        Quaternion RotationTowardsCamera(Vector3 pos) => Quaternion.LookRotation((facingPos - pos), Vector3.up); //Rotation towards the facing object.

        void CalculateLayoutParameters()
        {
            startXPos = -TotalLayoutWidth / 2f;

            if (!LayoutBeyondExtent(startXPos))
            {
                //yRotationStart = -baseYRotationOffset * (cardCount - 1) / 1.5f;
                //yRotationOffset = baseYRotationOffset;

                zRotationStart = -baseZRotationOffset * (cardCount - 1) / 2f;
                zRotationOffset = baseZRotationOffset;
                spacing = baseSpacing;
            }
            else
            {
                startXPos = leftExtent;
                //yRotationStart = baseYRotationStart;
                //yRotationOffset = -baseYRotationStart * 2f / cardCount;
                zRotationStart = baseZRotationStart;
                zRotationOffset = -baseZRotationStart * 2f / cardCount;
                spacing = (-leftExtent * 2f) / cardCount;
            }
            
            Debug.Log("yRotationStart: " + yRotationStart);
            startXPos_Squared = startXPos * startXPos;
            maxVerticalOffset = baseVerticalOffset * cardCount;
        }

        bool LayoutBeyondExtent (float startXPos)  => isRealPlayer ? (startXPos < leftExtent) : (startXPos > leftExtent);

        float MouseOffset()
        {
            return 0f;
        }
        #endregion
    }
}
