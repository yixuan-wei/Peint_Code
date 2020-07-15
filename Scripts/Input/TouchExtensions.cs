using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TouchExtensions 
{
    public static bool IsFinished(this Touch t)
    {
        return t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended;
    }
}
