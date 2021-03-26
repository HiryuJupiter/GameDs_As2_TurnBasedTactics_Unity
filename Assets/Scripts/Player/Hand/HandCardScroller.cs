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
     */



    public class HandCardScroller : MonoBehaviour
    {
        //The standard spacing between cards (before they become too crowded, before Mathf.Sign is calculated
        const float BaseSpacingRaw = 1f;

        [Header("Positional references")]
        [SerializeField] Transform center;
        [SerializeField] Transform spawnPos;
        [SerializeField] Transform leftLimit;

        [Header("Settings")]
        [Tooltip("The speed that the cards move in reaction to mouse movement")]
        [SerializeField] float mouseMoveSpeed = 1f;

        Player player;
        Hand hand;

        //Status
        bool initialized;
        bool isRealPlayer;

        //Cache
        float baseSpacing;
        Vector3 centerPos;
        Quaternion centerRot;
        float leftExtent; //Distance from middle to the left edge
        Transform cameraTrans;

        #region Mono
        public void Initilize(Player player)
        {
            initialized = true;

            //Reference
            this.player = player;
            hand = player.Hand;

            //Cache
            isRealPlayer = player.IsMainPlayer;
            cameraTrans = Camera.main.transform;
            centerPos = center.position;
            leftExtent = -(center.position.x - leftLimit.position.x);
            baseSpacing = Mathf.Sign(leftExtent) * -BaseSpacingRaw;
            Debug.Log("Player " + (player.IsMainPlayer ? "1" : "2") + " left extent: " + leftExtent + ", baseSpacing: " + baseSpacing);
        }

        private void Update()
        {
            UpdateCardPositions();
        }
        #endregion

        #region Positional update
        void UpdateCardPositions()
        {
            if (!initialized || hand.Cards.Count <= 0)
                return;

            //Calculations
            GetLayoutStartingXAndSpacing(out float startingX, out float spacing);
            Debug.Log("Player " + (player.IsMainPlayer ? "1" : "2") + " startingPos: " + startingX +
                " spacing: " + spacing);

            for (int i = 0; i < hand.Cards.Count; i++)
            {
                if (hand.Cards[i] != null)
                {
                    Vector3 p = centerPos;
                    p.x = startingX + spacing * i;

                    hand.Cards[i].SetTargetPositional(p, true);
                    SetCardToFaceCamera(hand.Cards[i], p);
                }
            }
        }
        #endregion

        #region Minor methods
        void SetCardToFaceCamera(Card c, Vector3 cardPos)
        {
            if (isRealPlayer)
            {
                Vector3 dirToCamera = cameraTrans.position - cardPos;
                c.SetTargetRotation(Quaternion.LookRotation(dirToCamera, Vector3.up));
            }
        }

        void GetLayoutStartingXAndSpacing(out float startingX, out float spacing)
        {
            startingX = -TotalLayoutWidthOfCardss() / 2f;
            //If there are so many cards that the starting position is beyond the outer limit, then recalculate
            bool tooManyCard = (player.IsMainPlayer) ? startingX < leftExtent : startingX > leftExtent;

            if (!tooManyCard)
            {
                Debug.Log("Player " + (player.IsMainPlayer ? "1" : "2") + " NOT too many cards ");
                spacing = baseSpacing;
            }
            else
            {
                Debug.Log("Player " + (player.IsMainPlayer ? "1" : "2") + " too many cards ");
                startingX = leftExtent;
                spacing = (-leftExtent * 2) / hand.Cards.Count;
            }
        }

        float TotalLayoutWidthOfCardss()
        {
            float normlSpacingCardsWidth = 0;
            for (int i = 0; i < hand.Cards.Count - 1; i++)
            {
                normlSpacingCardsWidth += baseSpacing;
            }
            return normlSpacingCardsWidth;
        }

        float MouseOffset()
        {
            return 0f;
        }
        #endregion
    }
}
