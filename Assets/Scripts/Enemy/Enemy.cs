using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyUI;
using Mirror;

public class Enemy : NetworkBehaviour
{
    [SyncVar]
    public int health;
    private EnemyAnimation animatorScript;
    // Start is called before the first frame update
    void Awake()
    {
        health = 100;
        animatorScript = GetComponent<EnemyAnimation>();
    }

    [Client]
    public bool TakeDamage(int damage) 
    {
        animatorScript.ChangeStateToTakeDamage();
        health -= damage;
        TakeDamageCommand(health);
        if (health <= 0) 
        {
            animatorScript.ChangeStateToDie();
            return true;
        }
        return false;
    }

    [Command(requiresAuthority = false)] // Currently not working
    public void TakeDamageCommand(int hp)
    {
        Debug.Log("Hp now " + hp);
        this.health = hp;
        animatorScript.ChangeStateToTakeDamage();
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
