using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    UIManager uiM;
    Player player;

    int health = 100;
    int gold = 200;
    int score = 0;
    int cardCost = 100;

    public bool HasMoneyForCard => gold >= 100;

    private void Awake()
    {
        Instance = this;
        player = Player.Instance;
    }

    void Start()
    {
        uiM = UIManager.Instance;
        uiM.SetGold(gold);
        uiM.SetP1HP(health);
        uiM.SetScore(score);
    }
    public void ModifyHealth (int amount)
    {
        health += amount;
        uiM.SetP1HP(health);
        if (health <= 0)
        {
            uiM.GameOver();
        }
    }

    public bool TryBuyCard ()
    {
        if (gold >= 100)
        {
            gold -= 100;
            uiM.SetGold(gold);
            return true;
        }
        return false;
    }

    public void EarnGold (int amount)
    {
        gold += amount;
        uiM.SetGold(gold);

        Player.Instance.CheckIfCanDisplayBuyCardButton();
    }

    public void RestartGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

  
}