using System.Collections;
using UnityEngine;

namespace TurnBasedGame.CardManagement
{
    public class CardContainer : MonoBehaviour
    {
        private Card card;
        
        public void Initialize (Card card)
        {
            this.card = card;
        }
    }
}