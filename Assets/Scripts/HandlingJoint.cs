using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ConfigurableJoint))]
public class HandlingJoint : MonoBehaviour
{
    Rigidbody m_target;
    ConfigurableJoint m_joint;

    Vector3 m_initDragPosition;

    bool m_isHolding;
    public bool isHolding
    {
        get { return m_isHolding; }
        set
        {
            m_isHolding = value;

            if (m_isHolding && m_target)
            {
                m_initDragPosition = transform.position;
                m_joint.connectedBody = m_target;

                if(!m_target.GetComponent<ConfigurableJoint>())
                    m_target.freezeRotation = true;
            }
            else
            {
                m_joint.connectedBody = null;
                if(m_target && !m_target.GetComponent<ConfigurableJoint>())
                    m_target.freezeRotation = false;
            }
        }
    }

    void Start()
    {
        m_joint = GetComponent<ConfigurableJoint>();
    }

    void Update()
    {
        if (m_isHolding && m_target)
        {
            if (!m_target.GetComponent<ConfigurableJoint>())
                m_target.transform.rotation = transform.rotation;
            else
                m_target.GetComponent<Rigidbody>().velocity = (transform.position - m_initDragPosition);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(!m_isHolding)
        {
            Rigidbody newTarget = other.GetComponent<Rigidbody>();

            if (newTarget)
                m_target = newTarget;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(!m_isHolding && other.GetComponent<Rigidbody>() == m_target)
            m_target = null;
    }
}
