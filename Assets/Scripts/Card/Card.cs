using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    #region Fields
    //Reference
    protected Player player;
    protected GamePhaseManager phaseManager;
    protected GameSettings settings;
    protected CardPositionManipulator mover;
    protected CardRotationManipulator rotator;
    protected CardScaleManipulator scaler;

    //Cache
    CardTypes cardType;

    public bool InMovingAnimation => mover.InDirectMove || mover.InParabolicMove;
    public bool InRotationAnimation => rotator.InRotationAnimation;
    public bool InScalingAnimation => scaler.InScalingAnimation;
    public CardTypes CardType => cardType;
    #endregion

    #region Initialize
    public void Initialize(Player player)
    {
        this.player = player;
        phaseManager = GamePhaseManager.Instance;
        settings = GameSettings.Instance;

        mover = new CardPositionManipulator(player, this);
        rotator = new CardRotationManipulator(player, this);
        scaler = new CardScaleManipulator(player, this);
    }
    #endregion

    #region Movement
    public void SetTargetPosition(Vector3 targetPos, bool parabolicMove)
        => mover.SetTargetPosition(targetPos, parabolicMove);

    public void SetTargetRotation(Quaternion targetRot)
        => rotator.SetTargetRotation(targetRot);
    #endregion

    #region Public - Highlight
    public void HighlightOffsetMove(bool moveLeft)
    {
        mover.SetHighlightOffsetToSide(moveLeft);
        scaler.SetHighlightOffsetToSide(moveLeft);
    }

    public void EnterHighlight()
    {
        mover.EnterHighlight();
        scaler.EnterHighlight();
    }

    public void ExitHighlight()
    {
        mover.ExitHighlight();
        scaler.ExitHighlight();
    }
    #endregion

    #region Detect mouse enter and exit
    void OnMouseEnter() => player.MouseEnterCard(this);

    void OnMouseExit() => player.MouseExitsCard(this);

    void OnMouseDown() => player.ClickedOnCard(this);
    #endregion

    #region Minor methods 
    public override string ToString() => cardType.ToString();
    #endregion
}

/*
         IEnumerator DoLerpPosition()
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