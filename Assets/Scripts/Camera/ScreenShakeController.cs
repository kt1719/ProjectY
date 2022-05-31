using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    private float shakeTimeRemaining, shakePower;

    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            transform.localPosition = new Vector3(xAmount, yAmount, transform.position.z);
        }
        else if(shakeTimeRemaining < 0)
        {
            shakeTimeRemaining = 0;
        }
        else
        {
            transform.localPosition = new Vector3(0, 0, transform.position.z);
        }
    }
    public void StartShake(float shakeTime, float power)
    {
        shakeTimeRemaining = shakeTime;
        shakePower = power;
    }
}
