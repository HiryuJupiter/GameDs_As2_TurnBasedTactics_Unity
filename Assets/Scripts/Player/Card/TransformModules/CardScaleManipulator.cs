using System.Collections;
using UnityEngine;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.CardManagement
{
    public class CardScaleManipulator
    {
        //Ref
        Player player;
        Card card;
        Transform transform;
        CardSettings settings;

        //Status
        private Vector3 targetScale;
        private float scaleLerpSpeed;
        private float lerpT_scale;

        //Cache
        private Vector3 startingScale;

        public bool InScalingAnimation { get; private set; }

        public CardScaleManipulator(Player player, Card card)
        {
            this.player = player;
            this.card = card;
            transform = card.transform;
            settings = CardSettings.Instance;

            //Cache
            scaleLerpSpeed = settings.ScaleLerpSpeed;
            startingScale = transform.localScale;
        }

        public void SetHighlightOffsetToSide(bool moveLeft)
        {
            SetScale(startingScale);
        }
        public void EnterHighlight()
        {
            SetScale(startingScale + settings.HighlightScale);
        }
        public void ExitHighlight()
        {
            SetScale(startingScale);
        }

        private void SetScale(Vector3 targetScale)
        {
            this.targetScale = targetScale;
            if (!InScalingAnimation)
                card.StartCoroutine(LerpScale());
            else
                lerpT_scale = 0f;
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
    }
}