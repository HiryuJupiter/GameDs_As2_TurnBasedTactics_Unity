using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    #region Fields
    [SerializeField] Transform spawnLocation;
    [SerializeField] Transform stationaryLocation;

    Player player;
    PlayerHand hand;
    CardDirectory cardDir;

    public List<Card> Cards { get; private set; }
    #endregion

    #region Public - Initialize
    public void Initialize(Player player)
    {
        //Initialize
        Cards = new List<Card>();

        //Reference
        cardDir = CardDirectory.Instance;
        this.player = player;
        hand = player.Hand;
    }
    #endregion

    #region Public - Fill deck, Draw card from deck, discard pile
    public void Fill() => StartCoroutine(DoFill());

    public bool TryDrawCard(out Card card)
    {
        card = null;
        if (Cards.Count > 0f)
        {
            card = Cards[Cards.Count - 1];
            Cards.RemoveAt(Cards.Count - 1);
            card.transform.parent = hand.transform;
            return true;
        }
        return false;
    }
    #endregion

    #region Public - status check
    public bool AreCardsInDeckStill()
    {
        foreach (var card in Cards)
        {
            if (card.InMovingAnimation)
                return false;
        }
        return true;
    }
    #endregion

    #region Private 
    IEnumerator DoFill()
    {
        float interval = GameSettings.Instance.DeckSpawnInterval;
        int deckSize = GameSettings.Instance.DeckSize;

        for (int i = 0; i < deckSize; i++)
        {
            Card c = cardDir.DrawRandomCard(
                stationaryLocation.position + new Vector3(0f, i * 0.01f, 0f),
                spawnLocation.rotation,
                transform);
            c.Initialize(player);
            //c.SetTargetPosition(stationaryLocation.position + new Vector3(0f, i * 0.01f, 0f), false);
            c.SetTargetRotation(stationaryLocation.rotation);
            Cards.Add(c);
            yield return new WaitForSeconds(interval);
        }
    }
    #endregion
}