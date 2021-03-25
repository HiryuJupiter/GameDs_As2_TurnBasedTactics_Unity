using System.Collections;
using UnityEngine;

public class QuickTest_Coroutines : MonoBehaviour
{
    private void Start()
    {
        //StartCoroutine(A());
        StartCoroutine(B());
        //StartCoroutine(C());
    }


    IEnumerator A()
    {
        Debug.Log("Start A");
        yield return  C();
        Debug.Log("End A");
    }

    IEnumerator B()
    {
        Debug.Log("Start B");
        yield return StartCoroutine(C());
        Debug.Log("End B");
    }

    IEnumerator C()
    {
        Debug.Log("Start C");
        yield return new WaitForSeconds(1f);
        Debug.Log("End C");
    }
}