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

        private Vector3 parabolicUp;
        private Vector3 parabolicFwd;

        public bool InMovingAnimation { get; private set; }

        public CardPositionManipulator(Player player, Card card)
        {
            this.player = player;
            this.card = card;
            transform = card.transform;
            settings = CardSettings.Instance;

            //Cache
            parabolicUp = player.PlayerHand.ParabolicDirRef.up;
            parabolicFwd = player.PlayerHand.ParabolicDirRef.forward;

            //Debug.DrawRay(player.PlayerHand.ParabolicDirRef.position, parabolicFwd, Color.cyan, 30f);
            //Debug.DrawRay(player.PlayerHand.ParabolicDirRef.position, parabolicUp, Color.red, 30f);
        }
        #endregion

        //=== PUBLIC ===
        public void SetTargetPosition(Vector3 targetPos)
        {
            this.targetPos = targetPos;
            UpdatePosition(true);
        }


        public void SetHighlightOffsetToSide(bool moveLeft)
        {
            highlightPosOffset = new Vector3(moveLeft ? -settings.HighlightOffsetX : settings.HighlightOffsetX, 0f, 0f);
            UpdatePosition(false);
        }

        public void EnterHighlight()
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
            //Vector3 parabolicOffset = ParabolicOffset(1);
            //Debug.Log("Parabolic offset: " + parabolicUpDir + ". Startpos: " + startPos);
            //Debug.DrawLine(Vector3.zero, startPos);
            //transform.position = startPos + parabolicOffset;
            ////transform.position = startPos + parabolicOffset;
            //yield break;

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

                Vector3 offset = ParabolicOffset(t);
                //if (player.IsMainPlayer)
                //{
                //    Debug.DrawLine(Vector3.zero, offset, Color.yellow, 2f);
                //}
                transform.position = startPos + ParabolicOffset(t);
                //transform.position = startPos + ParabolicOffset(t);
                //transform.position = Vector3.Lerp(startPos, targetPos, t);
                //transform.position = Vector3.Lerp(startPos, targetPos + highlightPosOffset, t) +
                //    ParabolicOffset(t);
                yield return null;
            }
            yield return null;

            inMovingAnimation = false;
            transform.position = targetPos;
        }

        private Vector3 ParabolicOffset(float t)
        {
            Vector3 dir = (targetPos) - startPos;
            Debug.DrawLine(startPos, targetPos, Color.magenta, 2f);

            float magnitude = dir.magnitude;

            float x = t;
            // t = t * t * (3f - 2f * t);
            //-x * x + x is just an inverse parabola, where y = 0 when x = 0 or 1
            float y = settings.ParabolicHeight * (-x * x + x);

            //Scale it so that when x = t = 1, x offset is at the endPosition
            Vector3 scaledParabolicPos = new Vector3(x * magnitude, y * magnitude);
            //if (player.IsMainPlayer)
            //{
            //    Debug.DrawLine(Vector3.zero, scaledParabolicPos, Color.white, 2f);
            //}
            //Vector3 relativeUpDir = Quaternion.Euler(0f, 0f, 90f) * dir.normalized; //Rotate a vector 90 degrees to the left
            //return Quaternion.LookRotation(Vector3.forward, Vector3.up) * scaledParabolicPos;
            //return scaledParabolicPos;


            Vector3 tempLine = new Vector3(10, 0, 0);
            Debug.DrawRay(Vector3.zero, tempLine, Color.green, 5f);
            Vector3 rotated = Quaternion.LookRotation(parabolicFwd, parabolicUp) * scaledParabolicPos;
            //Debug.DrawRay(Vector3.zero, rotated, Color.red, 5f);


            return Quaternion.LookRotation(parabolicFwd, parabolicUp) * scaledParabolicPos;
        }
    }
}

/*
 
        private IEnumerator DoLerpPosition()
        {
            //Vector3 parabolicOffset = ParabolicOffset(1);
            //Debug.Log("Parabolic offset: " + parabolicUpDir + ". Startpos: " + startPos);
            //Debug.DrawLine(Vector3.zero, startPos);
            //transform.position = startPos + parabolicOffset;
            ////transform.position = startPos + parabolicOffset;
            //yield break;

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

                //transform.position = startPos + ParabolicOffset(t);
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
            Vector3 dir = (targetPos + highlightPosOffset) - startPos;
            float magnitude = dir.magnitude;

            float x = t;
            // t = t * t * (3f - 2f * t);
            //-x * x + x is just an inverse parabola, where y = 0 when x = 0 or 1
            float y = settings.ParabolicHeight * (-x * x + x);

            //Scale it so that when x = t = 1, x offset is at the endPosition
            Vector3 scaledParabolicPos = new Vector3(x * magnitude, y * magnitude);
            //Vector3 relativeUpDir = Quaternion.Euler(0f, 0f, 90f) * dir.normalized; //Rotate a vector 90 degrees to the left
            return Quaternion.LookRotation(Vector3.forward, parabolicUpDir) * scaledParabolicPos;
        }
 */