using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.CardManagement
{
    public class Card : MonoBehaviour
    {
        private const float lerpSpeed = 1f;

        //Reference
        protected Player player;
        protected GamePhaseManager phaseManager;
        protected CardSettings settings;

        //Cache
        private CardTypes cardType;

        //Status
        private bool InMovingAnimation;
        private bool InRotationAnimation;
        private bool InScalingAnimation;
        private Vector3 targetPos;
        private Quaternion targetRot;
        private Vector3 targetScale;
        private float lerpT_move;
        private float lerpT_rot;
        private float lerpT_scale;

        private float highlightOffset_X;
        private float highlightOffset_Y;
        private float highlightOffset_Z;
        private Vector3 highlightScaleOffset;

        //Cache
        private Vector3 startingScale;


        public CardTypes CardType => cardType;
        bool CanHighlight => player.IsMainPlayer && GamePhaseManager.Phase == GamePhases.PlayingHand;

        public void Initialize(Player player)
        {
            this.player = player;
            phaseManager = GamePhaseManager.Instance;
            settings = CardSettings.Instance;

            startingScale = transform.localScale;
        }

        #region Movement
        public void SetTargetPosition(Vector3 targetPos)
        {
            this.targetPos = targetPos;
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            lerpT_move = 0f;

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
                lerpT_move += Time.deltaTime * lerpSpeed;
                if (lerpT_move > 1f)
                    lerpT_move = 1f;

                Vector3 highlightOffset = new Vector3(highlightOffset_X, highlightOffset_Y, highlightOffset_Z);
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos + highlightOffset, lerpT_move);
                yield return null;
            }
            InMovingAnimation = false;
            transform.localPosition = targetPos;
        }

        public void SetTargetRotation(Quaternion targetRot)
        {
            this.targetRot = targetRot;
            UpdateRotation();
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
                lerpT_rot += Time.deltaTime * lerpSpeed;
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
                lerpT_scale += Time.deltaTime * lerpSpeed;
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpT_scale);
                yield return null;
            }
            InScalingAnimation = false;
            transform.localScale = targetScale;
        }
        #endregion

        #region MonoBehaviour
        private static Card HighlightedCard;

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
                //Disable highlight mode
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

        #region Highlighting
        public void HighlightPartWay(bool isPartLeft)
        {
            highlightOffset_Y = 0f;
            highlightOffset_X = isPartLeft ? -settings.HighlightOffsetX : settings.HighlightOffsetX;
            highlightOffset_Z = 0f;
            highlightScaleOffset = Vector3.zero;
            UpdatePosition();
            UpdateRotation();
            SetScale(startingScale);
        }

        public void EnterHighlight()
        {
            highlightOffset_Y = settings.HighlightOffsetY;
            highlightOffset_X = 0f;
            highlightOffset_Z = -1f;
            highlightScaleOffset = settings.HighlightScale;
            UpdatePosition();
            UpdateRotation();
            SetScale(startingScale + highlightScaleOffset);
        }

        public void ExitHighlight()
        {
            highlightOffset_Y = 0f;
            highlightOffset_X = 0f;
            highlightOffset_Z = 0f;
            highlightScaleOffset = Vector3.zero;
            UpdatePosition();
            UpdateRotation();
            SetScale(startingScale);
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
