using System.Collections;
using UnityEngine;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.CardManagement
{
    public class CardPositionManipulator
    {
        #region Field and ctor
        //Ref
        Player player;
        Card card;
        Transform transform;
        CardSettings settings;

        //Status
        private bool inMovingAnimation;
        private Vector3 startPos;
        private Vector3 targetPos;
        private float moveLerpSpeed;
        private float lerpT_move;

        Vector3 highlightPosOffset;

        private Vector3 parabolicUpDir;

        public bool InMovingAnimation { get; private set; }

        public CardPositionManipulator(Player player, Card card)
        {
            this.player = player;
            this.card = card;
            transform = card.transform;
            settings = CardSettings.Instance;

            //Cache
            parabolicUpDir = player.PlayerHand.ParabolicUpDirection;
        }
        #endregion

        //=== PUBLIC ===
        public void SetTargetPosition(Vector3 targetPos)
        {
            this.targetPos = targetPos;
            UpdatePosition(true);
        }


        public void SetHighlightOffsetToSide (bool moveLeft)
        {
            highlightPosOffset = new Vector3(moveLeft ? -settings.HighlightOffsetX : settings.HighlightOffsetX, 0f, 0f);
            UpdatePosition(false); 
        }

        public void EnterHighlight ()
        {
            highlightPosOffset = new Vector3(0f, settings.HighlightOffsetY, -0.5f);
            UpdatePosition(false);
        }

        public void ExitHighlight()
        {
            highlightPosOffset = new Vector3(0f, 0f, 0f);
            UpdatePosition(false);
        }

        //=== PRIVATE ===
        private void UpdatePosition(bool slowMove)
        {
            lerpT_move = 0f;
            startPos = transform.position;
            moveLerpSpeed = slowMove ? settings.MoveLerpSpeed : settings.MoveLerpSpeed * 10f;

            if (!inMovingAnimation)
                card.StartCoroutine(DoLerpPosition());
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
                t = Mathf.Sin(t * Mathf.PI * 0.5f);

                transform.position = Vector3.Lerp(startPos, targetPos + highlightPosOffset, t);
                //transform.position = Vector3.Lerp(startPos, targetPos + highlightPosOffset, t) +
                //    ParabolicOffset(t);
                yield return null;
            }
            yield return null;

            inMovingAnimation = false;
            transform.position = targetPos + highlightPosOffset;
        }

        private Vector3 ParabolicOffset(float t)
        {
            Vector3 dir = targetPos - startPos;
            float magnitude = dir.magnitude;

            float x = t;
            // t = t * t * (3f - 2f * t);
            //-x * x + x is just an inverse parabola, where y = 0 when x = 0 or 1
            float y = settings.ParabolicHeight * (-x * x + x);

            //Scale it so that when x = t = 1, x offset is at the endPosition
            Vector3 scaledParabolicPos = new Vector3(x * magnitude, y * magnitude);
            Vector3 relativeUpDir = Quaternion.Euler(0f, 0f, 90f) * dir.normalized; //Rotate a vector 90 degrees to the left
            return Quaternion.LookRotation(Vector3.forward, relativeUpDir) * scaledParabolicPos;
        }

    }
}