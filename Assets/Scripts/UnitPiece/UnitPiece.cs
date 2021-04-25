using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPiece : MonoBehaviour
{
    #region Fields
    [SerializeField] Renderer renderer;
    [SerializeField] SpriteRenderer allianceRing;
    [SerializeField] SpriteRenderer selectionDot;
    [SerializeField]  Animator animator;

    //Ref
    Material material;

    //Status
    Vector3 startPos;
    Vector3 targetPos;

    //Cache
    Color defaultColor;

    public bool isMoving { get; private set; }
    public bool IsMainPlayer { get; private set; }
    public Player player { get; private set; }
    public Vector2Int TileIndex { get; private set; }
    #endregion

    #region Spawn initialization
    public void SpawnInitialization(Player player, Vector2Int tileIndex)
    {
        //Ref
        material = renderer.material;
        defaultColor = material.color;

        //Cache
        this.player = player;
        IsMainPlayer = player.IsMainPlayer;

        //Status
        TileIndex = tileIndex;

        //Intiailize
        allianceRing.color = IsMainPlayer ? Color.green : Color.red;
        selectionDot.color = allianceRing.color;
        allianceRing.enabled = true;
        transform.rotation = Quaternion.LookRotation(
            IsMainPlayer ? Vector3.forward : -Vector3.forward, Vector3.up);
    }
    #endregion

    #region Animator
    public void PlayIdle()
    {
        animator.Play("Idle");
    }
    public void PlayDead()
    {
        animator.Play("Dead");
    }

    public void PlayMoving()
    {
        animator.Play("Moving");
    }

    public void PlayAttack()
    {
        animator.Play("Attack");
    }
    #endregion

    #region Highlight
    public void ToggleSelectionHighlight(bool isOn)
    {
        selectionDot.enabled = isOn;
    }

    public void TogglehoverHighlight (bool isOn)
    {
        material.color = isOn ? Color.white : defaultColor;
    }
    #endregion

    #region Moving
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
    #endregion
}