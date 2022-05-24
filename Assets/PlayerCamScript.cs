using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        this.transform.localPosition = new Vector3(0, 0, -10);
    }
}
