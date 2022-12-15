using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    private Transform player;
    private Camera mainCamera;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        CameraMoving();
        CameraZooming();
    }

    void CameraMoving()
    {
        Vector3 temp = transform.position;
        temp.x = player.position.x;
        temp.y = player.position.y;
        transform.position = temp;
    }

    void CameraZooming()
    {
        if(mainCamera.orthographicSize <= 5 && mainCamera.orthographicSize >= 3)
        {
            
            if (Input.mouseScrollDelta.y != 0)
            {
                mainCamera.orthographicSize += (Input.mouseScrollDelta.y * -1) * 0.2f; 
            }

            if ( mainCamera.orthographicSize < 3 )    mainCamera.orthographicSize = 3;
            if ( mainCamera.orthographicSize > 5 )    mainCamera.orthographicSize = 5;
        }

    }

}
