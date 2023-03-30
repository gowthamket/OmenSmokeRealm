using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Omen : MonoBehaviour, IGameEventListener
{
    [SerializeField] private bool m_smoking;
    [SerializeField] private bool m_alternateVision;

    [Header("Camera")]
    [SerializeField] private Camera m_camera;
    [SerializeField] private Camera m_cameraIndicator;
    [SerializeField] private LayerMask m_layerMaskNormal;
    [SerializeField] private LayerMask m_layerMaskAlternate;

    [Header("Smoke")]
    [SerializeField] private float m_smokeSpeed;
    [SerializeField] private float m_smokeDistanceDefault;
    [SerializeField] private Vector2 m_smokeDistanceRange = new Vector2(0, 0);
    [SerializeField] private Smoke m_smokePrefab;
    private float m_smokeDistance;
    private Smoke m_smoke;

    [Header("Reference")]
    [SerializeField] private Var_Camera m_cameraSO;
    [SerializeField] private ScreenWipe m_screenWipe;
    [SerializeField] private MeshSwapper m_meshSwapper;

    private void OnValidate()
    {
        
    }
    private void Start()
    {
        m_cameraSO.Value = m_camera;
        m_screenWipe.RegisterListener(this);
    }
    private void Update()
    {
        if (!m_smoking)
        {
            //Enter Smoke
            if (Input.GetKeyDown(KeyCode.E))
            {
                m_smoking = true;

                //Handle Smoke
                m_smokeDistance = m_smokeDistanceDefault;
                m_smoke = Instantiate(m_smokePrefab);
                m_smoke.transform.position = m_camera.transform.position + m_camera.transform.forward * m_smokeDistance;

                //Handle Cameras
                m_camera.enabled = false;
                m_cameraIndicator.enabled = true;
                m_cameraIndicator.transform.position = m_camera.transform.position;
                m_cameraIndicator.transform.rotation = m_camera.transform.rotation;

                //Handle Vision
                if (m_alternateVision)
                    m_screenWipe.StartWipe();
            }
        }
        else
        {
            //Exit Smoke
            if (Input.mouseScrollDelta.sqrMagnitude != 0)
            {
                m_smoking = false;

                //Handle Cameras
                m_camera.enabled = true;
                m_cameraIndicator.enabled = false;

                //Handle Smoke
                Debug.Log(m_smoke.name, m_smoke);
                Destroy(m_smoke.gameObject);

                //Handle Vision
                if (m_alternateVision)
                    m_screenWipe.StartWipe();
            }

            //Deploy Smoke
            else if (Input.GetKeyDown(KeyCode.E))
            {
                m_smoking = false;

                //Handle Cameras
                m_camera.enabled = true;
                m_cameraIndicator.enabled = false;

                //Handle Vision
                if (m_alternateVision)
                    m_screenWipe.StartWipe();

                //Handle Smoke
                m_smoke.Deploy();
            }

            //Change Vision
            else if (Input.GetKeyDown(KeyCode.R)) // if player presses R, screenwipe happens, and vision goes between regular and shadow realm vision
            {
                m_screenWipe.StartWipe();
                m_alternateVision = !m_alternateVision;
            }

            //Handle Smoke Control
            else
            {
                //Extend/Retract Smoke
                int _direction = 0;
                if      (Input.GetKey(KeyCode.Mouse0)) { _direction = 1; }
                else if (Input.GetKey(KeyCode.Mouse1)) { _direction = -1; }

                m_smokeDistance = Mathf.Clamp(m_smokeDistance + m_smokeSpeed * _direction * Time.deltaTime, m_smokeDistanceRange.x, m_smokeDistanceRange.y);
                m_smoke.transform.position = m_camera.transform.position + m_camera.transform.forward * m_smokeDistance;
                m_cameraIndicator.transform.position = m_camera.transform.position + m_camera.transform.forward * (m_smokeDistance - Mathf.Min(m_smokeDistanceDefault, m_smokeDistance));
                m_cameraIndicator.transform.rotation = m_camera.transform.rotation;
            }
        }
    }

    void IGameEventListener.OnEventRaised()
    {
        if (m_smoking)
        {
            if (m_alternateVision)
            {
                m_meshSwapper.SwapTo(); // swaps to shadow realm
                m_cameraIndicator.cullingMask = m_layerMaskAlternate;
            }
            else
            {
                m_meshSwapper.SwapBack(); //swaps to normal realm
                m_cameraIndicator.cullingMask = m_layerMaskNormal;
            }
        }
        else // also swap back if we have exited the smoking state
        {
            m_meshSwapper.SwapBack();
            m_cameraIndicator.cullingMask = m_layerMaskNormal;
        }
    }

}
