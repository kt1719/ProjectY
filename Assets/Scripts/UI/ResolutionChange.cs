using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionChange : MonoBehaviour
{
    Dropdown dropDown;

    int resolutionChoice;

    Dictionary<int, Vector2> hash = new Dictionary<int, Vector2>()
    {
        { 0, new Vector2(960, 540) },
        { 1, new Vector2(858, 480) },
        { 2, new Vector2(640, 360) },
    };

    private void Awake()
    {
        dropDown = GetComponent<Dropdown>();

        dropDown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropDown);
        });

    }
    
    void DropdownValueChanged(Dropdown change)
    {
        if (resolutionChoice == change.value)
        {
            return;
        }

        resolutionChoice = change.value;
        EventManager.UpdateResolution((int)hash[resolutionChoice].x, (int)hash[resolutionChoice].y);
        Debug.Log(change.value);
        Debug.Log(change.captionText.text);
    }
}
