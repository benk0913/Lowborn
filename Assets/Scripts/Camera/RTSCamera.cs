using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCamera : MonoBehaviour
{

    public static RTSCamera Instance;

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

    [SerializeField]
    Terrain BordersTerrain;
    Vector3 TerrainCenter;

    bool OverBorderRight
    {
        get
        {
            return (transform.position.x > TerrainCenter.x + BordersTerrain.terrainData.size.x / 2);
        }
    }
    bool OverBorderLeft
    {
        get
        {
            return (transform.position.x < TerrainCenter.x - BordersTerrain.terrainData.size.x / 2);
        }
    }
    bool OverBorderTop
    {
        get
        {
            return (transform.position.z > TerrainCenter.z + BordersTerrain.terrainData.size.z / 2);
        }
    }
    bool OverBorderBottom
    {
        get
        {
            return (transform.position.x < TerrainCenter.z - BordersTerrain.terrainData.size.z / 2);
        }
    }

    private void Awake()
    {
        Instance = this;

        TerrainCenter = new Vector3(BordersTerrain.transform.position.x + BordersTerrain.terrainData.size.x / 2, 0, BordersTerrain.transform.position.z + BordersTerrain.terrainData.size.z / 2);
    }

    private void Update()
    {
        Debug.LogError("RIGHT :"+OverBorderRight + " | LEFT: " + OverBorderLeft + " | TOP: " + OverBorderTop + " | BOTTOM" + OverBorderBottom);

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || (MouseScroll && Input.mousePosition.y > (Screen.height - 1f))) && !OverBorderTop)
        {
            transform.position += transform.TransformDirection(0, 0, ScrollSpeed) * Time.unscaledDeltaTime;
        }
        else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || (MouseScroll && Input.mousePosition.y < 1f)) && !OverBorderBottom)
        {
            transform.position += transform.TransformDirection(0, 0, -ScrollSpeed) * Time.unscaledDeltaTime;
        }

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || (MouseScroll && Input.mousePosition.x < 1f)) && !OverBorderLeft)
        {
            transform.position += transform.TransformDirection(-ScrollSpeed, 0, 0) * Time.unscaledDeltaTime;
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || (MouseScroll && Input.mousePosition.x > Screen.width - 1f)) && !OverBorderRight)
        {
            transform.position += transform.TransformDirection(ScrollSpeed, 0, 0) * Time.unscaledDeltaTime;
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0f && Camera.main.orthographicSize > maxZoom)
        {
            Camera.main.orthographicSize -= ZoomSpeed * Time.unscaledDeltaTime;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Camera.main.orthographicSize < minZoom)
        {
            Camera.main.orthographicSize += ZoomSpeed * Time.unscaledDeltaTime;
        }

        //if(Input.GetKeyDown(KeyCode.Tab))
        //{
        //    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x
        //        ,transform.rotation.eulerAngles.y+90
        //        ,transform.rotation.eulerAngles.z);
        //}
    }
}
