using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.CardManagement
{
    public class Card : MonoBehaviour
    {
        private static Card HighlightedCard;

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

        private void OnGUI()
        {
            //GUI.Label(new Rect(20, 20, 200, 20), "HighlightedCard " + HighlightedCard.player.PlayerDeck);
            if (this == HighlightedCard)
            {
                GUI.Label(new Rect(20, 20, 200, 20), "HighlightedCard " + player.PlayerHand.Cards.IndexOf(HighlightedCard));
                GUI.Label(new Rect(20, 40, 200, 20), "InMovingAnimation " + InMovingAnimation);
                GUI.Label(new Rect(20, 60, 200, 20), "InScalingAnimation " + InScalingAnimation);

                GUI.Label(new Rect(20, 90, 200, 20), "lerpT_move " + lerpT_move);
                GUI.Label(new Rect(20, 110, 200, 20), "lerpT_scale " + lerpT_scale);
                GUI.Label(new Rect(20, 130, 200, 20), "lerpT_rot " + lerpT_rot);

                GUI.Label(new Rect(20, 160, 200, 20), "targetScale " + targetScale);
                GUI.Label(new Rect(20, 180, 200, 20), "highlightScaleOffset " + highlightScaleOffset);
            }
        }

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
            //while (true)
            while (lerpT_move < 1f)
            {
                lerpT_move += Time.deltaTime * currentLerpMoveSpeed;
                if (lerpT_move > 1f)
                    lerpT_move = 1f;

                transform.position = Vector3.Lerp(transform.position, targetPos + highlightOffset, lerpT_move);
                yield return null;
            }
            yield return null;

            InMovingAnimation = false;
            transform.position = targetPos + highlightOffset;
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
            Debug.Log("Setscale " + targetScale);
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
            highlightOffset = new Vector3(0f, settings.HighlightOffsetY, -0.5f);
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
