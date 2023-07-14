using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVCamera : MonoBehaviour
{
    [SerializeField]
    Camera camera;

    void Start()
    {
        camera.targetTexture.width = Screen.width;
        camera.targetTexture.height = Screen.height;
    }

    void Update()
    {
        transform.position = Camera.main.transform.position;
    }
}
