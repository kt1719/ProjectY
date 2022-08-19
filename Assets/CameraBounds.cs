using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerCam
{
    public class CameraBounds : MonoBehaviour
    {
        private SceneDetails sceneDetail;
        private GameObject player;
        private Camera cam;
        private CustomPixelPerfectScript customPixelPerfectScript;
        private Vector2 currentTransform;

        private void Awake()
        {
            sceneDetail = GameObject.Find("/SceneDetails").GetComponent<SceneDetails>();
            player = this.transform.parent.gameObject;
            cam = this.GetComponent<Camera>();
            customPixelPerfectScript = this.GetComponent<CustomPixelPerfectScript>();
        }

        private void Update()
        {
            Vector3 offset1 = this.cam.ScreenToWorldPoint(new Vector2(0, 0));
            Vector3 offset2 = this.cam.ScreenToWorldPoint(new Vector2((this.cam.pixelWidth/2), (this.cam.pixelHeight / 2))) - offset1;
            float min_x = sceneDetail.GetTopLeftBound().x + offset2.x;
            float max_x = sceneDetail.GetBottomRightBounds().x - offset2.x;
            float min_y = sceneDetail.GetBottomRightBounds().y + offset2.y;
            float max_y = sceneDetail.GetTopLeftBound().y - offset2.y;
            float x_val = Mathf.Clamp(player.transform.position.x, min_x, max_x);
            float y_val = Mathf.Clamp(player.transform.position.y, min_y, max_y);
            Debug.Log("Offset " + offset2);
            Debug.Log("min x " + min_x);
            Debug.Log("max x " + max_x);
            this.transform.position = new Vector3(x_val, y_val, this.transform.position.z);
        }

        // Need to subscribe to scenemanager active scene changed and change the component we're trying to find for bounding updates

    }
}