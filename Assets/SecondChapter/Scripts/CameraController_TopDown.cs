using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController_TopDown : MonoBehaviour
{
    public Transform follow;
    public float boundX = 0.15f;
    public float boundY = 0.05f;

    private void LateUpdate()
    {
        Vector3 frameDelta = Vector3.zero;
        
        // To check if player is inside camera bounds
        float deltaX = follow.position.x - transform.position.x;
        float deltaY = follow.position.y - transform.position.y;

        // Camera X bounds
        if (deltaX > boundX || deltaX < -boundX) { 
            if (transform.position.x < follow.position.x) {
                frameDelta.x = deltaX - boundX;
            }
            else {
                frameDelta.x = deltaX + boundX;
            }
        }

        // Camera Y bounds
        if (deltaY > boundY || deltaY < -boundY) {
            if (transform.position.y < follow.position.y) {
                frameDelta.y = deltaY - boundY;
            }
            else {
                frameDelta.y = deltaY + boundY;
            }
        }

        transform.position += new Vector3(frameDelta.x, frameDelta.y, 0);
    }
}
