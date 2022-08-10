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

        public void setScreenRes(int W, int H)
        {
            Screen.SetResolution(W, H, FullScreenMode.ExclusiveFullScreen, 60);
        }
        private void Awake()
        {
            setScreenRes(640, 360);
        }

        private void LateUpdate()
        {
            setScreenRes(640, 360);
            Debug.Log("Screen res low");
        }
        // Need to subscribe to scenemanager active scene changed and change the component we're trying to find for bounding updates

    }
}