using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    HealthBar healthBar;
    private void Awake()
    {
        healthBar = this.transform.Find("Healthbar").GetComponent<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageHPUI (int n)
    {
        healthBar.DamageHP(n);
        // Animate shake
    }
}
