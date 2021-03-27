//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TurnBasedGame.CardManagement;
//using TurnBasedGame.PlayerManagement;

//namespace TurnBasedGame.HandManagement
//{
//    /*
//     Things to keep in mind: player 1 have their card running from left to right    
//     the opponent, Player 2, needs to have their card running from right to left

//    ZRotation: negative number is clockwise, positive number is anticlockwise
//     */

//    public class HandSpreader_Ver4
//    {
//        //Status
//        int cardCount;
//        float startXPos;
//        float startingRot;
//        float spacing;
//        float rotationOffset;
//        float startXPos_Squared;
//        float maxVerticalOffset;

//        //Cache
//        readonly Player player;
//        readonly Hand hand;
//        readonly bool isRealPlayer;

//        readonly float leftExtent; //Distance from middle to the left edge
//        readonly float sign;
//        readonly float baseSpacing;
//        readonly float baseRotationOffset;
//        readonly float baseStartingRotation;
//        readonly float baseVerticalOffset;

//        readonly Vector3 centerPos;
//        readonly Quaternion yRot; //Y axis tilt to make cards not intersect into each other
//        readonly Vector3 facingPos;

//        //Properties
//        float TotalLayoutWidth => baseSpacing * (cardCount - 1);

//        #region Constructor
//        public HandSpreader_Ver4(Player player, Transform centuralCardReference, Transform leftLimit, Vector3 facingPos)
//        {
//            //Reference
//            this.player = player;
//            hand = player.PlayerHand;
//            CardSettings setting = CardSettings.Instance;

//            //Cache
//            isRealPlayer = player.IsMainPlayer;

//            leftExtent = leftLimit.position.x;
//            sign = Mathf.Sign(leftExtent);
//            baseSpacing = sign * -setting.spacing;
//            baseRotationOffset = -sign * setting.RotationOffset;
//            baseStartingRotation = sign * setting.StartingRotation;
//            baseVerticalOffset = setting.VerticalOffset;

//            centerPos = centuralCardReference.position;
//            yRot = Quaternion.Euler(0f, isRealPlayer ? -15f : -5f, 0f);
//            this.facingPos = facingPos;
//        }
//        #endregion

//        #region Public
//        public void UpdateAllCardsPositions()
//        {
//            cardCount = hand.Cards.Count; //Cache

//            if (cardCount <= 0) //Guard
//                return;

//            CalculateLayoutParameters();
//            for (int i = 0; i < cardCount; i++)
//            {
//                UpdateCardPosition(i);
//            }
//        }

//        public void UpdateSingleCardPosition (int index)
//        {
//            cardCount = hand.Cards.Count;
//            CalculateLayoutParameters();
//            UpdateCardPosition(index);
//        }

//        private void UpdateCardPosition (int index)
//        {
//            Card card = hand.Cards[index];

//            //if (isRealPlayer)
//            {
//                //Position based on index
//                Vector3 pos = centerPos;
//                pos.x = startXPos + spacing * index;

//                //Rotation
//                //Quaternion rot = RotationTowardsCamera(pos) * yRot;
//                Quaternion rot = RotationTowardsCamera(pos) * yRot * zRot(index);
//                card.SetTargetRotation(rot);

//                //Vertical positional offset based on rotation
//                float distToCenterSquared = pos.x * pos.x;
//                float perc = (startXPos == 0) ? 1 : 1 - (distToCenterSquared / startXPos_Squared);
//                pos = pos + rot.normalized * new Vector3(0f, maxVerticalOffset * perc, 0f);

//                card.SetTargetPosition(pos);
//            }
//            //else
//            //{
//            //    //Position based on index
//            //    Vector3 pos = centerPos;
//            //    pos.x = startXPos + spacing * index;
//            //    pos.z += 0.1f;
//            //    card.SetTargetPosition(pos);

//            //    //Rotation
//            //    Quaternion rot = RotationTowardsCamera(pos);
//            //    card.SetTargetRotation(rot);
//            //}
//        }

//        //private void UpdatePlayerCardsPositions ()
//        //{
//        //    CalculateLayoutParameters(out float startXPos, out float startingRot, out float spacing, out float rotationOffset);
//        //    float startXSquared = startXPos * startXPos;
//        //    float totalVerticalOffset = verticalOffset * cardCount;

//        //    for (int i = 0; i < cardCount; i++)
//        //    {
//        //        Card card = hand.Cards[i];

//        //        //Position based on index
//        //        Vector3 pos = centerPos;
//        //        pos.x = startXPos + spacing * i;

//        //        //Rotation
//        //        Quaternion rot = RotationTowardsCamera(pos) * yTilt * zTilt(startingRot, rotationOffset, i);
//        //        card.SetTargetRotation(rot);

//        //        //Vertical positional offset based on rotation
//        //        float distToCenterSquared = pos.x * pos.x;
//        //        float perc = (startXPos == 0) ? 1 : 1 - (distToCenterSquared / startXSquared);
//        //        pos = pos + rot.normalized * new Vector3(0f, totalVerticalOffset * perc, 0f);

//        //        card.SetTargetPosition(pos);
//        //    }
//        //}
//        #endregion

//        #region Minor methods
//        Quaternion zRot(int index) => Quaternion.Euler(0f, 0f, startingRot + (index * rotationOffset)); //Z-axis rotation of card
//        Quaternion RotationTowardsCamera(Vector3 pos) => Quaternion.LookRotation((facingPos - pos), Vector3.up); //Rotation towards the facing object.

//        void CalculateLayoutParameters()
//        {
//            startXPos = -TotalLayoutWidth / 2f;

//            if (!LayoutBeyondExtent(startXPos))
//            {
//                startingRot = -baseRotationOffset * (cardCount - 1) / 2f;
//                rotationOffset = baseRotationOffset;
//                spacing = baseSpacing;
//            }
//            else
//            {
//                startXPos = leftExtent;
//                startingRot = baseStartingRotation;
//                rotationOffset = -baseStartingRotation * 2f / cardCount;
//                spacing = (-leftExtent * 2f) / cardCount;
//            }

//            startXPos_Squared = startXPos * startXPos;
//            maxVerticalOffset = baseVerticalOffset * cardCount;
//        }

//        bool LayoutBeyondExtent (float startXPos)  => isRealPlayer ? (startXPos < leftExtent) : (startXPos > leftExtent);

//        float MouseOffset()
//        {
//            return 0f;
//        }
//        #endregion
//    }
//}
