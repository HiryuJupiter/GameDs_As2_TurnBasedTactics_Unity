using System.Collections;
using UnityEngine;

public class CardPositionManipulator
{
    #region Field and ctor
    //Ref
    Player player;
    PlayerHand hand;
    Card card;
    Transform transform;
    GameSettings settings;

    //Status
    Vector3 startLerpPos;
    Vector3 endLerpPos;
    float moveSpeed;
    float lerpT_move;

    Vector3 highlightOffset;

    public bool InDirectMove { get; private set; } = false;
    public bool InParabolicMove { get; private set; } = false;
    Vector3 finalPos => endLerpPos + highlightOffset + hand.PanningOffset;

    public CardPositionManipulator(Player player, Card card)
    {
        this.player = player;
        this.card = card;
        hand = player.Hand;
        transform = card.transform;
        settings = GameSettings.Instance;

        //Debug.DrawRay(player.PlayerHand.ParabolicDirRef.position, parabolicFwd, Color.cyan, 30f);
        //Debug.DrawRay(player.PlayerHand.ParabolicDirRef.position, parabolicUp, Color.red, 30f);
    }
    #endregion

    #region Public - set lerp target position
    public void SetTargetPosition(Vector3 targetPos, bool parabolicMove)
    {
        this.endLerpPos = targetPos;
        UpdatePosition(true, parabolicMove);
    }
    #endregion

    #region Public - set highlight position
    public void SetHighlightOffsetToSide(bool moveLeft)
    {
        highlightOffset = new Vector3(moveLeft ? -settings.HighlightOffsetX : settings.HighlightOffsetX, 0f, 0f);
        UpdatePosition(true, false);
    }

    public void EnterHighlight()
    {
        highlightOffset = new Vector3(0f, settings.HighlightOffsetY, -0.5f);
        UpdatePosition(true, false);
    }

    public void ExitHighlight()
    {
        highlightOffset = new Vector3(0f, 0f, 0f);
        UpdatePosition(true, false);
    }
    #endregion

    #region Update position
    void UpdatePosition(bool slowMove, bool parabolicMove)
    {
        lerpT_move = 0f;
        startLerpPos = transform.position;
        moveSpeed = settings.MoveLerpSpeed;
        //moveSpeed = slowMove ? settings.MoveLerpSpeed : settings.MoveLerpSpeed * 2f;

        if (parabolicMove && !InParabolicMove)
        {
            card.StartCoroutine(DoParabolicLerpPosition());
        }
        else if (!parabolicMove && !InDirectMove)
        {
            card.StartCoroutine(DoDirectLerpPosition());
        }
    }
    #endregion

    #region Direct lerp
    IEnumerator DoDirectLerpPosition()
    {
        InParabolicMove = false;
        InDirectMove = true;
        yield return null;
        while (lerpT_move < 1f && InDirectMove)
        {
            lerpT_move += Time.deltaTime * moveSpeed;
            if (lerpT_move > 1f)
                lerpT_move = 1f;

            //Smooth lerp
            float t = lerpT_move;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(startLerpPos, finalPos, t);
            yield return null;
        }
        yield return null;

        if (InDirectMove)
        {
            InDirectMove = false;
            transform.position = finalPos;
        }
    }
    #endregion

    #region Parabolic lerp
    IEnumerator DoParabolicLerpPosition()
    {
        InParabolicMove = true;
        InDirectMove = false;
        yield return null;

        while (lerpT_move < 1f)
        {
            lerpT_move += Time.deltaTime * moveSpeed;
            //Smooth lerp
            float t = lerpT_move;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.position = startLerpPos + ParabolicOffset(t);

            yield return null;
        }

        if (InParabolicMove)
        {
            InParabolicMove = false;
            transform.position = finalPos;
        }
    }

    Vector3 ParabolicOffset(float t)
    {
        Vector3 dir = finalPos - startLerpPos;
        float magnitude = dir.magnitude;

        //The math is based on {-x * x + x}, which is just an inverse parabola, 
        //...where y = 0 when x = 0 or 1, and y = height when x = 0.5
        float x = t;
        float y = settings.ParabolicHeight * (-x * x + x);

        //Scale it so that when x = t = 1, x offset is at the endPosition
        //We also want x to be on Z, because X axis is Vector3.right, and Z is transform.forward. 
        //To rotate this successfully using LookRotation, we want it to move in the forward direction
        Vector3 scaledParabolicPos = new Vector3(0f, y * magnitude, x * magnitude);

        Vector3 final = dir;
        try
        {
            final = Quaternion.LookRotation(dir, Vector3.up) * scaledParabolicPos; //Rotate a vector 90 degrees to the left
        }
        catch
        {
            Debug.LogError("Quaternion.LookRotation(dir, parabolicUp): " + Quaternion.LookRotation(dir, Vector3.up));
            Debug.LogError("scaledParabolicPos: " + scaledParabolicPos);
        }
        return final;
    }
    #endregion
}

/*
        IEnumerator DoLerpPosition()
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

        Vector3 ParabolicOffset(float t)
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