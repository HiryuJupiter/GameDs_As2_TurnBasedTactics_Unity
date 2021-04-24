using System.Collections;
using UnityEngine;

public class SelectionSlot : MonoBehaviour
{
    [SerializeField] Transform slotReference;

    GamePhaseManager gamePhaseManager;
    Player player;
    Card card;

    public Vector3 SelectionSlotPos => slotReference.position;
    public Card Card => card;

    public void Initialize(Player player)
    {
        this.player = player;
        gamePhaseManager = GamePhaseManager.Instance;
    }

    public void SetAsSelectedCard(Card card)
    {
        if (card == null)
        {
            Debug.LogError("No card! ");
            return;
        }
        this.card = card;
        card.SetTargetPosition(slotReference.position, false);
        card.SetTargetRotation(slotReference.rotation);
    }

    public bool TryRemoveCard(out Card card)
    {
        if (this.card != null)
        {
            card = this.card;
            this.card = null;
            return true;
        }
        card = null;
        return false;
    }
}