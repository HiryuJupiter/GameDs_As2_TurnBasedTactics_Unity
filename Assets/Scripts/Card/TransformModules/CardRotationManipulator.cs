using System.Collections;
using UnityEngine;


public class CardRotationManipulator
{
    //Ref
    Player player;
    Card card;
    Transform transform;
    GameSettings settings;

    //Status
    private Quaternion startRot;
    private Quaternion targetRot;
    float rotLerpSpeed;
    float lerpT_rot;

    public bool InRotationAnimation { get; private set; }

    public CardRotationManipulator(Player player, Card card)
    {
        this.player = player;
        this.card = card;
        transform = card.transform;
        settings = GameSettings.Instance;

        rotLerpSpeed = settings.RotLerpSpeed;
    }

    public void SetTargetRotation(Quaternion targetRot)
    {
        this.targetRot = targetRot;

        startRot = transform.rotation;
        lerpT_rot = 0f;

        if (!InRotationAnimation)
            card.StartCoroutine(DoLerpRotation());
    }

    IEnumerator DoLerpRotation()
    {
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