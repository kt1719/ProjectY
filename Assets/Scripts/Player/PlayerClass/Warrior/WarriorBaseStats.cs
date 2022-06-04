using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Warrior Stat", menuName = "WarriorStats")]
public class WarriorBaseStats : ScriptableObject
{
    public int health;
    public float speed;
}