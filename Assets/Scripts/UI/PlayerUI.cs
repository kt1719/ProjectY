using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    HealthBar healthBar;
    float duration = 0.64f / 6;
    public LeanTweenType easeType;
    private void Awake()
    {
        healthBar = this.transform.Find("Healthbar").GetComponent<HealthBar>();
    }

    public void DamageHPUI (int n)
    {
        healthBar.DamageHP(n);
        // Animate shake
        foreach (Transform child in transform)
        {
            LeanTween.moveX(child.gameObject, child.transform.position.x + 10f, duration).setLoopPingPong(2).setEase(easeType);
        }
    }
}
