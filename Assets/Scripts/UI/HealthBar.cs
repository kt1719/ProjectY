using PlayerClasses;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    RectTransform rectTransform;
    public GameObject damagedHPBarPrefab;
    public int hpOffset = -3;
    PlayerClass playerClassScript;
    GameObject healthObject;
    GameObject healthBar2;
    public LeanTweenType easeType;
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
        if (playerClassScript.readCurrentHP() <= 0)
        {
            return;
        }
        // Create two healthbars from the previous one
        healthBar2 = GenerateNewHealthBars(n);
        // Animate second healthbar using code
        SecondHPBarAnimation(healthBar2);
        //SecondHPBarAnimation();
    }

    private void SecondHPBarAnimation(GameObject healthBar)
    {
        LeanTween.move(healthBar, healthBar.transform.position + new Vector3(-5f, 20f), 0.56f).setEase(easeType);
        Color toColor = healthBar.GetComponent<Image>().color;
        toColor.a = 0;
        LeanTween.value(healthBar.gameObject, setColorCallback, healthBar.GetComponent<Image>().color, toColor, .28f)
            .setDelay(0.32f)
            .setOnComplete(DestroyMe);

        void DestroyMe()
        {
            Destroy(healthBar);
        }
    }

    private void setColorCallback(Color c)
    {
        Image img = healthBar2.GetComponent<Image>();
        img.color = c;
    }

    private GameObject GenerateNewHealthBars(int n) 
    {
        // Get starting hp
        int currHP = playerClassScript.readCurrentHP();

        // Get percentage decrease
        float ratio = Mathf.Max((float)(currHP - n) / (float)(currHP), 0);
        float newPos = rectTransform.sizeDelta.x * ratio;

        GameObject healthBar1 = Instantiate(healthObject, healthObject.transform);
        healthBar1.transform.SetParent(this.transform);
        healthBar1.GetComponent<RectTransform>().sizeDelta = new Vector2(newPos, rectTransform.sizeDelta.y);
        healthBar1.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        GameObject healthBar2 = Instantiate(damagedHPBarPrefab, healthObject.transform);
        healthBar2.transform.SetParent(this.transform);
        healthBar2.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPos + hpOffset, rectTransform.anchoredPosition.y);
        healthBar2.GetComponent<RectTransform>().sizeDelta = new Vector2(rectTransform.sizeDelta.x - newPos - hpOffset, rectTransform.sizeDelta.y);
        // Delete original healthbar and healthbar2 and replace with healthbar1
        UpdatePrevVars(healthBar1);
        return healthBar2;
    }

    private void UpdatePrevVars(GameObject healthBar1)
    {
        Destroy(healthObject);
        healthObject = healthBar1;
        healthObject.name = "Health";
        rectTransform = healthObject.GetComponent<RectTransform>();
    }
}
