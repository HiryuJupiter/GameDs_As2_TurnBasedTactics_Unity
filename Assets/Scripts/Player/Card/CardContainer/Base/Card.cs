using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.CardManagement
{
    public class Card : MonoBehaviour
    {
        //Reference
        protected Player player;
        protected GamePhaseManager phaseManager;
        protected CardSettings settings;

        //Status
        private bool InMovingAnimation;
        private bool InRotationAnimation;
        private bool InScalingAnimation;
        private Vector3 targetPos;
        private Quaternion targetRot;
        private Vector3 targetScale;
        private float currentLerpMoveSpeed;
        private float lerpT_move;
        private float lerpT_rot;
        private float lerpT_scale;

        Vector3 highlightOffset;
        private Vector3 highlightScaleOffset;

        //Cache
        private CardTypes cardType;
        private float slowMoveLerpSpeed;
        private float fastMoveLerpSpeed;
        private float rotLerpSpeed;
        private float scaleLerpSpeed;
        private Vector3 startingScale;

        public CardTypes CardType => cardType;
        bool CanHighlight => player.IsMainPlayer && GamePhaseManager.Phase == GamePhases.PlayingHand;

        #region Movement
        public void Initialize(Player player)
        {
            this.player = player;
            phaseManager = GamePhaseManager.Instance;
            settings = CardSettings.Instance;

            //Cache
            slowMoveLerpSpeed = settings.MoveLerpSpeed;
            fastMoveLerpSpeed = slowMoveLerpSpeed * 10f;
            rotLerpSpeed = settings.RotLerpSpeed;
            scaleLerpSpeed = settings.ScaleLerpSpeed;
            startingScale = transform.localScale;
        }

        public void SetTargetPosition(Vector3 targetPos)
        {
            this.targetPos = targetPos;
            UpdatePosition();
        }

        public void SetTargetRotation(Quaternion targetRot)
        {
            this.targetRot = targetRot;
            UpdateRotation();
        }

        private void UpdatePosition(bool slowMove = true)
        {
            lerpT_move = 0f;
            currentLerpMoveSpeed = slowMove ? slowMoveLerpSpeed : fastMoveLerpSpeed;

            if (!InMovingAnimation)
            {
                StartCoroutine(LerpPosition());
            }
        }

        private IEnumerator LerpPosition()
        {
            InMovingAnimation = true;
            while (lerpT_move < 1f)
            {
                lerpT_move += Time.deltaTime * currentLerpMoveSpeed;
                if (lerpT_move > 1f)
                    lerpT_move = 1f;

                transform.position = Vector3.Lerp(transform.position, targetPos + highlightOffset, lerpT_move);
                yield return null;
            }
            InMovingAnimation = false;
            transform.position = targetPos;
        }

        private void UpdateRotation()
        {
            if (!InRotationAnimation)
            {
                StartCoroutine(LerpRotation());
            }
            else
            {
                lerpT_rot = 0f;
            }
        }

        private IEnumerator LerpRotation()
        {
            lerpT_rot = 0f;
            InRotationAnimation = true;
            while (lerpT_rot < 1f)
            {
                lerpT_rot += Time.deltaTime * rotLerpSpeed;
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpT_rot);
                yield return null;
            }
            InRotationAnimation = false;
            transform.rotation = targetRot;
        }

        private void SetScale(Vector3 targetScale)
        {
            this.targetScale = targetScale;
            if (!InScalingAnimation)
            {
                StartCoroutine(LerpScale());
            }
            else
            {
                lerpT_scale = 0f;
            }
        }

        private IEnumerator LerpScale()
        {
            lerpT_scale = 0f;
            InScalingAnimation = true;
            while (lerpT_scale < 1f)
            {
                lerpT_scale += Time.deltaTime * scaleLerpSpeed;
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpT_scale);
                yield return null;
            }
            InScalingAnimation = false;
            transform.localScale = targetScale;
        }
        #endregion

        #region Highlight
        private static Card HighlightedCard;

        public void HighlightOffsetMove(bool moveLeft)
        {
            highlightOffset = new Vector3(moveLeft ? -settings.HighlightOffsetX : settings.HighlightOffsetX, 0f, 0f);
            highlightScaleOffset = Vector3.zero;
            UpdatePosition(false);
            UpdateRotation();
            SetScale(startingScale);
        }

        public void EnterHighlight()
        {
            highlightOffset = new Vector3(0f, settings.HighlightOffsetY, -1f);
            highlightScaleOffset = settings.HighlightScale;
            UpdatePosition(false);
            UpdateRotation();
            SetScale(startingScale + highlightScaleOffset);
        }

        public void ExitHighlight()
        {
            highlightOffset = new Vector3(0f, 0f, 0f);
            highlightScaleOffset = Vector3.zero;
            UpdatePosition(false);
            UpdateRotation();
            SetScale(startingScale);
        }

        private void OnMouseEnter()
        {
            if (!CanHighlight) return;

            if (HighlightedCard != this)
            {
                HighlightedCard = this;
                player.PlayerHand.SetHighlightCard(this);
            }
        }

        private void OnMouseExit()
        {
            if (!CanHighlight) return;

            if (HighlightedCard == this)
            {
                HighlightedCard = null;
                player.PlayerHand.ExitHighlight();
            }
        }

        private void OnMouseDown()
        {
            if (!CanHighlight) return;

            player.PlayerHand.ExitHighlight();
            Debug.Log("clicked on card " + name);
            player.RemoveCard(this);
        }
        #endregion

        #region Minor methods 
        public override string ToString()
        {
            return cardType.ToString();
        }
        #endregion
    }
}
