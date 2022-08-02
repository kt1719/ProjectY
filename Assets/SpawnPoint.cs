using FX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawn
{
    public class SpawnPoint : MonoBehaviour
    {
        private int playersSpawning;
        private bool spawning = false;
        public GameObject spawnFXObject;
        public GameObject fxSpawnLocation;
        public GameObject playerSpawnLocation;

        public void Update()
        {
            playersSpawning = transform.GetChild(0).GetChild(0).childCount; // Get the FX gameobject and count how many spawns are happening
        }

        private void LateUpdate()
        {
            if (playersSpawning == 0 && spawning)
            {
                StartCoroutine(TurnOffSpawn());
            }
        }

        public IEnumerator SpawnFX(GameObject player)
        {
            TurnOnSpawn();
            yield return new WaitForSeconds(0.3f);
            spawning = true;
            GameObject playerFX = Instantiate(spawnFXObject, transform);
            playerFX.transform.SetParent(fxSpawnLocation.transform);
            playerFX.transform.localPosition = new Vector3(0, 0, 0);
            playerFX.GetComponent<SpawnFXScript>().ReferencePlayer(player);
        }

        public Vector2 ReturnCenterPos()
        {
            return playerSpawnLocation.transform.position;
        }

        IEnumerator TurnOffSpawn()
        {
            yield return new WaitForSeconds(0.7f);
            GetComponent<Animator>().SetBool("Spawning", false);
        }

        void TurnOnSpawn()
        {
            GetComponent<Animator>().SetBool("Spawning", true);
        }
    }
}
