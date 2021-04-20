using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.CardManagement
{
    public class Card : MonoBehaviour
    {
        #region Fields
        private static Card HighlightedCard;

        //Reference
        protected Player player;
        protected GamePhaseManager phaseManager;
        protected CardSettings settings;

        //Status
        private bool inMovingAnimation;
        private bool inRotationAnimation;
        private bool inScalingAnimation;
        private Vector3 startPos;
        private Vector3 targetPos;
        private Quaternion startRot;
        private Quaternion targetRot;
        private Vector3 targetScale;
        private float moveLerpSpeed;
        private float lerpT_move;
        private float lerpT_rot;
        private float lerpT_scale;

        Vector3 highlightPosOffset;
        private Vector3 highlightScaleOffset;

        //Cache
        private CardTypes cardType;
        private float rotLerpSpeed;
        private float scaleLerpSpeed;
        private Vector3 startingScale;

        public bool InMovingAnimation => inMovingAnimation;
        public bool InRotationAnimation => inRotationAnimation;
        public bool InScalingAnimation => inScalingAnimation;
        public CardTypes CardType => cardType;
        bool CanHighlight => player.IsMainPlayer && GamePhaseManager.Phase == GamePhases.PlayingHand;

        //private void OnGUI()
        //{
        //    //GUI.Label(new Rect(20, 20, 200, 20), "HighlightedCard " + HighlightedCard.player.PlayerDeck);
        //    if (this == HighlightedCard)
        //    {
        //        GUI.Label(new Rect(20, 20, 200, 20), "HighlightedCard " + player.PlayerHand.Cards.IndexOf(HighlightedCard));
        //        GUI.Label(new Rect(20, 40, 200, 20), "InMovingAnimation " + inMovingAnimation);
        //        GUI.Label(new Rect(20, 60, 200, 20), "InScalingAnimation " + inScalingAnimation);

        //        GUI.Label(new Rect(20, 90, 200, 20), "lerpT_move " + lerpT_move);
        //        GUI.Label(new Rect(20, 110, 200, 20), "lerpT_scale " + lerpT_scale);
        //        GUI.Label(new Rect(20, 130, 200, 20), "lerpT_rot " + lerpT_rot);

        //        GUI.Label(new Rect(20, 160, 200, 20), "targetScale " + targetScale);
        //        GUI.Label(new Rect(20, 180, 200, 20), "highlightScaleOffset " + highlightScaleOffset);
        //    }
        //}
        #endregion

        #region Initialize
        public void Initialize(Player player)
        {
            this.player = player;
            phaseManager = GamePhaseManager.Instance;
            settings = CardSettings.Instance;

            //Cache
            scaleLerpSpeed = settings.ScaleLerpSpeed;
            startingScale = transform.localScale;
        }
        #endregion

        #region Movement
        public void SetTargetPosition(Vector3 targetPos)
        {
            this.targetPos = targetPos;
            UpdatePosition(true);
        }

        public void SetTargetRotation(Quaternion targetRot, bool slowMove)
        {

            this.targetRot = targetRot;
            UpdateRotation(slowMove);
        }
        #endregion

        #region Private - transform manipulation
        private void UpdatePosition(bool slowMove)
        {
            lerpT_move = 0f;
            startPos = transform.position;
            moveLerpSpeed = slowMove ? settings.MoveLerpSpeed : settings.MoveLerpSpeed * 10f;

            if (!inMovingAnimation)
                StartCoroutine(DoLerpPosition());
        }

        private IEnumerator DoLerpPosition()
        {
            inMovingAnimation = true;
            //while (true)
            while (lerpT_move < 1f)
            {
                lerpT_move += Time.deltaTime * moveLerpSpeed;
                if (lerpT_move > 1f)
                    lerpT_move = 1f;

                //Smooth lerp
                float t = lerpT_move;
                //t = t * t * (3f * 2f * t);
                t = Mathf.Sin(t * Mathf.PI * 0.5f);

                transform.position = Vector3.Lerp(startPos, targetPos + highlightPosOffset, t);
                yield return null;
            }
            yield return null;

            inMovingAnimation = false;
            transform.position = targetPos + highlightPosOffset;
        }

        private void UpdateRotation(bool slowMove)
        {
            rotLerpSpeed = slowMove ? settings.RotLerpSpeed : settings.RotLerpSpeed * 5f;
            startRot = transform.rotation;
            lerpT_rot = 0f;

            if (!inRotationAnimation)
                StartCoroutine(DoLerpRotation());
        }

        private IEnumerator DoLerpRotation()
        {
            lerpT_rot = 0f;
            inRotationAnimation = true;

            while (lerpT_rot < 1f)
            {
                lerpT_rot += Time.deltaTime * rotLerpSpeed;

                float t = lerpT_rot;
                //t = t * t * (3f * 2f * t);
                t = Mathf.Sin(t * Mathf.PI * 0.5f);

                transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
                yield return null;
            }
            inRotationAnimation = false;
            transform.rotation = targetRot;
        }

        private void SetScale(Vector3 targetScale)
        {
            this.targetScale = targetScale;
            if (!inScalingAnimation)
                StartCoroutine(LerpScale());
            else
                lerpT_scale = 0f;
        }

        private IEnumerator LerpScale()
        {
            lerpT_scale = 0f;
            inScalingAnimation = true;
            while (lerpT_scale < 1f)
            {
                lerpT_scale += Time.deltaTime * scaleLerpSpeed;
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpT_scale);
                yield return null;
            }
            inScalingAnimation = false;
            transform.localScale = targetScale;
        }
        #endregion

        #region Public - Highlight
        public void HighlightOffsetMove(bool moveLeft)
        {
            highlightPosOffset = new Vector3(moveLeft ? -settings.HighlightOffsetX : settings.HighlightOffsetX, 0f, 0f);
            highlightScaleOffset = Vector3.zero;
            UpdatePosition(false);
            UpdateRotation(false);
            SetScale(startingScale);
        }

        public void EnterHighlight()
        {
            highlightPosOffset = new Vector3(0f, settings.HighlightOffsetY, -0.5f);
            highlightScaleOffset = settings.HighlightScale;
            UpdatePosition(false);
            UpdateRotation(false);
            SetScale(startingScale + highlightScaleOffset);
        }

        public void ExitHighlight()
        {
            highlightPosOffset = new Vector3(0f, 0f, 0f);
            highlightScaleOffset = Vector3.zero;
            UpdatePosition(false);
            UpdateRotation(false);
            SetScale(startingScale);
        }
        #endregion

        #region Detect mouse enter and exit
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
