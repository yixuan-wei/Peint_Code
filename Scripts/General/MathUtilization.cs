using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtilization
{
    public static float Clamp( this float target, float positiveLimit )
    {
        if ( Mathf.Abs( target ) < positiveLimit )
            return target;
        else if ( target < 0.0f )
            return -positiveLimit;
        else return positiveLimit;
    }

    public static Vector2 Clamp( this Vector2 target, float maxLength )
    {
        if ( target.magnitude < maxLength )
            return target;
        else
        {
            float factor = maxLength/target.magnitude;
            return new Vector2( target.x * factor, target.y * factor );
        }
    }

    //size is assumed to be (positiveX, positiveY)
    public static bool IsVector2InsideSize(Vector2 vec1, Vector2 vec2, Vector2 size)
    {
        Vector2 difference = vec1-vec2;
        if ( difference.x < size.x && difference.x > -size.x && difference.y < size.y && difference.y > -size.y )
        {
            return true;
        }
        else return false;
    }

    //whether position inside circle of center and radius
    public static bool IsPositionInsideCircle( this Vector3 center, float radius, Vector3 position )
    {
        Vector3 difference = position-center;
        if ( difference.magnitude < radius )
            return true;
        else return false;
    } 
        
    public static void SortMonoBehaviorArrayWithPositionX(this MonoBehaviour[] array)
    {
        float length = array.Length;
        for ( int i = 0; i < length; i++ )
            for ( int j = 0; j < length - i - 1; j++ )
            {
                if(array[j].transform.position.x>array[j+1].transform.position.x)
                {
                    MonoBehaviour temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
    }
}