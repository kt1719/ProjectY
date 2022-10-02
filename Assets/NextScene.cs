using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class NextScene : MonoBehaviour
{
    public int nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        Debug.Log(collision.transform.root.GetComponent<NetworkBehaviour>());
        if (collision.transform.root.GetComponent<NetworkBehaviour>().hasAuthority)
        {
            Debug.Log("Switching");
            SceneManager.LoadScene(nextScene, LoadSceneMode.Additive);
            Destroy(this.gameObject);
            // Unload everything in the previous scene including this
        }
    }
}
