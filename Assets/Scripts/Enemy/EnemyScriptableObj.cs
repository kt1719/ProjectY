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
    public float speed;
    public float attackRadius;
    public float movementRadius;
    public int damage;
    public float idleTime;
    public int xpGiven;

    public EnemyClass enemyClass;
}
