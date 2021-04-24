﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CancelPlacementButton : MonoBehaviour, IPointerClickHandler
{
    GamePhaseManager phaseManager;

    private void Start()
    {
        phaseManager = GamePhaseManager.Instance;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        phaseManager.Player1.CancelPlacement();
    }

}