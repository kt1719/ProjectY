using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PlayerUI
{
    public class PlayerMovement : NetworkBehaviour
    {
        // Variables for movement
        private float speed = 3f;
        private float dashingspeed = 4.5f;

        // Variables for dashing
        private float dashingTime = 0.2f;

        // Movement State tracker
        private States movementState = States.NoMovement;
        private Vector2 currSpeed = Vector2.zero;

        private PlayerAnimation animatorScript;

        private Rigidbody2D rigidInstance;

        private SpriteRenderer rendererInstance;

        enum States
        {
            NoMovement,
            Moving,
            Dashing
        }

        private void Start()
        {
            rigidInstance = GetComponent<Rigidbody2D>();
            animatorScript = GetComponent<PlayerAnimation>();
            rendererInstance = GetComponent<SpriteRenderer>();
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

            if (Mathf.Abs(x) != 0 || Mathf.Abs(y) != 0)
            {
                animatorScript.ChangeStateRunning();
                movementState = States.Moving;
            }
            else
            {
                animatorScript.ChangeStateNotRunning();
                movementState = States.NoMovement;
            }

            FlipMovement(x);
            FlipMovementCommand(x);

            rigidInstance.velocity = new Vector2(x, y);
        }

        private void FlipMovement(float x)
        {
            // To change where the characater is facing depending on input
            if (x != 0)
            {
                if (x < 0)
                {
                    rendererInstance.flipX = true;
                    this.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    rendererInstance.flipX = false;
                    this.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }
        }

        [Command]
        private void FlipMovementCommand(float x)
        {
            // To change where the characater is facing depending on input
            if (x != 0)
            {
                if (x < 0)
                {
                    rendererInstance.flipX = true;
                    this.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    rendererInstance.flipX = false;
                    this.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }
        }
    }
}
