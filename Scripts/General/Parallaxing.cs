using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    /*
     * Track camera position, to judge how far to move this frame
     */
    [Range(0,1)]
    public float m_speedFactor = 1f;

    private Vector3 m_startCamPos;
    private Vector3 m_deltaDistance;
    private Vector3 m_startPosition;

    // Start is called before the first frame update
    void Start()
    {
        m_startPosition = transform.position;
        m_deltaDistance = transform.position - Camera.main.transform.position;
        m_startCamPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newCamPos = Camera.main.transform.position;
        //update the back ground appropriately
        m_deltaDistance= m_speedFactor * ( newCamPos - m_startCamPos );
        transform.position = m_startPosition+m_deltaDistance;
    }
}
