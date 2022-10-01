using PlayerClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    RectTransform rectTransform;
    public int hpOffset = -3;
    PlayerClass playerClassScript;
    GameObject healthObject;
    private void Awake()
    {
        healthObject = this.transform.Find("Health").gameObject;
        rectTransform = healthObject.GetComponent<RectTransform>();
        playerClassScript = this.transform.root.GetComponent<PlayerClass>();
    }

    void HealHP(int n)
    {

    }

    public void DamageHP(int n)
    {
        // Create two healthbars from the previous one
        GameObject healthBar2 = GenerateNewHealthBars(n);
        // Animate second healthbar using code
        // To be implemented
    }

    private GameObject GenerateNewHealthBars(int n)
    {
        // Get starting hp
        int currHP = playerClassScript.readCurrentHP();

        // Get percentage decrease
        float ratio = (float)(currHP - n) / (float)(currHP);
        float newPos = rectTransform.sizeDelta.x * ratio;

        GameObject healthBar1 = Instantiate(healthObject, healthObject.transform);
        healthBar1.transform.SetParent(this.transform);
        Debug.Log(healthBar1.name);
        healthBar1.GetComponent<RectTransform>().sizeDelta = new Vector2(newPos, rectTransform.sizeDelta.y);
        healthBar1.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        //GameObject healthBar2 = Instantiate(healthObject, healthObject.transform);
        //healthBar2.transform.SetParent(this.transform);
        //healthBar2.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPos + hpOffset, rectTransform.anchoredPosition.y);
        //healthBar2.GetComponent<RectTransform>().sizeDelta = new Vector2(rectTransform.sizeDelta.x - newPos - hpOffset, rectTransform.sizeDelta.y);

        // Delete original healthbar and healthbar2 and replace with healthbar1
        UpdatePrevVars(healthBar1);
        //return healthBar2;
        return healthBar1;
    }

    private void UpdatePrevVars(GameObject healthBar1)
    {
        Destroy(healthObject);
        healthObject = healthBar1;
        rectTransform = healthObject.GetComponent<RectTransform>();
    }
}
