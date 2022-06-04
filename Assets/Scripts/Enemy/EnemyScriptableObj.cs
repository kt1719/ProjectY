using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "EnemyStats")]
public class EnemyScriptableObj : ScriptableObject
{
    public enum EnemyClass
    {
        Slime,
        None
    }

    public int health;
    public float aggroSpeed;
    public float walkSpeed;
    public float aggroRadius;
    public float attackRadius;
    public float outerRadius;
    public float movementRadius;
    public int damage;
    public float idleTime;
    public int xpGiven;
    public float centerYOffset; // Used because the center of the sprite is not actually the center

    public EnemyClass enemyClass;
}
