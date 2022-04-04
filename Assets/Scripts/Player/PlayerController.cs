using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerUI
{
    public class PlayerController : MonoBehaviour
    {
        // Variables for movement
        private float speed = 3f;
        private float dashingspeed = 4.5f;

        // Variables for dashing
        private float ButtonCooler = 0.5f;
        private int ButtonCount = 0;
        private float dashingTime = 0.2f;

        // Movement State tracker
        private States movementState = States.NoMovement;
        private Vector2 currSpeed = Vector2.zero;

        private Rigidbody2D rigidInstance;

        enum States
        {
            NoMovement,
            Moving,
            Dashing
        }

        private void Start()
        {
            rigidInstance = GetComponent<Rigidbody2D>();   
        }

        // Update is called once per frame
        void Update()
        {
            MovePlayer();
            CheckDash();
        }

        private void CheckDash()
        {
            bool moving = (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W));
            if (moving)
            {

                if ((ButtonCooler > 0 && ButtonCount == 1) || movementState == States.Dashing)
                {
                    //Has double tapped
                    movementState = States.Dashing;
                    currSpeed = rigidInstance.velocity;
                    Debug.Log(movementState);
                }
                else
                {
                    ButtonCooler = 0.5f;
                    ButtonCount += 1;
                }
            }

            if (ButtonCooler > 0)
            {
                ButtonCooler -= 1 * Time.deltaTime;
            }
            else
            {
                ButtonCount = 0;
            }
            Dash();
        }

        private void Dash()
        {
            if (movementState == States.Dashing && dashingTime > 0)
            {
                rigidInstance.velocity = currSpeed * dashingspeed;
                dashingTime -= 1 * Time.deltaTime;
            }
            else if (movementState == States.Dashing && dashingTime < 0)
            {
                movementState = States.NoMovement;
                dashingTime = 0.2f;
                rigidInstance.velocity = new Vector2(0, 0);
                return;
            }
            else return;
        }

        private void MovePlayer()
        {
            if (movementState == States.Dashing)
            {
                Debug.Log("unlucky");
                return;
            }
            float x = Input.GetAxisRaw("Horizontal"); //returns -1 for 'a' and returns 1 for 'd'
            float y = Input.GetAxisRaw("Vertical"); //returns -1 for 'a' and returns 1 for 'd'

            float normaliser = Mathf.Sqrt(x * x + y * y);
            x = (x != 0) ? x / (normaliser) * speed : 0;
            y = (y != 0) ? y / (normaliser) * speed : 0;

            if (Mathf.Abs(x) != 0 || Mathf.Abs(y) != 0) movementState = States.Moving;
            else movementState = States.NoMovement;

            // To change where the characater is facing depending on input
            float newAngle = (x < 0) ? 180 : 0;
            transform.localEulerAngles = new Vector3(0, (newAngle), 0);

            rigidInstance.velocity = new Vector2(x, y);
        }
    }
}
