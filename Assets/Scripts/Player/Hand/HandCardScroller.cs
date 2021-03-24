using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.CardManagement;

namespace TurnBasedGame.HandManagement
{
    public class HandCardScroller : MonoBehaviour
    {
        [SerializeField] private bool allowDebugInput = false;

        [Header("Positional references")]
        [SerializeField] Transform center;
        [SerializeField] Transform spawnPos;
        [SerializeField] Transform leftLimit;

        [Header("Settings")]
        [Tooltip("The standard spacing between cards (before they become too crowded)")]
        const float baseSpacing = 1f;
        [Tooltip("The speed that the cards move in reaction to mouse movement")]
        [SerializeField] float mouseMoveSpeed = 1f;


        CardDirectory cardDirectory;
        List<Card> cards = new List<Card>();

        //Cache
        Vector3 centerPos;
        Quaternion centerRot;
        float leftExtent; //Distance from middle to the left edge
        Transform cameraTrans;

        #region Mono
        private void Start()
        {
            cardDirectory = CardDirectory.Instance;
            cameraTrans = Camera.main.transform;

            //Cache calculations
            centerPos = center.position;
            leftExtent = center.position.x - leftLimit.position.x;
        }

        private void Update()
        {
            if (allowDebugInput)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    AddCard(spawnPos);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    RemoveCard();
                }
            }

            UpdateCardPositions();
        }
        #endregion

        #region Card addition/removal
        void AddCard(Transform spawn)
        {
            Card c = cardDirectory.DrawCard(CardTypes.Dummy, spawn.position, spawn.rotation);
            c.SetTargetRotation(center.rotation);
            cards.Add(c);
        }

        void RemoveCard()
        {
            if (cards.Count > 0)
            {
                Destroy(cards[cards.Count - 1].gameObject);
                cards.RemoveAt(cards.Count - 1);
            }
        }
        #endregion

        #region Positional update
        void UpdateCardPositions()
        {
            if (cards.Count <= 0)
                return;

            //Calculations
            float cardWidth = 0;
            for (int i = 0; i < cards.Count - 1; i++)
            {
                cardWidth += baseSpacing;
            }
            float startingPos = - cardWidth / 2f;

            // To do: Recalculate if there are too many cards
            //If (startingPos < extent; ...

            //Set card positions
            for (int i = 0; i < cards.Count; i++)
            {
                Vector3 p = centerPos;
                p.x = startingPos + baseSpacing * i;
                cards[i].SetTargetPositional(p, true);

                Vector3 dirToCamera = cameraTrans.position - p;
                cards[i].SetTargetRotation(Quaternion.LookRotation(dirToCamera, Vector3.up));
            }
        }

        float MouseOffset()
        {
            return 0f;
        }
        #endregion
    }
}
