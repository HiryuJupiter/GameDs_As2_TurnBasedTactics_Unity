using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.CardManagement
{
    public class Card : MonoBehaviour
    {
        private const float fastMoveSpeed = 10f;
        private const float slowMoveSpeed = 5f;

        //Reference
        private Player player;

        //Status
        private CardTypes cardType;
        private Vector3 targetPos;
        private Quaternion targetRot;
        private float lerpT_move;
        private float lerpT_rot;
        private float moveSpeedMod;

        public CardTypes CardType => cardType;

        public void Initialize(Player player)
        {
            this.player = player;
        }

        public void SetTargetPositional(Vector3 targetPos, bool isFastMove)
        {
            this.targetPos = targetPos;
            moveSpeedMod = isFastMove ? fastMoveSpeed : slowMoveSpeed;

            if (!InMovingAnimation)
            {
                StartCoroutine(LerpPosition());
            }
            else
            {
                lerpT_move = 0f;
            }
        }

        private IEnumerator LerpPosition()
        {
            lerpT_move = 0f;
            while (lerpT_move < 1f)
            {
                lerpT_move += Time.deltaTime * moveSpeedMod;
                transform.position = Vector3.Lerp(transform.position, targetPos, lerpT_move);
                yield return null;
            }
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
            while (lerpT_rot < 1f)
            {
                lerpT_rot += Time.deltaTime * moveSpeedMod;
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpT_rot);
                yield return null;
            }
            transform.rotation = targetRot;
        }

        #region Minor methods 
        private bool InMovingAnimation => lerpT_move > 0f;
        private bool InRotationAnimation => lerpT_rot > 0f;

        public override string ToString()
        {
            return cardType.ToString();
        }
        #endregion
    }
}
