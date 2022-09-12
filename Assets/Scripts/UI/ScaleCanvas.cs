using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class ScaleCanvas : MonoBehaviour
{
    CanvasScaler canvasScaler;
    private void Awake()
    {
        canvasScaler = this.GetComponent<CanvasScaler>();
    }
    /// <summary>
    /// Subscriber Events for the Character UI Script
    /// </summary>
    private void OnEnable()
    {
        EventManager.UpdateResolutionEvent += ChangeRes;
    }

    private void OnDisable()
    {
        EventManager.UpdateResolutionEvent -= ChangeRes;
    }

    void ChangeRes(int x, int y)
    {
        canvasScaler.referenceResolution = new Vector2(x, y);
    }
}
