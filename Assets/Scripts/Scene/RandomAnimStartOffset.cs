using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimStartOffset : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("start_offset", Random.Range(0f, 1f));
    }
}
