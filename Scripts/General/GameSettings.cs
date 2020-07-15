using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct PlayerMetrics
{
    public float m_jumpStartSpeed;
    public float m_moveSpeed;
    public float m_jumpMoveSpeed;
    public float m_jumpClampedSpeed;
    public float m_fallMultiplier;
    public float m_lowJumpMultiplier;
    public float m_maxFallingSpeed;
    public float m_respawnPauseTime;
    public bool  m_checkpointOn;
    public float m_jumpAnimSpeed;
    public float m_walkAnimSpeed;
}

public class GameSettings : MonoBehaviour
{
    private int m_difficulty = 0;
    private int m_levelProgress=0;

    public PlayerMetrics[] m_playerMetrics;
    static public string s_levelProgress = "LevelProgress";
    static public string s_difficulty = "Difficulty";

    private void Awake()
    {
        if ( GameObject.FindObjectsOfType<GameSettings>().Length > 1 )
            Destroy( gameObject );
        DontDestroyOnLoad( gameObject );

        if ( m_playerMetrics.Length != 3 )
            Debug.LogError( "Player Metrics should have 3 sets: easy, normal, difficulty" );

        m_levelProgress = PlayerPrefs.GetInt( s_levelProgress, 0 );
        m_difficulty = PlayerPrefs.GetInt( s_difficulty, 0 );
    }

    // Update is called once per frame
    void Update()
    {
        m_levelProgress = PlayerPrefs.GetInt( s_levelProgress );
        m_difficulty = PlayerPrefs.GetInt( s_difficulty );
    }

    public void SetDifficulty( int difficulty )
    {
        PlayerPrefs.SetInt( s_difficulty, difficulty );
    }

    public void ResetLevelProgress()
    {
        PlayerPrefs.SetInt( s_levelProgress, 0 );
    }
        
}
