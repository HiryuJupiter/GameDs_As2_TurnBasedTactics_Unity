using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedGame.CardManagement
{
    public abstract class Card : MonoBehaviour
    {
        private int id;
        public int ID
        {
            get => id;
            set => id = value;
        }

        public Card(int id)
        {
            ID = id;
        }



        public override string ToString()
        {
            return id.ToString();
        }
    }
}