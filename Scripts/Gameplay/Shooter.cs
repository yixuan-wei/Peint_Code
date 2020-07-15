using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject m_projectile = null;
    public float m_frequencyPerSecond = 1f;
    public float m_projectileLifeSpan = 1f;

    private Coroutine m_shootCounter = null;
    private float m_shootInterval = 0f;

    private void Awake()
    {
        m_shootInterval = 1f / m_frequencyPerSecond;
    }

    IEnumerator ShootProjectile()
    {
        GameObject projectile = Instantiate( m_projectile, transform.position,transform.rotation );
        projectile.transform.position = transform.position;
        projectile.GetComponent<Projectile>().m_livingTime = m_projectileLifeSpan;
        yield return new WaitForSeconds( m_shootInterval );
        m_shootCounter = null;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        m_shootInterval = 1f / m_frequencyPerSecond;
        if ( m_shootCounter == null )
            m_shootCounter = StartCoroutine( ShootProjectile() );
    }

    public void RestartShooting()
    {
        if ( m_shootCounter != null )
        {
            StopCoroutine( m_shootCounter );
            m_shootCounter = null;
        }
    }
}
