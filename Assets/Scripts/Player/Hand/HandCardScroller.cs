using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.CardManagement;

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

        Hand hand;

        //Cache
        Vector3 centerPos;
        Quaternion centerRot;
        float leftExtent; //Distance from middle to the left edge
        Transform cameraTrans;

        #region Mono
        public void Initialize (Hand hand)
        {
            //Reference
            this.hand = hand;

            //Cache
            cameraTrans = Camera.main.transform;
            centerPos = center.position;
            leftExtent = center.position.x - leftLimit.position.x ;
            Debug.Log("left extent: " + leftExtent);
        }

        public void TickUpdate ()
        {
            UpdateCardPositions();
        }
       
        #endregion

        #region Positional update
        void UpdateCardPositions()
        {
            if (hand.Cards.Count <= 0)
                return;

            //Calculations
            float spacing = baseSpacing;
            float normlSpacingCardsWidth = 0;
            for (int i = 0; i < hand.Cards.Count - 1; i++)
            {
                normlSpacingCardsWidth += spacing;
            }
            float left = -normlSpacingCardsWidth / 2f;

            // Recalculate pacing if there are too many cards
            //If (startingPos < extent; ...
            //Debug.Log("startingPos " + startingPos);

            //Set card positions
            //If there is not too many cards, then place the cards ...
            //...starting at the left and apply regular spacing per card
            if (left >= leftExtent)
            {
                for (int i = 0; i < hand.Cards.Count; i++)
                {
                    Vector3 p = centerPos;
                    p.x = left + spacing * i;
                    hand.Cards[i].SetTargetPositional(p, true);

                    Vector3 dirToCamera = cameraTrans.position - p;
                    hand.Cards[i].SetTargetRotation(Quaternion.LookRotation(dirToCamera, Vector3.up));
                }
            }
            //If there are too many cards, then place the cards starting...
            //... at the left pos and apply the reduced spacing per card.
            else
            {
                spacing = (Mathf.Abs(leftExtent)* 2) / hand.Cards.Count;

                for (int i = 0; i < hand.Cards.Count; i++)
                {
                    Vector3 p = centerPos;
                    p.x = leftExtent + spacing * i;
                    hand.Cards[i].SetTargetPositional(p, true);

                    Vector3 dirToCamera = cameraTrans.position - p;
                    hand.Cards[i].SetTargetRotation(Quaternion.LookRotation(dirToCamera, Vector3.up));
                }
            }
        }

        float MouseOffset()
        {
            return 0f;
        }
        #endregion
    }
}
