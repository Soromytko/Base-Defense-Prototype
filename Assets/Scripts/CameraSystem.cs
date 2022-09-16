using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Camera Camera { get; private set; }

    public void Pursue(Vector3 target)
    {
        transform.position = Vector3.Lerp(transform.position, target, Time.fixedDeltaTime);
    }

    private void Awake()
    {
        Camera = GetComponentInChildren<Camera>() ??
            throw new System.Exception("Camera is not founded");
    }

}
