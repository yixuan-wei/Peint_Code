using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public bool  m_onColorPlatform = false;
    public float m_jumpStartSpeed = 7.95f;
    public float m_moveSpeed = 2.5f;
    public float m_jumpMoveSpeed= 2.51f;
    public float m_jumpClampedSpeed = 7.29f;
    public float m_fallMultiplier = 2.08f;
    public float m_lowJumpMultiplier = 2.5f;
    public float m_maxFallingSpeed = 30.0f;
    public float m_respawnPauseTime = 1.0f;
    public PlatformColor m_color = PlatformColor.PINK;

    private GameSettings m_settings = null;
    private MusicManager m_audioSFX = null;
    private PlayerInputScreen m_input = null;
    private SpriteRenderer m_renderer = null;
    private SpriteRenderer[] m_sprites = null;
    private Rigidbody2D m_rigidbody = null;
    public Color[] m_tints = null;
    private Animator[] m_animators = null;
    private Coroutine m_respawnPause = null;
    private bool m_respawnPausing = true;

    private ParticleSystem[] m_particles = null;

    private void Start()
    {
        m_settings = GameObject.FindObjectOfType<GameSettings>();
        int difficulty = PlayerPrefs.GetInt(GameSettings.s_difficulty,0);
        if ( difficulty < 0 || difficulty > m_settings.m_playerMetrics.Length - 1 )
            Debug.LogError( "difficulty stored in PlayerPrefs is wrong" );
        PlayerMetrics curMetric = m_settings.m_playerMetrics[difficulty];
        m_jumpStartSpeed = curMetric.m_jumpStartSpeed;
        m_jumpMoveSpeed = curMetric.m_jumpMoveSpeed;
        m_lowJumpMultiplier = curMetric.m_lowJumpMultiplier;
        m_jumpClampedSpeed = curMetric.m_jumpClampedSpeed;
        m_maxFallingSpeed = curMetric.m_maxFallingSpeed;
        m_moveSpeed = curMetric.m_moveSpeed;
        m_fallMultiplier = curMetric.m_fallMultiplier;
        m_respawnPauseTime = curMetric.m_respawnPauseTime;

        ParticleSystem[] tempParticles = GetComponentsInChildren<ParticleSystem>();
        if ( tempParticles.Length != (int)PlatformColor.NUM_COLORS )
            Debug.LogError( "Particle system's number in player is wrong" );
        m_particles = new ParticleSystem[3];
        for(int pIdx=0;pIdx<tempParticles.Length;pIdx++ )
        {
            if(tempParticles[pIdx].name=="GreenParticles")
            {
                m_particles[1] = tempParticles[pIdx];
            }
            else if(tempParticles[pIdx].name=="PinkParticles")
            {
                m_particles[0] = tempParticles[pIdx];
            }
            else if(tempParticles[pIdx].name=="YellowParticles")
            {
                m_particles[2] = tempParticles[pIdx];
            }
            tempParticles[pIdx].Stop();
        }

        m_audioSFX = GameObject.FindObjectOfType<MusicManager>();
        m_input = gameObject.GetComponent<PlayerInputScreen>();
        m_rigidbody = gameObject.GetComponent<Rigidbody2D>();
        m_sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        for ( int id = 0; id < m_sprites.Length; id++ )
        {
            if ( m_sprites[id].gameObject.name == "Visual_Color" )
            {
                m_renderer = m_sprites[id];
                break;
            }
        }

        m_animators = gameObject.GetComponentsInChildren<Animator>();
        if (m_respawnPause == null)
            m_respawnPause = StartCoroutine(RespawnPause());
        foreach(Animator anim in m_animators)
        {
            //anim.SetBool( "Walking", true );
            anim.SetFloat( "JumpSpeed", curMetric.m_jumpAnimSpeed );
            anim.SetFloat( "WalkSpeed", curMetric.m_walkAnimSpeed );
            anim.SetBool( "Jumping", false );
        }

        UpdateColor();
    }

    void UpdateParticles(int colorIdx)
    {
        for(int idx=0;idx<m_particles.Length;idx++ )
        {
            if ( idx == colorIdx )
                m_particles[idx].Play();
            else m_particles[idx].Stop();
        }
    }

    void UpdateColor()
    {
        if ( m_input.m_changeColor )
        {
            int newColorID = ((int)m_color+1)%(int)PlatformColor.NUM_COLORS;
            m_color = (PlatformColor)newColorID;
            UpdateParticles( newColorID );
            m_input.m_wasChanging = true;
            m_input.m_changeColor = false;
            m_audioSFX.PlayColorSFX();
        }
        if ( -1 < (int)m_color && (int)m_color < (int)PlatformColor.NUM_COLORS )
        {
            Color newc = m_tints[(int)m_color];
            m_renderer.color = new Color(newc.r,newc.g,newc.b);
        }
        else
        {
            Debug.LogError( "Player Sprite Index out of Color Enum range" );
        }
    }

    void UpdateLeftRightMove()
    {
        float moveSpeed = m_moveSpeed;
        if ( m_rigidbody.velocity.y != 0.0f )
            moveSpeed = m_jumpMoveSpeed;
        else m_audioSFX.PlayRunSFX();
        Vector2 position = m_rigidbody.position;
        position.x += moveSpeed * Time.deltaTime;
        for ( int sID = 0; sID < m_sprites.Length; sID++ )
            m_sprites[sID].flipX = true;
        m_rigidbody.position = position;

    }

    void UpdateJumpMove()
    {
        Vector2 velocity = m_rigidbody.velocity;
        if ( m_input.m_jump && m_input.m_readyToJump && !m_input.m_wasJumping )
        {
            velocity.y = m_jumpStartSpeed;
            m_audioSFX.PlayJumpSFX();
            m_audioSFX.PlayUIButtonSFX();
            m_input.m_wasJumping = true;
        }
        else if ( !m_input.m_jump )
        {
            velocity.y = velocity.y.Clamp( m_jumpClampedSpeed );
        }
        if ( velocity.y < 0 )
        {
            velocity.y += Physics2D.gravity.y * ( m_fallMultiplier - 1 ) * Time.deltaTime;
        }
        else if ( velocity.y > 0 && !m_input.m_jump )
        {
            velocity.y += Physics2D.gravity.y * ( m_lowJumpMultiplier - 1 ) * Time.deltaTime;
        }
        velocity.y = -( -velocity.y ).Clamp( m_maxFallingSpeed );
        if(!m_input.m_readyToJump)
        {
            foreach ( Animator anim in m_animators )
            {
                anim.SetBool( "Jumping", true );
            }
        }
        else
        {
            foreach ( Animator anim in m_animators )
            {
                anim.SetBool( "Jumping", false );
            }
        }
        m_rigidbody.velocity = velocity;
    }

    IEnumerator RespawnPause()
    {
        yield return new WaitForSeconds(m_respawnPauseTime);
        foreach (Animator anim in m_animators)
        {
            anim.SetBool( "Walking", true );
        }
        m_respawnPausing = false;
        yield return null;
    }


    // Update is called once per frame
    void Update()
    {
        if ( Time.timeScale != 0f)
        {
            UpdateColor();
            if (!m_respawnPausing)
            {
                UpdateLeftRightMove();
                UpdateJumpMove();
                m_rigidbody.angularVelocity = 0;
            }
        }
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        if ( collision.collider.gameObject.layer == LayerMask.NameToLayer( "Platform" )
            && collision.contacts[0].normal.x == 0
            && collision.contacts[0].normal.y > 0 )
        {
            m_input.m_readyToJump = true;
            if(collision.gameObject.GetComponent<PlatformSwitcher>()!=null)//is a color platform
            {
                m_onColorPlatform = true;
            }
        }
    }

    private void OnCollisionStay2D( Collision2D collision )
    {
        if ( collision.collider.gameObject.layer == LayerMask.NameToLayer( "Platform" )
            && collision.contacts[0].normal.x == 0
            && collision.contacts[0].normal.y > 0 )
        {
            m_input.m_readyToJump = true;
            if ( collision.gameObject.GetComponent<PlatformSwitcher>() != null )//is a color platform
            {
                m_onColorPlatform = true;
            }
        }
    }

    private void OnCollisionExit2D( Collision2D collision )
    {
        if ( collision.collider.gameObject.layer == LayerMask.NameToLayer( "Platform" ) )
        {
            m_input.m_readyToJump = false;
            if ( collision.gameObject.GetComponent<PlatformSwitcher>() != null )//is a color platform
            {
                m_onColorPlatform = false;
            }
        }
    }
}
