using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCamera : MonoBehaviour
{
    [SerializeField]
    float ScrollSpeed = 1f;

    [SerializeField]
    float ZoomSpeed = 1f;

    [SerializeField]
    float minZoom = 20f;

    [SerializeField]
    float maxZoom = 5f;

    [SerializeField]
    bool MouseScroll = true;

    private void Update()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || (MouseScroll && Input.mousePosition.y > (Screen.height - 1f)))
        {
            transform.position += transform.TransformDirection(0, ScrollSpeed, 0) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || (MouseScroll && Input.mousePosition.y < 1f))
        {
            transform.position += transform.TransformDirection(0, -ScrollSpeed, 0) * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || (MouseScroll && Input.mousePosition.x < 1f))
        {
            transform.position += transform.TransformDirection(-ScrollSpeed, 0, 0) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || (MouseScroll && Input.mousePosition.x > Screen.width - 1f))
        {
            transform.position += transform.TransformDirection(ScrollSpeed, 0, 0) * Time.deltaTime;
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0f && Camera.main.orthographicSize > maxZoom)
        {
            Camera.main.orthographicSize -= ZoomSpeed * Time.deltaTime;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Camera.main.orthographicSize < minZoom)
        {
            Camera.main.orthographicSize += ZoomSpeed * Time.deltaTime;
        }

        //if(Input.GetKeyDown(KeyCode.Tab))
        //{
        //    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x
        //        ,transform.rotation.eulerAngles.y+90
        //        ,transform.rotation.eulerAngles.z);
        //}
    }
}
