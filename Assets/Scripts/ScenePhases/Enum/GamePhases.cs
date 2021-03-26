using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedGame
{
    public enum GamePhases
    {
        GameStartFillDeck,
        DrawCard, //Drawing cards from deck
        PlayingHand, //Playing cards from hand
        UnitMoving, //Moving pieces
        UnitFighting,
        TurnCompleteEvaluation,
    }
}
