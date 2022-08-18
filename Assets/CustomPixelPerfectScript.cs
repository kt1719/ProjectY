using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CustomPixelPerfectScript : MonoBehaviour
{
    public int PPU;
    public int refResX;
    public int refResY;
    public bool cropFrameXBool;
    public bool cropFrameYBool;

    PixelPerfectCamera pixelPerfectCameraScript;

    private void Awake()
    {
        pixelPerfectCameraScript = GetComponent<PixelPerfectCamera>();
        pixelPerfectCameraScript.cropFrameY = cropFrameYBool;
        pixelPerfectCameraScript.cropFrameX = cropFrameXBool;
        pixelPerfectCameraScript.refResolutionX = refResX;
        pixelPerfectCameraScript.refResolutionY = refResY;
        pixelPerfectCameraScript.assetsPPU = PPU;
    }
}
