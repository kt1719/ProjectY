using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void ChangeResolution(int x, int y);
    public static event ChangeResolution UpdateResolutionEvent;

    public void UpdateResolution(int x, int y)
    {
        if (UpdateResolutionEvent != null)
        {
            UpdateResolutionEvent(x, y);
        }
    }

    private void Start()
    {
        UpdateResolution(960, 540);
    }
}
