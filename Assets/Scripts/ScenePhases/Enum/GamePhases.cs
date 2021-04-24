using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GamePhases
{
    Standby,
    phase1_GameStartFillDeck,

    phase2_DrawCard,
    phase3_CardSelection, //Card highlight, selection
    phase4_Placement,
    phase5_UnitControl,  //Unit move, attack
    phase6_AIControlPhase,

    TurnCompleteEvaluation,
}