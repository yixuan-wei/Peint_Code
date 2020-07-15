using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSetting : MonoBehaviour
{
    private GameSettings m_settings=null;
    // Start is called before the first frame update
    void Start()
    {
        m_settings = GameObject.FindObjectOfType<GameSettings>();
    }

    public void OnResetRequest()
    {
        m_settings.ResetLevelProgress();
    }
}
