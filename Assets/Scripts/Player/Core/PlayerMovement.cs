using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using PlayerAnim;
using PlayerClasses;

namespace PlayerCore
{
    public class PlayerMovement : NetworkBehaviour
    {
        // Variables for movement
        public static float speed;
        private float dashingspeed = 4.5f;
        private bool frozen = false;
        private bool pressedMovementKey = false;

        // Variables for dashing
        private float dashingTime = 0.2f;

        private float currentSpeedX = 0;
        private float currentSpeedY = 0;

        // Movement State tracker
        private States movementState = States.NoMovement;
        private Vector2 currSpeed = Vector2.zero;

        private PlayerAnimation animatorScript;

        private Rigidbody2D rigidInstance;
        private SpriteRenderer rendererInstance;
        private Warrior warriorScript;

        private int pixelsPerUnit;
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
            warriorScript = GetComponent<Warrior>();
            pixelsPerUnit = (int)GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        }

        public void MovePlayer()
        {
            if (movementState == States.Dashing || frozen) // Should get rid of dashing and only make it a ninja logic
            {
                return;
            }
            CalculateSpeeds(out currentSpeedX, out currentSpeedY);

            rigidInstance.velocity = new Vector2(currentSpeedX, currentSpeedY);
        }

        public void ChangeAnimatorAnimation()
        {
            if (Mathf.Abs(currentSpeedX) != 0 || Mathf.Abs(currentSpeedY) != 0)
            {
                if (currentSpeedY > 0)
                {
                    animatorScript.ChangeStateRunning("Up");
                }
                else if (currentSpeedY < 0)
                {
                    animatorScript.ChangeStateRunning("Down");
                }
                else if (currentSpeedX != 0)
                {
                    animatorScript.ChangeStateRunning("Horizontal");
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
            rigidInstance.velocity = new Vector2(0, 0);
            animatorScript.ChangeStateRunning();
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
            pressedMovementKey = (x != 0 || y != 0) ? true : false;
            float normaliser = Mathf.Sqrt(x * x + y * y);
            x = (x != 0) ? x / (normaliser) * warriorScript.getSpeed() : 0;
            y = (y != 0) ? y / (normaliser) * warriorScript.getSpeed() : 0;
        }

        // Flip Movement Logic //////////////////////
        public void FlipMovement()
        {
            float x_Velocity = Input.GetAxisRaw("Horizontal");
            if ((animatorScript.CheckHorizontalAnimatorState() && x_Velocity > 0) && pressedMovementKey && !frozen)
            {
                rendererInstance.flipX = false;
                this.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if ((animatorScript.CheckHorizontalAnimatorState() && x_Velocity < 0) && pressedMovementKey && !frozen)
            {
                rendererInstance.flipX = true;
                this.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            else if (!animatorScript.CheckHorizontalAnimatorState())
            {
                rendererInstance.flipX = false;
                this.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
            }

            //Only go on if it's multiplayer
            if (!isLocalPlayer) return;
            FlipMovementCommand(rendererInstance.flipX, this.transform.GetChild(0).transform.localEulerAngles);
        }

        [Command]
        private void FlipMovementCommand(bool flipState, Vector3 eulerAngles)
        {
            rendererInstance.flipX = flipState;
            FlipMovementClientRPC(rendererInstance.flipX, this.transform.GetChild(0).transform.localEulerAngles);
        }

        [ClientRpc(includeOwner = false)]
        private void FlipMovementClientRPC(bool flipState, Vector3 eulerAngles)
        {
            if (isLocalPlayer) return;
            rendererInstance.flipX = flipState;
        }

        // Should not be in the movement script!!!!!!!!!!!!!!!!!!!!!!!!
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
