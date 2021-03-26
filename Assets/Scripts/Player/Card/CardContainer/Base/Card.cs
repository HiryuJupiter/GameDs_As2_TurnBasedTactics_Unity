using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.CardManagement
{
    public class Card : MonoBehaviour
    {
        private const float slowMoveSpeed = 1f;

        //Reference
        protected Player player;
        protected GamePhaseManager phaseManager;

        //Status
        private CardTypes cardType;
        private Vector3 targetPos;
        private Quaternion targetRot;
        private float lerpT_move;
        private float lerpT_rot;

        public CardTypes CardType => cardType;

        public void Initialize(Player player)
        {
            this.player = player;
            phaseManager = GamePhaseManager.Instance;
        }

        public void SetTargetPosition(Vector3 targetPos)
        {
            this.targetPos = targetPos;
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
                lerpT_move += Time.deltaTime * slowMoveSpeed;
                if (lerpT_move > 1f) lerpT_move = 1f;
                transform.position = Vector3.Lerp(transform.position, targetPos, lerpT_move);
                yield return null;
            }
            InMovingAnimation = false;
            transform.position = targetPos;
        }

        public void SetTargetRotation(Quaternion targetRot)
        {
            this.targetRot = targetRot;

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
                lerpT_rot += Time.deltaTime * slowMoveSpeed;
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpT_rot);
                yield return null;
            }
            InRotationAnimation = false;
            transform.rotation = targetRot;
        }

        #region Minor methods 
        private bool InMovingAnimation;
        private bool InRotationAnimation;

        public override string ToString()
        {
            return cardType.ToString();
        }
        #endregion
    }
}
