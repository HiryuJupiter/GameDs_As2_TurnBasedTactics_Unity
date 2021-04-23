using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    [SerializeField] Transform discardLocation;
    Player player;
    PlayerHand hand;
    public List<Card> Cards { get; private set; }

    #region Public - Initialize
    public void Initialize(Player player)
    {
        //Initialize
        Cards = new List<Card>();

        //Reference
        this.player = player;
        hand = player.Hand;
    }
    #endregion

    public void AddToDiscardPile(Card card)
    {
        Cards.Add(card);
        card.SetTargetPosition(discardLocation.position + new Vector3(0f, Cards.Count * 0.01f), true);
        card.SetTargetRotation(discardLocation.rotation);
        card.transform.parent = transform;
    }
}