using System.Collections;
using UnityEngine;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.CardManagement
{
    public class CardRotationManipulator
    {
        //Ref
        Player player;
        Card card;
        Transform transform;
        CardSettings settings;

        //Status
        private Quaternion startRot;
        private Quaternion targetRot;
        private float rotLerpSpeed;
        private float lerpT_rot;

        public bool InRotationAnimation { get; private set; }

        public CardRotationManipulator(Player player, Card card)
        {
            this.player = player;
            this.card = card;
            transform = card.transform;
            settings = CardSettings.Instance;
        }

        public void SetTargetRotation(Quaternion targetRot, bool slowMove)
        {
            this.targetRot = targetRot;
            UpdateRotation(slowMove);
        }

        public void SetHighlightOffsetToSide(bool moveLeft)
        {
            UpdateRotation(false);
        }
        public void EnterHighlight()
        {
            UpdateRotation(false);
        }
        public void ExitHighlight()
        {
            UpdateRotation(false);
        }

        private void UpdateRotation(bool slowMove)
        {
            rotLerpSpeed = slowMove ? settings.RotLerpSpeed : settings.RotLerpSpeed * 5f;
            startRot = transform.rotation;
            lerpT_rot = 0f;

            if (!InRotationAnimation)
                card.StartCoroutine(DoLerpRotation());
        }

        private IEnumerator DoLerpRotation()
        {
            lerpT_rot = 0f;
            InRotationAnimation = true;

            while (lerpT_rot < 1f)
            {
                lerpT_rot += Time.deltaTime * rotLerpSpeed;

                float t = lerpT_rot;
                //t = t * t * (3f * 2f * t);
                t = Mathf.Sin(t * Mathf.PI * 0.5f);

                transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
                yield return null;
            }
            InRotationAnimation = false;
            transform.rotation = targetRot;
        }

    }
}