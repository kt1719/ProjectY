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
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

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
            yScale = Mathf.Max(1, (float)y / 360);
            xScale = Mathf.Max(1, (float)x / 640);
            UpdateScale();
        }

        void UpdateScale()
        {
            float scaleVal = Mathf.Max(xScale, yScale);
            rectTransform.sizeDelta = new Vector2(540 * scaleVal, 255 * scaleVal);
            rectTransform.ForceUpdateRectTransforms();
        }
    }
}
