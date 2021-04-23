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



//    public class HandSpreader_Ver1 : MonoBehaviour, IHandSpreader
//    {
//        //The standard spacing between cards (before they become too crowded, before Mathf.Sign is calculated
//        const float BaseSpacingRaw = 1f;

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
//        Quaternion cardTilt_RealPlayer;
//        Quaternion cardTilt_Enemy;

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
//            cardTilt_RealPlayer = Quaternion.Euler(0f, -15f, 0f);
//            cardTilt_Enemy = Quaternion.Euler(0f, 0f, 0f);
//            //Debug.Log("Player " + (player.IsMainPlayer ? "1" : "2") + " left extent: " + leftExtent + ", baseSpacing: " + baseSpacing);
//        }

//        public void UpdateCardPositions()
//        {
//            if (!initialized || hand.Cards.Count <= 0)
//                return;

//            //Calculations
//            GetLayoutStartingXAndSpacing(out float startingX, out float spacing);

//            for (int i = 0; i < hand.Cards.Count; i++)
//            {
//                Vector3 p = centerPos;
//                p.x = startingX + spacing * i;

//                hand.Cards[i].SetTargetPosition(p, true);

//                //Rotation
//                Vector3 dirToCamera = cardFacingTarget.position - p;
//                Quaternion rot = Quaternion.LookRotation(dirToCamera , Vector3.up) * ((isRealPlayer) ? cardTilt_RealPlayer : cardTilt_Enemy);
//                hand.Cards[i].SetTargetRotation(rot);
//            }
//        }
//        #endregion

//        #region Minor methods
//        void GetLayoutStartingXAndSpacing(out float startingX, out float spacing)
//        {
//            startingX = -TotalLayoutWidthOfCardss() / 2f;
//            //If there are so many cards that the starting position is beyond the outer limit, then recalculate
//            bool tooManyCard = (player.IsMainPlayer) ? startingX < leftExtent : startingX > leftExtent;

//            if (!tooManyCard)
//            {
//                //Debug.Log("Player " + (player.IsMainPlayer ? "1" : "2") + " NOT too many cards ");
//                spacing = baseSpacing;
//            }
//            else
//            {
//                //Debug.Log("Player " + (player.IsMainPlayer ? "1" : "2") + " too many cards ");
//                startingX = leftExtent;
//                spacing = (-leftExtent * 2) / hand.Cards.Count;
//            }
//        }

//        float TotalLayoutWidthOfCardss()
//        {
//            float normlSpacingCardsWidth = 0;
//            for (int i = 0; i < hand.Cards.Count - 1; i++)
//            {
//                normlSpacingCardsWidth += baseSpacing;
//            }
//            return normlSpacingCardsWidth;
//        }

//        float MouseOffset()
//        {
//            return 0f;
//        }
//        #endregion
//    }
//}
