using UnityEngine;
using System.Collections.Generic;

public class HandController : MonoBehaviour
{
    Collider m_collider;
    Animator m_animator;
    SteamVR_TrackedController m_trackedController;

    HandlingJoint m_handlingJoint;
    
    void Start()
    {
        m_collider = GetComponent<Collider>();
        m_animator = GetComponentInChildren<Animator>();
        m_trackedController = GetComponent<SteamVR_TrackedController>();
        m_trackedController.PadClicked += onPad;
        m_trackedController.PadUnclicked += offPad;

        m_handlingJoint = GetComponentInChildren<HandlingJoint>();
    }

    void offPad(object sender, ClickedEventArgs e)
    {
        m_handlingJoint.isHolding = false;
        m_collider.enabled = true;
        m_animator.SetBool("grab", false);
    }

    void onPad(object sender, ClickedEventArgs e)
    {
        m_handlingJoint.isHolding = true;
        m_collider.enabled = false;
        m_animator.SetBool("grab", true);
    }

    void OnTriggerEnter(Collider other)
    {
        IActivable activable = other.GetComponent<IActivable>();
        if (activable)
            activable.Switch();
    }

    void OnCollisionEnter(Collision collision)
    {
        SteamVR_Controller.Input((int)m_trackedController.controllerIndex).TriggerHapticPulse(1000);
    }
}
