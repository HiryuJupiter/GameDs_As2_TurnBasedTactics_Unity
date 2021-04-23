using System.Collections;
using UnityEngine;

public class CardScaleManipulator
{
    //Ref
    Player player;
    Card card;
    Transform transform;
    GameSettings settings;

    //Status
    Vector3 targetScale;
    float scaleLerpSpeed;
    float lerpT_scale;

    //Cache
    Vector3 startingScale;

    public bool InScalingAnimation { get; private set; }

    public CardScaleManipulator(Player player, Card card)
    {
        this.player = player;
        this.card = card;
        transform = card.transform;
        settings = GameSettings.Instance;

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

    void SetScale(Vector3 targetScale)
    {
        this.targetScale = targetScale;
        if (!InScalingAnimation)
            card.StartCoroutine(LerpScale());
        else
            lerpT_scale = 0f;
    }

    IEnumerator LerpScale()
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