using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerCore;

namespace FX
{
    public class SpawnFXScript : MonoBehaviour
    {
        GameObject player;

        public void ReferencePlayer(GameObject obj)
        {
            player = obj;
        }

        public void SpawnPlayer()
        {
            if (!player)
            {
                return;
            }
            player.GetComponent<PlayerController>().SpawnPlayerAnimation();
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}
