using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    #region Fields
    [Header("Positional references")]
    [SerializeField] Transform centuralCardPosition;
    [SerializeField] Transform leftLimit;
    [SerializeField] Transform facingTarget; //The target that the cards are facing

    Player player;
    HandSpreader spreader;
    GameState gameState;

    //Cache
    int handSize;
    float cardDrawInterval;

    public List<Card> Cards { get; private set; }
    Deck deck => player.PlayerDeck;
    #endregion

    #region Public - Initialization
    public void Initialize(Player player)
    {
        //Initialize
        Cards = new List<Card>();

        //Reference
        this.player = player;
        gameState = GameState.Instance;
        spreader = new HandSpreader(player, centuralCardPosition, leftLimit, facingTarget.position);

        //Cache
        handSize = GameSettings.Instance.HandSize;
        cardDrawInterval = GameSettings.Instance.CardDrawInterval;
    }
    #endregion

    #region Public - Refresh
    public void RefreshHandCardPositions ()
    {
        spreader.UpdateAllCardTransforms(false);
    }
    #endregion

    #region Public - Panning
    public void RaiseHand()
    {
        if (!player.IsMainPlayer)
            return;
        foreach (Card card in Cards)
        {
            card.SetPanningY(0f);
        }
    }

    public void HideHand()
    {
        foreach (Card card in Cards)
        {
            card.SetPanningY(-5f);
        }
    }

    public void PanningUpdate()
    {
        //Vector2 delta = (Vector2)Input.mousePosition - screenCenter;
        //mousePanOffset = Camera.main.ScreenToWorldPoint(mouse);
        //panningOffset.x = mousePanOffset.x * setting.MousePanSensitivity * 100;
    }
    #endregion

    #region Public - status check
    public bool AreCardsStill()
    {
        foreach (var card in Cards)
        {
            if (card.InMovingAnimation)
                return false;
        }
        return true;
    }
    #endregion

    #region Public - Adding and removing card
    public void DrawHand() => StartCoroutine(DoDrawHand());
    IEnumerator DoDrawHand()
    {
        RaiseHand();

        while (Cards.Count < handSize)
        {
            if (deck.TryDrawCard(out Card card))
            {
                AddCard(card, false);
            }
            else
                break; //No more cards in deck
        }

        for (int i = 0; i < Cards.Count; i++)
        {
            spreader.UpdateSingleCardTransform(i, true);
            yield return new WaitForSeconds(cardDrawInterval);
        }
    }

    public void CustomDrawCard ()
    {
        if (deck.TryDrawCard(out Card card))
        {
            AddCard(card, true);
        }
    }

    public void AddCard(Card card, bool updatePos = true)
    {
        Cards.Add(card);
        card.SetIsHandcard(true);

        if (updatePos)
            spreader.UpdateAllCardTransforms(false);
    }

    public bool TryRemoveCardFromHand(Card card) 
    {
        if (Cards.Contains(card))
        {
            Cards.Remove(card);
            card.ExitHighlight();
            card.ExitPanning();
            card.SetIsHandcard(false);
            spreader.UpdateAllCardTransforms(false);
            return true;
        }
        Debug.LogError("Doesn't contain card!");
        return false;
    }
    #endregion

    #region Public - card highlight
    public void SetHighlightCard(Card card)
    {
        if (Cards.Contains(card))
        {
            int index = Cards.IndexOf(card);
            for (int i = 0; i < Cards.Count; i++)
            {
                if (i < index)
                {
                    Cards[i].HighlightOffsetMove(true);

                }
                else if (i == index)
                {
                    Cards[i].EnterHighlight();
                }
                else
                {
                    Cards[i].HighlightOffsetMove(false);
                }
            }
        }
    }
    public void ExitHighlightOnAllCards()
    {
        foreach (var c in Cards)
        {
            c.ExitHighlight();
        }
    }
    #endregion
}
