using System.Collections;
using UnityEngine;
using TurnBasedGame.PlayerManagement;

namespace TurnBasedGame.HandManagement
{
    public interface IHandSpreader
    {
        void Initilize(Player player);
        void UpdateCardPositions();
    }
}