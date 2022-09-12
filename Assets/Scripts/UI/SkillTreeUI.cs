using PlayerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class SkillTreeUI : MonoBehaviour
    {
        RectTransform rectTransform;
        float yScale = 1;
        float xScale = 1;

        float refResX;
        float refResY;
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            EventManager.UpdateResolutionEvent += ChangeRes;
            refResX = rectTransform.sizeDelta.x;
            refResY = rectTransform.sizeDelta.y;
        }
        private void OnDestroy()
        {
            EventManager.UpdateResolutionEvent -= ChangeRes;
        }

        void ChangeRes(int x, int y)
        {
            yScale = Mathf.Max(1, (float)y / 360);
            xScale = Mathf.Max(1, (float)x / 640);
            UpdateScale();
        }

        void UpdateScale()
        {
            float scaleVal = Mathf.Max(xScale, yScale);
            rectTransform.sizeDelta = new Vector2(refResX * scaleVal, refResY * scaleVal);
            rectTransform.ForceUpdateRectTransforms();
        }
    }
}
