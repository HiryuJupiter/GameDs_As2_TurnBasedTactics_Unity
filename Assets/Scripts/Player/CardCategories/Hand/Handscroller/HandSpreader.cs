using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 Things to keep in mind: player 1 have their card running from left to right    
 the opponent, Player 2, needs to have their card running from right to left

ZRotation: negative number is clockwise, positive number is anticlockwise
 */

public class HandSpreader
{
    //Status
    int cardCount;
    float startXPos;
    float zRotationStart;
    float zRotationOffset;
    float spacing;
    float startXPos_Squared;
    float maxVerticalOffset;

    //Cache
    readonly PlayerHand hand;
    readonly bool isRealPlayer;

    readonly float leftExtent; //Distance from the middle to the left edge
    readonly float sign;
    readonly float baseSpacing; //X position spacing
    readonly Vector3 centerPos;
    readonly Vector3 facingPos;

    readonly float baseZRotationStart;
    readonly float baseZRotationOffset;
    readonly Quaternion baseYRot;
    readonly float baseVerticalOffset; //Y position offset

    //Properties
    float TotalLayoutWidth => baseSpacing * (cardCount - 1);

    #region Constructor
    public HandSpreader(Player player, Transform centuralCardReference, Transform leftLimit, Vector3 facingPos)
    {
        //Reference
        hand = player.Hand;
        GameSettings setting = GameSettings.Instance;

        //Cache
        isRealPlayer = player.IsMainPlayer;

        leftExtent = leftLimit.position.x;
        sign = Mathf.Sign(leftExtent);
        baseSpacing = sign * -setting.spacing;
        centerPos = centuralCardReference.position;
        this.facingPos = facingPos;

        baseZRotationStart = sign * setting.ZRotationStart;
        baseZRotationOffset = -sign * setting.ZRotationOffset;
        baseYRot = Quaternion.Euler(0f, isRealPlayer ? -15f : -5f, 0f);
        baseVerticalOffset = setting.VerticalOffset;
    }
    #endregion

    #region Public hooks - update card positions
    public void UpdateAllCardTransforms(bool parabolicMove)
    {
        if (!HasCardsInHand) //Guard
            return;

        cardCount = hand.Cards.Count;
        CalculateLayoutParameters();
        for (int i = 0; i < hand.Cards.Count; i++)
        {
            UpdateCardTransform(i, parabolicMove);
        }
    }

    public void UpdateSingleCardTransform(int index, bool parabolicMove)
    {
        cardCount = hand.Cards.Count;
        CalculateLayoutParameters();
        UpdateCardTransform(index, parabolicMove);
    }
    #endregion

    #region Private 
    void CalculateLayoutParameters()
    {
        if (!LayoutBeyondExtent(startXPos))
        {
            //When there are not too many cards, naturally fan them out
            startXPos = -TotalLayoutWidth / 2f;
            zRotationStart = -baseZRotationOffset * (cardCount - 1) / 2f;
            zRotationOffset = baseZRotationOffset;
            spacing = baseSpacing;
        }
        else
        {
            //When there are too many cards, then cram them by dividing...
            //the max allowed rotaiton and spacing by the card count.
            startXPos = leftExtent;
            zRotationStart = baseZRotationStart;
            zRotationOffset = -baseZRotationStart * 2f / cardCount;
            spacing = (-leftExtent * 2f) / cardCount;
        }

        startXPos_Squared = startXPos * startXPos;
        maxVerticalOffset = baseVerticalOffset * cardCount;
    }

    void UpdateCardTransform(int index, bool parabolicMove)
    {
        Card card = hand.Cards[index];

        //Get preliminary starting position based on index
        Vector3 pos = centerPos;
        pos.x = startXPos + spacing * index;

        //Set rotation
        Quaternion rot = RotationTowardsCamera(pos) * baseYRot * zRot(index);
        card.SetTargetRotation(rot);

        //Vertical positional offset based on rotation
        float distToCenterSquared = pos.x * pos.x; //The central x-position is 0, so pos.x is naturally the distance.
        float perc = (startXPos == 0) ? 1 : 1 - (distToCenterSquared / startXPos_Squared);
        pos = pos + rot.normalized * new Vector3(0f, maxVerticalOffset * perc, 0f);

        card.SetTargetPosition(pos, parabolicMove);
    }
    #endregion

    #region Minor methods
    Quaternion zRot(int index) => Quaternion.Euler(0f, 0f, zRotationStart + (index * zRotationOffset)); //Z-axis rotation of card
    Quaternion RotationTowardsCamera(Vector3 pos) => Quaternion.LookRotation((facingPos - pos), Vector3.up); //Rotation towards the facing object.

    bool HasCardsInHand => hand.Cards.Count > 0;

    bool LayoutBeyondExtent(float startXPos) => isRealPlayer ? (startXPos < leftExtent) : (startXPos > leftExtent);
    #endregion
}