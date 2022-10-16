using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartTransitionAnimation()
    {
        animator.SetBool("transitioningScene", true);
    }

    public void EndTransitionAnimation()
    {
        animator.SetBool("transitioningScene", false);
    }

    public void SetReadyForTransition()
    {
        this.transform.parent.GetComponent<CharacterUI>().SetReadyForTransition();
    }
}
