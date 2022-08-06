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
            player.GetComponent<PlayerController>().SpawnPlayer();
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}
