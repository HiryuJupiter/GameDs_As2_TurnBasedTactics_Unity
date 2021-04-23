using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
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
        return card;
    }

    public Card DrawRandomCard(Vector3 pos, Quaternion rotation, Transform parent)
    {
        Card card = Instantiate(RandomCard(), pos, rotation, parent) as Card;
        return card;
    }

    void Awake()
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

    Card RandomCard()
    {
        int index = Random.Range(0, lookup.Count);
        return lookup.ElementAt(index).Value;
    }
}