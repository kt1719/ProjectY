using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerCore;

namespace PlayerCam
{
    public class CameraBounds : MonoBehaviour
    {
        private SceneDetails sceneDetail;
        private GameObject player;
        private Camera cam;

        private void Awake()
        {
            player = this.transform.root.gameObject;
            cam = this.GetComponent<Camera>();
        }

        private void Update()
        {
            KeepCameraInBounds();
        }

        private void KeepCameraInBounds()
        {
            if (player.GetComponent<PlayerController>().currScene == "")
            {
                return;
            }
            else if (sceneDetail == null)
            {
                UpdateCameraBounds();
            }
            Vector3 offset1 = this.cam.ScreenToWorldPoint(new Vector2(0, 0));
            Vector3 offset2 = this.cam.ScreenToWorldPoint(new Vector2((this.cam.pixelWidth / 2), (this.cam.pixelHeight / 2))) - offset1;
            float min_x = sceneDetail.GetTopLeftBound().x + offset2.x;
            float max_x = sceneDetail.GetBottomRightBounds().x - offset2.x;
            float min_y = sceneDetail.GetBottomRightBounds().y + offset2.y;
            float max_y = sceneDetail.GetTopLeftBound().y - offset2.y;
            float x_val = Mathf.Clamp(player.transform.position.x, min_x, max_x);
            float y_val = Mathf.Clamp(player.transform.position.y, min_y, max_y);
            this.transform.position = new Vector3(x_val, y_val, this.transform.position.z);
        }

        public void UpdateCameraBounds()
        {
            sceneDetail = GameObject.Find("/SceneDetails" + player.GetComponent<PlayerController>().currScene).GetComponent<SceneDetails>();
        }
    }
}