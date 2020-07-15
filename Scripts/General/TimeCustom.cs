using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCustom 
{
    public float m_duration;
    public float m_nextElapsed;
    // Start is called before the first frame update
    public void Start(float s)
    {
        m_duration = s;
        m_nextElapsed = Time.time + m_duration;
    }

    public bool HasElapsed()
    {
        return IsRunning() && ( Time.time >= m_nextElapsed );
    }

    float GetElapsed()
    {
        float start = m_nextElapsed - m_duration;
        return Time.time - start;
    }

    // 0 to 1 of how far into the timer I am; 
    public float GetElapsedNormalized()
    {
        return GetElapsed() / m_duration;
    }

    bool IsRunning()
    {
        if ( m_duration > 0 )
            return true;
        else return false;
    }

    public bool Decrement()
    {
        if ( HasElapsed() )
        {
            m_nextElapsed += m_duration;
            return true;
        }
        else return false;
    }

    // Update is called once per frame
    public void Stop()
    {
        m_duration = -1f;
    }
}
