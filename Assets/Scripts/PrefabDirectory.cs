using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Units;

[DefaultExecutionOrder(-1000)]
public class PrefabDirectory : MonoBehaviour
{
    public static PrefabDirectory Instance;

    [Header("Card")]
    [SerializeField] Card GodzillaCard;
    [SerializeField] Card KingCard;
    [SerializeField] Card QueenCard;
    [SerializeField] Card JackCard;
    [SerializeField] Card KnightCard;
    [SerializeField] Card SwordsmanCard;
    [SerializeField] Card ArcherCard;
    [SerializeField] Card SpearmanCard;

    [Header("Unit pieces")]
    [SerializeField] PlayerUnit GodzillaPiece;
    [SerializeField] PlayerUnit KingPiece;
    [SerializeField] PlayerUnit QueenPiece;
    [SerializeField] PlayerUnit JackPiece;
    [SerializeField] PlayerUnit KnightPiece;
    [SerializeField] PlayerUnit SwordsmanPiece;
    [SerializeField] PlayerUnit ArcherPiece;
    [SerializeField] PlayerUnit SpearmanPiece;

    private Dictionary<CardTypes, Card> cardLookup;
    private Dictionary<CardTypes, PlayerUnit> pieceLookup;
    public UnitManager unit;

    public Card DrawCard(CardTypes cardType, Vector3 pos, Quaternion rotation)
    {
        //Debug.DrawLine(Vector3.zero, pos, Color.red, 10f);
        Card card = Instantiate(cardLookup[cardType], pos, rotation) as Card;
        return card;
    }

    public Card DrawRandomCard(Vector3 pos, Quaternion rotation, Transform parent)
    {
        Card card = Instantiate(RandomCard(), pos, rotation, parent) as Card;
        return card;
    }

    public PlayerUnit GetUnitPiece (CardTypes types)
    {
       return pieceLookup[types];
    }
    

    void Awake()
    {
        Instance = this;
        cardLookup = new Dictionary<CardTypes, Card>()
        {
            {CardTypes.Godzilla,    GodzillaCard },
            {CardTypes.King,        KingCard },
            {CardTypes.Queen,       QueenCard },
            {CardTypes.Jack,        JackCard },
            {CardTypes.Knight,      KnightCard },
            {CardTypes.Swordsman,   SwordsmanCard },
            {CardTypes.Archer,      ArcherCard },
            {CardTypes.Spearman,    SpearmanCard },
        };

        pieceLookup = new Dictionary<CardTypes, PlayerUnit>()
        {
            {CardTypes.Godzilla,    GodzillaPiece },
            {CardTypes.King,        KingPiece},
            {CardTypes.Queen,       QueenPiece},
            {CardTypes.Jack,        JackPiece},
            {CardTypes.Knight,      KnightPiece},
            {CardTypes.Swordsman,   SwordsmanPiece },
            {CardTypes.Archer,      ArcherPiece},
            {CardTypes.Spearman,    SpearmanPiece},
        };

    }

    //Minor methods
    Card RandomCard()
    {
        int index = Random.Range(0, cardLookup.Count);
        return cardLookup.ElementAt(index).Value;
    }

    // New hex void
}