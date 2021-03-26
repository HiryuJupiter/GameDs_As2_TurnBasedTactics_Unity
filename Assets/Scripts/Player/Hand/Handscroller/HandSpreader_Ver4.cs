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

    public class HandSpreader_Ver4
    {
        //Status
        int cardCount;

        //Cache
        readonly Player player;
        readonly Hand hand;

        readonly bool isRealPlayer;

        readonly float leftExtent; //Distance from middle to the left edge
        readonly float sign;
        readonly float spacing;
        readonly float rotationOffset;
        readonly float startRotation;
        readonly float verticalOffset;

        readonly Vector3 centerPos;
        readonly Quaternion yTilt; //Y axis tilt to make cards not intersect into each other
        readonly Vector3 facingPos;

        //Properties
        float TotalLayoutWidth => spacing * (cardCount - 1);

        #region Public
        public HandSpreader_Ver4(Player player, Transform centuralCardReference, Transform leftLimit, Vector3 facingPos)
        {
            //Reference
            this.player = player;
            hand = player.PlayerHand;
            CardSettings setting = CardSettings.Instance;

            //Cache
            isRealPlayer = player.IsMainPlayer;

            leftExtent = leftLimit.position.x;
            sign = Mathf.Sign(leftExtent);
            spacing = sign * -setting.spacing;
            rotationOffset = -sign * setting.RotationOffset;
            startRotation = sign * setting.StartingRotation;
            verticalOffset = setting.VerticalOffset;

            centerPos = centuralCardReference.position;
            yTilt = Quaternion.Euler(0f, -15f, 0f);
            this.facingPos = facingPos;
        }

        public void UpdateCardPositions()
        {
            //Cache
            cardCount = hand.Cards.Count;

            //Guard
            if (cardCount <= 0)
                return;

            if (isRealPlayer)
                UpdatePlayerCardPosition();
            else
                UpdateAICardPosition();
        }

        public void HighlightCard (Card card)
        {
            if (hand.Cards.Contains(card))
            {

            }
        }

        private void UpdatePlayerCardPosition ()
        {
            CalculateLayoutParameters(out float startXPos, out float startingRot, out float spacing, out float rotationOffset);
            float startXSquared = startXPos * startXPos;
            float totalVerticalOffset = verticalOffset * cardCount;

            for (int i = 0; i < cardCount; i++)
            {
                Card card = hand.Cards[i];

                //Position based on index
                Vector3 p = centerPos;
                p.x = startXPos + spacing * i;

                //Rotation
                Quaternion rot = RotationTowardsCamera(p) * yTilt * zTilt(startingRot, rotationOffset, i);
                card.SetTargetRotation(rot);

                //Vertical positional offset based on rotation
                if (startXPos == 0)
                {
                    card.SetTargetPosition(p);
                }
                else
                {
                    float distToCenterSquared = p.x * p.x;
                    float perc = 1 - (distToCenterSquared / startXSquared);

                    //hand.Cards[i].SetTargetPositional(p + rot * new Vector3(0f, VerticalOffset * perc, 0f));
                    card.SetTargetPosition(p + rot.normalized * new Vector3(0f, totalVerticalOffset * perc, 0f));
                }
            }
        }

        void UpdateAICardPosition()
        {
            CalculateLayoutParameters(out float startXPos, out float startRot, out float spacing, out float rotationOffset);

            for (int i = 0; i < cardCount; i++)
            {
                //Position part 1: position based on index
                Vector3 p = centerPos;
                p.x = startXPos + spacing * i;
                hand.Cards[i].SetTargetPosition(p);

                //Rotation
                Vector3 dirToCamera = facingPos - p;
                Quaternion rot = Quaternion.LookRotation(dirToCamera, Vector3.up);
                hand.Cards[i].SetTargetRotation(rot);
            }
        }
        #endregion

        #region Minor methods
        Quaternion zTilt(float startingRot, float rotationOffset, int index) => Quaternion.Euler(0f, 0f, startingRot + (index * rotationOffset));
        Quaternion RotationTowardsCamera(Vector3 pos) => Quaternion.LookRotation((facingPos - pos), Vector3.up);

        void CalculateLayoutParameters(out float startXPos, out float startingRot, out float spacing, out float rotationOffset)
        {
            startXPos = -TotalLayoutWidth / 2f;

            if (!LayoutBeyondExtent(startXPos))
            {
                startingRot = -this.rotationOffset * (cardCount - 1) / 2f;
                rotationOffset = this.rotationOffset;
                spacing = this.spacing;
            }
            else
            {
                startXPos = leftExtent;
                startingRot = startRotation;
                rotationOffset = -startRotation * 2f / cardCount;
                spacing = (-leftExtent * 2f) / cardCount;
            }
        }

        //Layout width has gone beyond extent.
        bool LayoutBeyondExtent (float startXPos)  => isRealPlayer ? (startXPos < leftExtent) : (startXPos > leftExtent);

        float MouseOffset()
        {
            return 0f;
        }
        #endregion
    }
}
