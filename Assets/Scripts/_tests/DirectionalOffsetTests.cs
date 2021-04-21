using System.Collections;
using UnityEngine;

public class DirectionalOffsetTests : MonoBehaviour
{
    Vector3 myPos => transform.position;

    private void OnDrawGizmosSelected()
    {
        Vector3 fwd = transform.forward;

        //These are perfectly right
        Debug.DrawRay(myPos, transform.forward, Color.blue);
        Debug.DrawRay(myPos, transform.right, Color.red);
        Debug.DrawRay(myPos, transform.up, Color.green);

        Vector3 up = Quaternion.Euler(0f, 0f, 90f) * fwd.normalized;

        Vector3 localForward = Quaternion.LookRotation(fwd, up) * Vector3.forward;
        Vector3 localRight = Quaternion.LookRotation(fwd, up) * Vector3.right;
        Vector3 localUp = Quaternion.LookRotation(fwd, up) * Vector3.up;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(myPos + localForward, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(myPos + localRight, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(myPos + localUp, 0.2f);
    }
}

//Try look at +++++++++++++++++++++++++++++++++++++++++
/*
 transform.LookAt(point);

transform.rotation = Quaternion.LookRotation(point - transform.position);

 transform.LookAt(target.transform.position, originalUp)
 */




/*
 * 
 * LookRotation doesn't work for rotating non-forward rotations.
private void OnDrawGizmosSelected()
{
    Vector3 fwd = transform.forward;

    //These are perfectly right
    Debug.DrawRay(myPos, transform.forward, Color.blue);
    Debug.DrawRay(myPos, transform.right, Color.red);
    Debug.DrawRay(myPos, transform.up, Color.green);

    Vector3 up = Quaternion.Euler(0f, 0f, 90f) * fwd.normalized;

    Vector3 localForward = Quaternion.LookRotation(fwd, up) * Vector3.forward;
    Vector3 localRight = Quaternion.LookRotation(fwd, up) * Vector3.right;
    Vector3 localUp = Quaternion.LookRotation(fwd, up) * Vector3.up;

    Gizmos.color = Color.blue;
    Gizmos.DrawSphere(myPos + localForward, 0.2f);
    Gizmos.color = Color.red;
    Gizmos.DrawSphere(myPos + localRight, 0.2f);
    Gizmos.color = Color.green;
    Gizmos.DrawSphere(myPos + localUp, 0.2f);
}




 */
