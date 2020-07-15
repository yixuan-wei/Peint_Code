using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ButtonUtilization 
{
    public static bool IsClickInside( this Button button, Vector3 mousePos, float pad, bool isWorldCanvas = false)
    {
        RectTransform rTrans = button.gameObject.GetComponent<RectTransform>();
        Vector2 scale = rTrans.rect.size*.5f + new Vector2(pad,pad);
        Vector3 pos = rTrans.position;
        if ( isWorldCanvas )
            pos = Camera.main.WorldToScreenPoint( pos );

        if ( MathUtilization.IsVector2InsideSize( pos, mousePos, scale ) )
        {
            return true;
        }
        else return false;
    }


}
