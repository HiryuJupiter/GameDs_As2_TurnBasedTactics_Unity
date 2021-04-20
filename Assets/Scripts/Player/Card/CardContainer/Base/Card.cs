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
        protected CardPositionManipulator mover;
        protected CardRotationManipulator rotator;
        protected CardScaleManipulator scaler;

        //Cache
        private CardTypes cardType;

        public bool InMovingAnimation => mover.InMovingAnimation;
        public bool InRotationAnimation => rotator.InRotationAnimation;
        public bool InScalingAnimation => scaler.InScalingAnimation;
        public CardTypes CardType => cardType;
        bool CanHighlight => player.IsMainPlayer && GamePhaseManager.Phase == GamePhases.PlayingHand;
        #endregion

        #region Initialize
        public void Initialize(Player player)
        {
            this.player = player;
            phaseManager = GamePhaseManager.Instance;
            settings = CardSettings.Instance;

            mover = new CardPositionManipulator(player, this);
            rotator = new CardRotationManipulator(player, this);
            scaler = new CardScaleManipulator(player, this);
        }
        #endregion

        #region Movement
        public void SetTargetPosition(Vector3 targetPos) 
            => mover.SetTargetPosition(targetPos);

        public void SetTargetRotation(Quaternion targetRot, bool slowMove) 
            => rotator.SetTargetRotation(targetRot, slowMove);
        #endregion

        #region Private - transform manipulation

        //private void UpdateRotation(bool slowMove)
        //{
        //    rotLerpSpeed = slowMove ? settings.RotLerpSpeed : settings.RotLerpSpeed * 5f;
        //    startRot = transform.rotation;
        //    lerpT_rot = 0f;

        //    if (!inRotationAnimation)
        //        StartCoroutine(DoLerpRotation());
        //}

        //private IEnumerator DoLerpRotation()
        //{
        //    lerpT_rot = 0f;
        //    inRotationAnimation = true;

        //    while (lerpT_rot < 1f)
        //    {
        //        lerpT_rot += Time.deltaTime * rotLerpSpeed;

        //        float t = lerpT_rot;
        //        //t = t * t * (3f * 2f * t);
        //        t = Mathf.Sin(t * Mathf.PI * 0.5f);

        //        transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
        //        yield return null;
        //    }
        //    inRotationAnimation = false;
        //    transform.rotation = targetRot;
        //}

        //private void SetScale(Vector3 targetScale)
        //{
        //    this.targetScale = targetScale;
        //    if (!inScalingAnimation)
        //        StartCoroutine(LerpScale());
        //    else
        //        lerpT_scale = 0f;
        //}

        //private IEnumerator LerpScale()
        //{
        //    lerpT_scale = 0f;
        //    inScalingAnimation = true;
        //    while (lerpT_scale < 1f)
        //    {
        //        lerpT_scale += Time.deltaTime * scaleLerpSpeed;
        //        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpT_scale);
        //        yield return null;
        //    }
        //    inScalingAnimation = false;
        //    transform.localScale = targetScale;
        //}
        #endregion

        #region Public - Highlight
        public void HighlightOffsetMove(bool moveLeft)
        {
            mover.SetHighlightOffsetToSide(moveLeft);
            rotator.SetHighlightOffsetToSide(moveLeft);
            scaler.SetHighlightOffsetToSide(moveLeft);
        }

        public void EnterHighlight()
        {
            mover.EnterHighlight();
            rotator.EnterHighlight();
            scaler.EnterHighlight();
        }

        public void ExitHighlight()
        {
            mover.ExitHighlight();
            rotator.ExitHighlight();
            scaler.ExitHighlight();
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
        public override string ToString() => cardType.ToString();
        #endregion
    }
}

/*
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
                t = Mathf.Sin(t * Mathf.PI * 0.5f);

                transform.position = Vector3.Lerp(startPos, targetPos + highlightPosOffset, t);
                yield return null;
            }
            yield return null;

            inMovingAnimation = false;
            transform.position = targetPos + highlightPosOffset;
        }

 */