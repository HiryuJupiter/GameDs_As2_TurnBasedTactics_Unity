using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GamePhases
{
    Standby,
    p1_GameStartFillDeck,

    p2_DrawCard,
    p3_PlayerControl, 
    p4_AIControlPhase,

    TurnCompleteEvaluation,
}