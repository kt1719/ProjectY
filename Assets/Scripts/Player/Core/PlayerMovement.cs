using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using PlayerAnim;

namespace PlayerCore
{
    public class PlayerMovement : NetworkBehaviour
    {
        // Variables for movement
        private float speed = 3f;
        private float dashingspeed = 4.5f;
        private bool frozen = false;

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

        private void Awake()
        {
            rigidInstance = GetComponent<Rigidbody2D>();
            animatorScript = GetComponent<PlayerAnimation>();
            rendererInstance = GetComponent<SpriteRenderer>();
        }

        public void MovePlayer()
        {
            if (movementState == States.Dashing || frozen) // Should get rid of dashing and only make it a ninja logic
            {
                return;
            }
            float x, y;
            CalculateSpeeds(out x, out y);

            if (Mathf.Abs(x) != 0 || Mathf.Abs(y) != 0)
            {
                if (y > 0)
                {
                    animatorScript.ChangeStateRunning("Up");
                }
                else if (y < 0)
                {
                    animatorScript.ChangeStateRunning("Down");
                }
                else if (x != 0)
                {
                    animatorScript.ChangeStateRunning("Horizontal");
                    FlipMovement(x);
                }
                else
                {
                    Debug.Log("Should never be this case");
                }
                movementState = States.Moving;
            }
            else
            {
                animatorScript.ChangeStateRunning();
                movementState = States.NoMovement;
            }

            rigidInstance.velocity = new Vector2(x, y);
        }

        public bool CheckDash(bool keyDown) // Change this to be inherited by the rogue movement class
        {
            if (keyDown && movementState == States.Moving) // implies movement state != dashing
            {
                movementState = States.Dashing;
                currSpeed = rigidInstance.velocity;
            }

            if (movementState == States.Dashing && dashingTime > 0)
            {
                rigidInstance.velocity = currSpeed * dashingspeed;
                dashingTime -= 1 * Time.deltaTime;
                return true;
            }
            else if (movementState == States.Dashing && dashingTime < 0)
            {
                movementState = States.NoMovement;
                dashingTime = 0.2f;
                rigidInstance.velocity = new Vector2(0, 0);
                return false;
            }
            else return false;
        }

        public void FreezeMovement()
        {
            if (movementState == States.Moving)
            {
                rigidInstance.velocity = new Vector2(0, 0);
                animatorScript.ChangeStateRunning();
            }
            frozen = true;
        }

        public void UnFreezeMovement()
        {
            frozen = false;
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
        [Client]
        private void FlipMovement(float x)
        {
            //Only go on for the LocalPlayer
            if (!isLocalPlayer) return;

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
            FlipMovementCommand(rendererInstance.flipX, this.transform.GetChild(0).transform.localEulerAngles);
        }

        [Command]
        private void FlipMovementCommand(bool flipState, Vector3 eulerAngles)
        {
            // To change where the characater is facing depending on input
            
            rendererInstance.flipX = flipState;
            FlipMovementClientRPC(rendererInstance.flipX, this.transform.GetChild(0).transform.localEulerAngles);
        }

        [ClientRpc]
        private void FlipMovementClientRPC(bool flipState, Vector3 eulerAngles)
        {
            if(isLocalPlayer) return;
            rendererInstance.flipX = flipState;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == "Player")
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.tag == "Player")
            {
                Debug.Log("Got hit by another player");
            }
        }

        ////////////////////////////////////////////
    }
}
