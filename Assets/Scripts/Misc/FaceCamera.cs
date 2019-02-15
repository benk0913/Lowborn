using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(RTSCamera.Instance.transform);
    }
}
