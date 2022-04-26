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
            if (movementState == States.Dashing) // Should get rid of dashing and only make it a ninja logic
            {
                return;
            }
            float x, y;
            CalculateSpeeds(out x, out y);

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

            rigidInstance.velocity = new Vector2(x, y);
        }

        private void CalculateSpeeds(out float x, out float y)
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
            float normaliser = Mathf.Sqrt(x * x + y * y);
            x = (x != 0) ? x / (normaliser) * speed : 0;
            y = (y != 0) ? y / (normaliser) * speed : 0;
        }

        // Flip Movement Logic //////////////////////
        private void FlipMovement(float x)
        {
            if (isServer)
            {
                // This sends a command to all the clients (including the host) The own host's scene gets updated content
                FlipMovementClientRPC(x);
            }
            else if (isClient)
            {
                // This only sends it to the host so the client also has to update it's own local version
                FlipMovementLocal(x); // Local update
                FlipMovementCommand(x); // Server update
            }
        }

        private void FlipMovementLocal(float x)
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

        [ClientRpc]
        private void FlipMovementClientRPC(float x)
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

        ////////////////////////////////////////////
    }
}
