using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSet : MonoBehaviour
{
    public int size;
    public float scale;
    public RectTransform[] rect;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        rect[0].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / scale);
        rect[0].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height / scale);
        
        rect[1].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / scale);
        rect[1].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height / scale);

        rect[2].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / scale);
        rect[2].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height / scale);
    }

    void Update()
    {
        size = Screen.width;
    }
}
