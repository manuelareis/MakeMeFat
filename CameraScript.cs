using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

    //cam sizes
    public static Camera cam;
    public static float camWidth;
    public static float camHeight;

    void Awake()
    {
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
    }
}