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

    //Cache
    int handSize;
    float cardDrawInterval;
    private Vector3 panningOffset;

    public List<Card> Cards { get; private set; }
    public Vector3 PanningOffset => panningOffset;
    Deck deck => player.PlayerDeck;
    #endregion

    #region Public - Initialization
    public void Initialize(Player player)
    {
        //Initialize
        Cards = new List<Card>();

        //Reference
        this.player = player;
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
        panningOffset.y = -0f;
    }

    public void LowerHand()
    {
        panningOffset.y = -1f;
    }

    public void HideHand()
    {
        panningOffset.y = -3f;
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

    #region Public - Drawing and custom add card
    public void DrawHand() => StartCoroutine(DoDrawHand());
    IEnumerator DoDrawHand()
    {
        RaiseHand();

        while (Cards.Count < handSize)
        {
            if (deck.TryDrawCard(out Card card))
            {
                Cards.Add(card);
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

    public void AddCard(Card card)
    {
        Cards.Add(card);
        spreader.UpdateAllCardTransforms(false);
    }
    #endregion

    #region Public - card selection
    public bool TryRemoveCardFromHand(Card card) //A verbose name but makes it distinct from Deck's DrawCard method
    {
        if (Cards.Contains(card))
        {
            Cards.Remove(card);
            spreader.UpdateAllCardTransforms(false);
            return true;
        }
        else
        {
            Debug.LogError("Doesn't contain card!");
        }
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
    public void ExitHighlight()
    {
        foreach (var c in Cards)
        {
            c.ExitHighlight();
        }
    }
    #endregion
}
