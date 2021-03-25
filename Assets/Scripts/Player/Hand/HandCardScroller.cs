using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.CardManagement;

namespace TurnBasedGame.HandManagement
{
    [RequireComponent(typeof(Hand))]
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
        private void Awake()
        {
            //Reference
            hand = GetComponent<Hand>();

            //Cache
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
            if (hand.Cards.Count <= 0)
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

                        Vector3 dirToCamera = cameraTrans.position - p;
                        hand.Cards[i].SetTargetRotation(Quaternion.LookRotation(dirToCamera, Vector3.up));
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

                        Vector3 dirToCamera = cameraTrans.position - p;
                        hand.Cards[i].SetTargetRotation(Quaternion.LookRotation(dirToCamera, Vector3.up));
                    }
                }
            }
        }

        float HandTotalWidth ()
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
