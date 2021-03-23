using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedGame.CardManagement
{
    public class CardDirectory : MonoBehaviour
    {
        public static CardDirectory Instance;


        [SerializeField] private CardContainer DummyCard;

        public CardContainer GetDummyCard ()
        {
            CardContainer card = Instantiate(DummyCard, transform.position, Quaternion.identity);
            return card;
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}
