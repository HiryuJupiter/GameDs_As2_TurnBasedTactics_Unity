using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPiece : MonoBehaviour
{
    Vector3 startPos;
    Vector3 targetPos;

    public bool isMoving { get; private set; }
    public bool IsMainPlayer { get; private set; }
    public Player player { get; private set; }

    public void Spawn(Player player)
    {
        //Cache
        this.player = player;
        IsMainPlayer = player.IsMainPlayer;
        transform.rotation = Quaternion.LookRotation(
            IsMainPlayer ? Vector3.forward : -Vector3.forward, Vector3.up);
    }

    public void MoveToTarget(Vector3 pos, Action callback)
    {
        if (!isMoving)
        {
            startPos = transform.position;
            targetPos = pos;
            StartCoroutine(DoDirectLerpPosition(callback));
        }
    }

    IEnumerator DoDirectLerpPosition(Action callback)
    {
        isMoving = true;
        Vector3 prev;

        for (float t = 0; t < 1f; t += Time.deltaTime * 5f)
        {
            prev = transform.position;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.LookRotation(transform.position - prev, Vector3.up);
            yield return null;
        }

        isMoving = false;
        transform.position = targetPos;
        callback();
    }
}
