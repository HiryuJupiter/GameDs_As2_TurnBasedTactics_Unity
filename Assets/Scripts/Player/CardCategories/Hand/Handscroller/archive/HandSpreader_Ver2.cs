//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TurnBasedGame.CardManagement;
//

//namespace TurnBasedGame.HandManagement
//{
//    /*
//     Things to keep in mind: player 1 have their card running from left to right    
//     the opponent, Player 2, needs to have their card running from right to left
//     */

//    public class HandSpreader_Ver2 : MonoBehaviour, IHandSpreader
//    {
//        //The standard spacing between cards (before they become too crowded, before Mathf.Sign is calculated
//        const float BaseSpacingRaw = 1f;
//        const float BaseStartingRotation = 15f;
//        const float BaseRotationOffset = 2f; //Base per-card rotation offset going from left to right
//        const float VerticalOffset = 0.04f; //Per card

//        [Header("Positional references")]
//        [SerializeField] Transform centuralCardPosition;
//        [SerializeField] Transform leftLimit;
//        [SerializeField] Transform cardFacingTarget;

//        [Header("Settings")]
//        [Tooltip("The speed that the cards move in reaction to mouse movement")]
//        [SerializeField] float mouseMoveSpeed = 1f;

//        Player player;
//        Hand hand;

//        //Status
//        bool initialized;
//        bool isRealPlayer;

//        //Cache
//        float baseSpacing;
//        Vector3 centerPos;
//        Quaternion centerRot;
//        float leftExtent; //Distance from middle to the left edge
//        Quaternion cardTilt;

//        #region Public
//        public void Initilize(Player player)
//        {
//            initialized = true;

//            //Reference
//            this.player = player;
//            hand = player.PlayerHand;

//            //Cache
//            isRealPlayer = player.IsMainPlayer;
//            centerPos = centuralCardPosition.position;
//            centerRot = centuralCardPosition.rotation;
//            leftExtent = -(centuralCardPosition.position.x - leftLimit.position.x);
//            baseSpacing = Mathf.Sign(leftExtent) * -BaseSpacingRaw;
//            cardTilt = Quaternion.Euler(0f, isRealPlayer ? -15f : -10f, 0f);
//            //Debug.Log("Player " + (player.IsMainPlayer ? "1" : "2") + " left extent: " + leftExtent + ", baseSpacing: " + baseSpacing);
//        }

//        public void UpdateCardPositions()
//        {
//            if (!initialized || hand.Cards.Count <= 0)
//                return;

//            //Calculations
//            GetLayoutStartingXAndSpacing(out float startXPos, out float startingRot, out float spacing, out float rotationOffset);
//            //Debug.Log("Player " + (player.IsMainPlayer ? "1" : "2") + " startingRot: " + startingRot + " rotationOffset: " + rotationOffset);
//            float edgeXSquared = startXPos * startXPos;
//            float totalVerticalOffset = VerticalOffset * hand.Cards.Count;

//            for (int i = 0; i < hand.Cards.Count; i++)
//            {
//                //Position part 1: position based on index
//                Vector3 p = centerPos;
//                p.x = startXPos + spacing * i;

//                //Rotation
//                Vector3 dirToCamera = cardFacingTarget.position - p;
//                Quaternion rot = Quaternion.LookRotation(dirToCamera, Vector3.up) * cardTilt * Quaternion.Euler(0f, 0f, startingRot + (i * rotationOffset));
//                hand.Cards[i].SetTargetRotation(rot);

//                //Position part 2: positional vertical offset based on rotation, to make the cards move up a bit to form a mound.
//                //The middle card should move up the furthest. However, due to the mound being an inverse parabolic shape ...
//                //... we'll use OneMinus value of the squared value of distToCenter.
//                if (edgeXSquared == 0)
//                {
//                    hand.Cards[i].SetTargetPosition(p, true);
//                }
//                else
//                {
//                    float distToCenterSquared = p.x * p.x;
//                    float perc = 1 - (distToCenterSquared / edgeXSquared);

//                    //hand.Cards[i].SetTargetPositional(p + rot * new Vector3(0f, VerticalOffset * perc, 0f));
//                    hand.Cards[i].SetTargetPosition(p + rot.normalized * new Vector3(0f, totalVerticalOffset * perc, 0f), true);
//                }
//            }
//        }
//        #endregion

//        #region Minor methods
//        void GetLayoutStartingXAndSpacing(out float startXPos, out float startingRot, out float spacing, out float rotationOffset)
//        {
//            startXPos = -TotalLayoutWidthOfCardss() / 2f;
//            //If there are so many cards that the starting position is beyond the outer limit, recalculate.
//            bool tooManyCard = (player.IsMainPlayer) ? (startXPos < leftExtent) : (startXPos > leftExtent);

//            if (!tooManyCard)
//            {
//                //Rot
//                float totalRotation = BaseRotationOffset * (hand.Cards.Count - 1);
//                startingRot = Mathf.Sign(leftExtent) * totalRotation / 2f;
//                rotationOffset = -Mathf.Sign(leftExtent) * BaseRotationOffset;

//                //Spacing
//                spacing = baseSpacing;
//            }
//            else
//            {
//                //Pos
//                startXPos = leftExtent;

//                //Rot
//                startingRot = Mathf.Sign(leftExtent) * BaseStartingRotation;
//                rotationOffset = -Mathf.Sign(leftExtent) * BaseStartingRotation / hand.Cards.Count;

//                //Spacing
//                spacing = (-leftExtent * 2) / hand.Cards.Count;
//            }
//        }

//        float TotalLayoutWidthOfCardss()
//        {
//            float normlSpacingCardsWidth = baseSpacing * (hand.Cards.Count - 1);
//            //for (int i = 0; i < hand.Cards.Count - 1; i++)
//            //{
//            //    normlSpacingCardsWidth += baseSpacing;
//            //}
//            return normlSpacingCardsWidth;
//        }

//        float MouseOffset()
//        {
//            return 0f;
//        }
//        #endregion
//    }
//}
