using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomSceneManager : MonoBehaviour
{
    [SerializeField] GameObject ninja;
    // Start is called before the first frame update
    void Awake()
    {
       GameObject m_NinjaInstantiated = Instantiate(ninja);
       NetworkServer.Spawn(m_NinjaInstantiated);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
