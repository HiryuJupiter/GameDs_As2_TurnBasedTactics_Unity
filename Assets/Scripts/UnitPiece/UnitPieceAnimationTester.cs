﻿using System.Collections;
using UnityEngine;

public class UnitPieceAnimationTester : MonoBehaviour
{
    PlayerUnit unit;

    void Start()
    {
        unit = GetComponent<PlayerUnit>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            unit.PlayIdle();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            unit.PlayDead();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            unit.PlayMoving();
        if (Input.GetKeyDown(KeyCode.Alpha4))
            unit.PlayAttack();
    }
}