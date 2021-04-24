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

    public bool IsMainPlayer => player.IsMainPlayer;
    public bool IsHandcard { get; private set; }
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

    #region Public - set hand
    public void SetIsHandcard(bool isTrue) => IsHandcard = isTrue;
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
        scaler.ExitHighlight();
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

    public void SetPanningX(float x) => mover.SetPanningX(x);
    public void SetPanningY(float y) => mover.SetPanningY(y);
    public void ExitPanning() => mover.ClearPanning();
    #endregion
}