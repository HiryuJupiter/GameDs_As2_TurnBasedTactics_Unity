using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int MainGameLevelIndex = 1;
    public Transform title;
    Quaternion startRot;
    Vector3 startScale;

    void Start()
    {
        startRot = title.rotation;
        startScale = title.localScale;
    }

    void Update()
    {
        float s = Mathf.Sin(Time.time  * 3f) * 0.1f ;
        title.rotation = startRot * Quaternion.AngleAxis(s  * 100, Vector3.forward);
        title.localScale = startScale + new Vector3(s, s, s);
    }

    public void PressedStartGame ()
    {
        SceneManager.LoadScene(MainGameLevelIndex);
    }
}
