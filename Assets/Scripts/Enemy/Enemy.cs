using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyUI;

public class Enemy : MonoBehaviour
{
    private int health;
    private EnemyAnimation animatorScript;
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        animatorScript = GetComponent<EnemyAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage) 
    {
        animatorScript.ChangeStateToTakeDamage();
        health -= damage;
        if (health <= 0) 
        {
            animatorScript.ChangeStateToDie();
        }
    }

    // DO NOT CALL, this should be called from the animation timeline and not programmatically
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
