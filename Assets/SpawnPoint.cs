using FX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawn
{
    public class SpawnPoint : MonoBehaviour
    {
        private int playersSpawning;
        public GameObject spawnFXObject;
        public GameObject fxSpawnLocation;
        public GameObject playerSpawnLocation;
        public void Update()
        {
            playersSpawning = transform.childCount;
        }

        private void LateUpdate()
        {
            if (playersSpawning == 0)
            {
                TurnOffSpawn();
            }
            else
            {
                TurnOnSpawn();
            }
        }

        public void SpawnFX(GameObject player)
        {
            GameObject playerFX = Instantiate(spawnFXObject, transform);
            playerFX.transform.SetParent(fxSpawnLocation.transform);
            playerFX.transform.localPosition = new Vector3(0, 0, 0);
            playerFX.GetComponent<SpawnFXScript>().ReferencePlayer(player);
        }

        public Vector2 ReturnCenterPos()
        {
            return playerSpawnLocation.transform.position;
        }

        void TurnOffSpawn()
        {
            return;
        }

        void TurnOnSpawn()
        {
            return;
        }
    }
}
