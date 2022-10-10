using FX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using PlayerCore;
using System;

namespace Spawn
{
    public struct SpawnMessageV2 : NetworkMessage
    {}
    public class SpawnPoint : NetworkBehaviour
    {
        public bool localPlayerSpawned = false;
        private int playersSpawning;
        private bool spawning = false;
        public bool sendSpawnMessage = false;
        public GameObject spawnFXObject;
        public GameObject fxSpawnLocation;
        public GameObject playerSpawnLocation;
        public Queue<GameObject> playerFX;
        public Queue<GameObject> playerSpawnQueue;

        private void Awake()
        {
            playerSpawnQueue = new Queue<GameObject>();
            playerFX = new Queue<GameObject>();
        }

        public void Update()
        {
            CheckForSpawn();
            playersSpawning = transform.GetChild(0).GetChild(0).childCount; // Get the FX gameobject and count how many spawns are happening
        }

        void CheckForSpawn()
        {
            if (playerSpawnQueue.Count != 0 || !localPlayerSpawned)
            {
                StartCoroutine(SpawnFX());
            }
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
            GameObject player = (!localPlayerSpawned) ? NetworkClient.localPlayer.gameObject : playerSpawnQueue.Dequeue();
            spawning = true;
            if (!localPlayerSpawned)
            {
                localPlayerSpawned = true;
            }
            TurnOnSpawn();
            yield return new WaitForSeconds(0.3f);

            InstaniateFX();
            GameObject fx = playerFX.Dequeue();
            fx.layer = this.gameObject.layer;
            fx.GetComponent<SpawnFXScript>().ReferencePlayer(player);
        }

        void InstaniateFX()
        {
            GameObject res = Instantiate(spawnFXObject, transform);
            res.transform.SetParent(fxSpawnLocation.transform);
            res.layer = this.transform.root.gameObject.layer;
            res.transform.localPosition = new Vector3(0, 0, 0);
            playerFX.Enqueue(res);
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
