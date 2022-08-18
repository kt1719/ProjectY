using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

namespace PlayerCam
{
    public class CameraBounds : MonoBehaviour
    {
        SceneDetails sceneDetail;
        Transform player;
        Camera cam;

        Vector3 bottomRightCameraCoord;
        Vector3 topLeftCameraCoord;

        
        // Need to subscribe to scenemanager active scene changed and change the component we're trying to find for bounding updates

    }
}