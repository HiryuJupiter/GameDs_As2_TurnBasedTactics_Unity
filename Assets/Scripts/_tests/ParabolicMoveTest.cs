using System.Collections;
using UnityEngine;

public class ParabolicMoveTest : MonoBehaviour
{
    public Transform p1, p2;
    const float TileMagnitude = 0.8f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            StartCoroutine(DoLerpPosition());
    }

    float moveLerpSpeed = 2f;
    IEnumerator DoLerpPosition()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * moveLerpSpeed;
            if (t > 1f)
                t = 1f;

            //Smooth lerp
            //t = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.position = p1.position + ParabolicOffset(t, Vector3.up, p1.position, p2.position);
            //transform.position = Vector3.Lerp(p1.position, p2.position, t) +
            //    ParabolicOffset(t);
            yield return null;
        }
        yield return null;
    }


    Vector3 ParabolicOffset(float t, Vector3 up, Vector3 pos1, Vector3 pos2)
    {
        Vector3 dir = pos2 - pos1;
        float magnitude = dir.magnitude;

        //The math is based on {-x * x + x}, which is just an inverse parabola, 
        //...where y = 0 when x = 0 or 1, and y = height when x = 0.5
        float x = t;
        float y = TileMagnitude * (-x * x + x);

        //Scale it so that when x = t = 1, x offset is at the endPosition
        //We also want x to be on Z, because X axis is Vector3.right, and Z is transform.forward. 
        //To rotate this successfully using LookRotation, we want it to move in the forward direction
        Vector3 scaledParabolicVector = new Vector3(0f, y * magnitude, x * magnitude);

        Debug.DrawRay(pos1, scaledParabolicVector, Color.grey, 10f);

        Debug.DrawRay(pos1, dir, Color.blue, 10f);

        Vector3 final = Quaternion.LookRotation(dir, Vector3.up) * scaledParabolicVector;
        Debug.DrawRay(pos1, final, Color.white, 10f);

        return final;
    }
}


/* Faild WIP
 
 */

/* 2D
     Vector3 ParabolicOffset(float t)
    {
        Vector3 dir = p2.position - p1.position;
        float magnitude = dir.magnitude;

        float x = t;
        // t = t * t * (3f - 2f * t);
        //-x * x + x is just an inverse parabola, where y = 0 when x = 0 or 1
        float y = TileMagnitude * (-x * x + x);

        //Scale it so that when x = t = 1, x offset is at the endPosition
        Vector3 scaledParabolicPos = new Vector3(x * magnitude, y * magnitude);
        Vector3 relativeUpDir = Quaternion.Euler(0f, 0f, 90f) * dir.normalized; //Rotate a vector 90 degrees to the left
        return Quaternion.LookRotation(Vector3.forward, relativeUpDir) * scaledParabolicPos;
    }
 */