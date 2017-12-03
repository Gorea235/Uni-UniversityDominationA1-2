using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    //speed modifiers
    public float panSpeed = 15.0f;
    public float zoomSpeed = 4.0f;

    Vector3 hit_position;
    Vector3 current_position;
    Vector3 camera_position;
    float z = 0.0f;

    bool flag = false; //for checking if transition from position to needed destination is finished
    Vector3 target_position;

    void Update()
    {
        #region Zoom controls
        if (Input.GetAxis("Mouse ScrollWheel") > 0) { //zoom in

            if (transform.position.z+zoomSpeed < 0)
            {
                Vector3 move = zoomSpeed * transform.forward;
                transform.Translate(move, Space.World);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0){ //zoom out
            if (transform.position.z-zoomSpeed >= -18)
            {
                Vector3 move = zoomSpeed * (-transform.forward);
                transform.Translate(move, Space.World);
            }

        }
        #endregion

        #region Pan controls
        if (Input.GetMouseButtonDown(0))
        {
            hit_position = Input.mousePosition;
            camera_position = transform.position;
        }



        if (Input.GetMouseButton(0))
        {
            current_position = Input.mousePosition;
            LeftMouseDrag();
            flag = true;
        }

        if (flag)
        {
            transform.position = Vector3.MoveTowards(transform.position, target_position, Time.deltaTime * panSpeed);
            if (transform.position == target_position) //have we reached the target position?
            {
                flag = false; // stop moving
            }
        }
#endregion
    }
        #region Pan Helper function
    void LeftMouseDrag()
    {
        current_position.z = hit_position.z = camera_position.y; //hack to account for the fact that our map is 3D but we reference it as a 2D (i.e. we need only x and y)

        // Get direction of movement.
        Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);

        //transition camera
        target_position = camera_position + direction;
    }
#endregion
}