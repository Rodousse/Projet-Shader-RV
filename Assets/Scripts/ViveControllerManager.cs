﻿using UnityEngine;

public class ViveControllerManager : MonoBehaviour
{
    [SerializeField]
    Transform m_leftStick;

    [SerializeField]
    Transform m_rightStick;

    AudioSource m_audioSource;
    [SerializeField]
    AudioClip[] m_stepsAudioClip;

    Vector3 m_rPos;
    Vector3 m_lPos;

    [SerializeField]
    float m_stepThreshold = 0.5f;
    float m_step = 0;

    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

	void Update ()
    {
        bool leftTrigger = m_leftStick.GetComponent<SteamVR_TrackedController>().triggerPressed;
        bool rightTrigger = m_rightStick.GetComponent<SteamVR_TrackedController>().triggerPressed;
        
        if (m_step > m_stepThreshold)
        {
            m_step -= m_stepThreshold;
            m_audioSource.PlayOneShot(m_stepsAudioClip[Random.Range(0, m_stepsAudioClip.Length)]);
        }

        if (leftTrigger && rightTrigger)
        {
            areaZone.drag = 0f;
            Vector3 velocity = Vector3.zero;
            Vector3 rvel = (m_rightStick.position - rOldPos) / Time.deltaTime;
            Vector3 lvel = (m_leftStick.position - lOldPos) / Time.deltaTime;

            RightMagnitude = rvel.magnitude;
            LeftMagnitude = lvel.magnitude;

            Magnitude = (rvel + lvel).magnitude;
            if (RightMagnitude > 1.0f && LeftMagnitude > 1.0f)//les deux bougent
            {
                if ((rvel + lvel).magnitude < threshold)//même vitesse (à peu près) et direction opposé
                {
                    velocity = (rvel - lvel) / 2;
                    Run(velocity);
                }                  

            }

            lOldPos = m_leftStick.position;
            rOldPos = m_rightStick.position;
        }
        else
            areaZone.drag = 5;

    }

    [SerializeField,Range(0,5)]
    float RightMagnitude, LeftMagnitude, Magnitude;

    [SerializeField, Range(0, 100)]
    float threshold; 

    [SerializeField]
    Rigidbody areaZone;

    Vector3 lOldPos;
    Vector3 rOldPos;
    [SerializeField]
    float speedAccel;

    [SerializeField]
    float maxSpeed;
    void Run(Vector3 vel)
    {
        Vector3 SumForCtrl = (m_leftStick.forward+m_rightStick.transform.forward)/ 2;
        vel.y = 0;
        Vector3 nVel = vel.magnitude * SumForCtrl * maxSpeed;
        nVel.y = 0;

        m_step += nVel.magnitude;
        //RaycastHit info;

        //Vector3 p_pos = transform.position;
        //p_pos.y = 0.2f;
        //Physics.SphereCast(p_pos, 0.33f, SumForCtrl, out info);
        //if (info.collider != null && info.collider.CompareTag("Obstacles"))
        //{
        //    areaZone.velocity = Vector3.zero;
        //    areaZone.isKinematic = true;
        //    areaZone.isKinematic = false;
        //}
        //else
        //{
        //    areaZone.velocity = Vector3.MoveTowards(areaZone.velocity, nVel, speedAccel * Time.deltaTime);
        //}

        areaZone.velocity = Vector3.MoveTowards(areaZone.velocity, nVel, speedAccel * Time.deltaTime);

    }
}
