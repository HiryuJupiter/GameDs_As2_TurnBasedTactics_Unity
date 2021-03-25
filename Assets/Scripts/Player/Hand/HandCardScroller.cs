using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.CardManagement;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.HandManagement
{
    public class HandCardScroller : MonoBehaviour
    {
        [Header("Positional references")]
        [SerializeField] Transform center;
        [SerializeField] Transform spawnPos;
        [SerializeField] Transform leftLimit;

        [Header("Settings")]
        [Tooltip("The standard spacing between cards (before they become too crowded)")]
        const float baseSpacing = 1f;
        [Tooltip("The speed that the cards move in reaction to mouse movement")]
        [SerializeField] float mouseMoveSpeed = 1f;

        Player player;
        Hand hand;

        //Status
        bool initialized;
        bool isRealPlayer;

        //Cache
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
            Debug.Log("left extent: " + leftExtent);
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
            float startingPos = -HandTotalWidth() / 2f;
            
            //If there is not too many cards, then place the cards ...
            //...starting at the left and apply regular spacing per card
            if (startingPos >= leftExtent)
            {
                for (int i = 0; i < hand.Cards.Count; i++)
                {
                    if (hand.Cards[i] != null)
                    {
                        Vector3 p = centerPos;
                        p.x = startingPos + baseSpacing * i;

                        hand.Cards[i].SetTargetPositional(p, true);
                        SetCardToFaceCamera(hand.Cards[i], p);
                    }
                }
            }
            //If there are too many cards, then place the cards starting...
            //... at the left pos and apply the reduced spacing per card.
            else
            {
                float spacing = (Mathf.Abs(leftExtent) * 2) / hand.Cards.Count;

                for (int i = 0; i < hand.Cards.Count; i++)
                {
                    if (hand.Cards[i] != null)
                    {
                        Vector3 p = centerPos;
                        p.x = leftExtent + spacing * i;

                        hand.Cards[i].SetTargetPositional(p, true);
                        SetCardToFaceCamera(hand.Cards[i], p);
                    }
                }
            }
        }

        #endregion

        #region Minor methods

        void SetCardToFaceCamera (Card c, Vector3 cardPos)
        {
            if (isRealPlayer)
            {
                Vector3 dirToCamera = cameraTrans.position - cardPos;
                c.SetTargetRotation(Quaternion.LookRotation(dirToCamera, Vector3.up));
            }
        }

        float HandTotalWidth()
        {
            float spacing = baseSpacing;
            float normlSpacingCardsWidth = 0;
            for (int i = 0; i < hand.Cards.Count - 1; i++)
            {
                normlSpacingCardsWidth += spacing;
            }
            Debug.Log("Hand total width = " + normlSpacingCardsWidth);
            return normlSpacingCardsWidth;
        }

        float MouseOffset()
        {
            return 0f;
        }


        #endregion
    }
}
