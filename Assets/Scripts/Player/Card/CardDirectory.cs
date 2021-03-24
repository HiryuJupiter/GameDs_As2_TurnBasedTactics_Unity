using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedGame.CardManagement
{
    public class CardDirectory : MonoBehaviour
    {
        public static CardDirectory Instance;

        [SerializeField] private Card Dummy;
        [SerializeField] private Card King;
        [SerializeField] private Card Queen;
        [SerializeField] private Card Jack;
        [SerializeField] private Card Knight;
        [SerializeField] private Card Swordsman;
        [SerializeField] private Card Archer;
        [SerializeField] private Card Spearman;

        private Dictionary<CardTypes, Card> lookup;

        public Card DrawCard(CardTypes cardType, Vector3 pos, Quaternion rotation)
        {
            //Debug.DrawLine(Vector3.zero, pos, Color.red, 10f);
            Card card = Instantiate(lookup[cardType], pos, rotation) as Card;
            card.Initialize();
            return card;
        }

        private void Awake()
        {
            Instance = this;
            lookup = new Dictionary<CardTypes, Card>()
            {
                {CardTypes.Dummy,       Dummy },
                {CardTypes.King,        King },
                {CardTypes.Queen,       Queen },
                {CardTypes.Jack,        Jack },
                {CardTypes.Knight,      Knight },
                {CardTypes.Swordsman,   Swordsman },
                {CardTypes.Archer,      Archer },
                {CardTypes.Spearman,    Spearman },
            };
        }
    }
}