using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TurnBasedGame.CameraControl
{
    public class CameraPositionBookmark : MonoBehaviour
    {
        [SerializeField] private float fieldOfView;

        public void SetCameraToThisLocation(Camera camera)
        {
            camera.fieldOfView = fieldOfView;
            camera.transform.position = transform.position;
            camera.transform.rotation = transform.rotation;

            //To do: smooth move.
        }
    }
}

