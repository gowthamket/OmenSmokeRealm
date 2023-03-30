using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour, IGameEventListener<string>
{
    [SerializeField] private bool m_fall;
    [SerializeField] private bool m_deployed;

    [SerializeField] private GameObject m_indicator;
    [SerializeField] private Comp_SMBEventCurrator m_eventCurrator;

    [Header("Smoke")]
    [SerializeField] private GameObject m_smoke;
    [SerializeField] private Vector3 m_smokeOffset;
    [SerializeField] private float m_fallSpeed;

    [Header("Center")]
    [SerializeField] private GameObject m_center;
    [SerializeField] private string m_centerLayerName;

    [Header("Land Position")]
    [SerializeField] private float m_checkDistance;
    [SerializeField] private LayerMask m_layerMask = -1;
    [SerializeField] private SpriteRenderer m_landPosition;    
    [SerializeField] private Vector3 m_offset;
    [SerializeField] private Color m_colorGood;
    [SerializeField] private Color m_colorBad;

    private bool m_hit;
    private RaycastHit m_raycastHit;


    private void Awake()
    {
        m_eventCurrator.RegisterListener(this);
    }
    private void Update()
    {
        // m_hit is a raycast shooting downward at a limited distance
        m_hit = Physics.Raycast(transform.position, Vector3.down, out m_raycastHit, m_checkDistance, m_layerMask);

        if (!m_deployed)
        {
            Vector3 _landPosPos;
            if (m_hit)  // if raycast hits something
            { 
                m_landPosition.color = m_colorGood;
                _landPosPos = m_raycastHit.point + m_offset;
            }
            else        
            { 
                m_landPosition.color = m_colorBad;
                _landPosPos = transform.position + m_offset; // change color to red and move to center of smoke with a little offset
            }

            m_landPosition.transform.position = _landPosPos;
        }
        else
        {
            if (m_fall)
            {
                if (m_hit)
                {
                    Vector3 _targetPosition = m_raycastHit.point + m_smokeOffset;
                    Vector3 _newPosition = Vector3.MoveTowards(transform.position, _targetPosition, m_fallSpeed * Time.deltaTime);
                    transform.position = _newPosition;
                    if (_newPosition == m_raycastHit.point)
                        m_fall = false;
                }
            }
        }
    }

    public void Deploy()
    {
        m_deployed = true;
        m_smoke.SetActive(true);
        m_center.SetActive(false);
        m_indicator.SetActive(false);
        m_landPosition.gameObject.SetActive(false);
    }

    void IGameEventListener<string>.OnEventRaised(string t)
    {
        if (t == "Middle")
        {
            m_fall = true;
            m_center.SetActive(true);
            m_center.layer = LayerMask.NameToLayer(m_centerLayerName);
        }
        else if (t == "End")
        {
            Destroy(gameObject);
        }
    }
}
