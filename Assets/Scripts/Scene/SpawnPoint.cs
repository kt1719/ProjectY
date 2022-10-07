using FX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Spawn
{
    public class SpawnPoint : NetworkBehaviour
    {
        private int playersSpawning;
        private bool spawning = false;
        public GameObject spawnFXObject;
        public GameObject fxSpawnLocation;
        public GameObject playerSpawnLocation;
        public GameObject playerFX;

        private void Start()
        {
            StartCoroutine(SpawnFX());
        }

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

        public IEnumerator SpawnFX()
        {
            GameObject player = NetworkClient.localPlayer.gameObject;
            TurnOnSpawn();
            yield return new WaitForSeconds(0.3f);
            spawning = true;

            playerFX = InstaniateFX();
            playerFX.layer = this.gameObject.layer;
            playerFX.GetComponent<SpawnFXScript>().ReferencePlayer(player);
            InstaniateFxCommand();
        }

        [Command (requiresAuthority = false)]
        void InstaniateFxCommand()
        {
            InstaniateFxRPC();
        }

        [ClientRpc]
        void InstaniateFxRPC()
        {
            InstaniateFX();
        }

        GameObject InstaniateFX()
        {
            if (playerFX)
            {
                return null;
            }
            GameObject res = Instantiate(spawnFXObject, transform);
            res.transform.SetParent(fxSpawnLocation.transform);
            res.layer = 6;
            res.transform.localPosition = new Vector3(0, 0, 0);
            return res;
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
