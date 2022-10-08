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
        healthBar = this.transform.GetChild(0).Find("Healthbar").GetComponent<HealthBar>();
    }

    public void DamageHPUI (int n)
    {
        LeanTween.moveX(this.transform.GetChild(0).gameObject, this.transform.GetChild(0).transform.position.x + 10f, duration).setLoopPingPong(2).setEase(easeType);
        healthBar.DamageHP(n);
        // Animate shake
    }
}
