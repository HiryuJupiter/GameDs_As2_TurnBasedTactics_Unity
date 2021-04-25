using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public int p1Health = 100;
    public int p2Health = 100;

    UIManager uiM;

    public void ModifyP1Health (int amount)
    {
        p1Health += amount;
        uiM.SetP1HP(p1Health);
        if (p1Health <= 0)
        {
            Debug.Log("Game over");
        }
    }

    public void ModifyP2Health(int amount)
    {
        p2Health += amount;
        uiM.SetP2HP(p2Health);
        if (p2Health <= 0)
        {
            Debug.Log("Game won");
        }
    }

    public void RestartGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Start()
    {
        uiM = UIManager.Instance;
    }
}