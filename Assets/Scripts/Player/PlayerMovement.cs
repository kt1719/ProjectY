using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerUI
{
    public class PlayerMovement : MonoBehaviour
    {
        // Variables for movement
        private float speed = 3f;
        private float dashingspeed = 4.5f;

        // Variables for dashing
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
        public void CheckDash()
        {
            // Have to check the condition for moving
            bool dash = (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && movementState != States.Dashing;

            if (dash && movementState == States.Moving)
            {
                movementState = States.Dashing;
                currSpeed = rigidInstance.velocity;
            }

            Dash();
        }

        public void Dash()
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

        public void MovePlayer()
        {
            if (movementState == States.Dashing)
            {
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
