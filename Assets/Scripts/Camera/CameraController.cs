using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CameraPositionBookmark defaultView;
    public CameraPositionBookmark PlacementView;
    Camera camera;

    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

        }
    }
}