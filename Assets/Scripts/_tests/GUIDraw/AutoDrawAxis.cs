using System.Collections;
using UnityEngine;

namespace Assets.Scripts._tests.GUIDraw
{
    public class AutoDrawAxis : MonoBehaviour
    {
        public bool isOn = false;
        void OnDrawGizmosSelected()
        {
            if (!isOn)
                return;

            Debug.DrawRay(transform.position, transform.forward * 0.5f, Color.blue);
            Debug.DrawRay(transform.position, transform.right * 0.5f, Color.red);
            Debug.DrawRay(transform.position, transform.up * 0.5f, Color.green);
        }
    }
}