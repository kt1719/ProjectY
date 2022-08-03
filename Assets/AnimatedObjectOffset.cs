using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedObjectOffset : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        GetComponent<Animator>().SetFloat("start_offset", Random.value);
    }
}
