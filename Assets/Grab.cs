using UnityEngine;
using HTC.UnityPlugin.Vive;
using System.Collections;
using Valve.VR;

public class Grab : MonoBehaviour {

	[SerializeField, Range(0,2)]
    float m_ScaleFactoMin = 1;

    Vector3 m_InitScale;
    Quaternion m_InitRot;

    Vector3[] m_InitPos = new Vector3[2];
    bool[] m_TriggerState = new bool[2];

    private GameObject target;

	// Use this for initialization
	void Start ()
    {
        m_InitScale = transform.localScale;
        m_InitRot = transform.rotation;

        ViveInput.AddPressDown(HandRole.RightHand, ControllerButton.Grip, () => OnTriggerDown(HandRole.RightHand));
        ViveInput.AddPressDown(HandRole.LeftHand, ControllerButton.Grip, () => OnTriggerDown(HandRole.LeftHand));

        ViveInput.AddPressUp(HandRole.RightHand, ControllerButton.Grip, () => OnTriggerUp(HandRole.RightHand));
        ViveInput.AddPressUp(HandRole.LeftHand, ControllerButton.Grip, () => OnTriggerUp(HandRole.LeftHand));
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_TriggerState[0] && m_TriggerState[1])
        {
            transform.localScale = Vector3.Max(m_InitScale * (1 - Vector3.Distance(m_InitPos[0], m_InitPos[1]) + Vector3.Distance(VivePose.GetPose(HandRole.RightHand).pos, VivePose.GetPose(HandRole.LeftHand).pos)),
                                                Vector3.one * m_ScaleFactoMin);
            
            transform.rotation = m_InitRot * Quaternion.FromToRotation(m_InitPos[0] - m_InitPos[1],
                                                                        VivePose.GetPose(HandRole.RightHand).pos - VivePose.GetPose(HandRole.LeftHand).pos);
        }
    }

    void OnTriggerDown(HandRole hand)
    {
        SetHandState(hand, true);

        m_InitPos[0] = VivePose.GetPose(HandRole.RightHand).pos;
        m_InitPos[1] = VivePose.GetPose(HandRole.LeftHand).pos;
    }

    void OnTriggerUp(HandRole hand)
    {
        SetHandState(hand, false);

        m_InitScale = transform.localScale;
        m_InitRot = transform.rotation;
    }

    void SetHandState(HandRole hand, bool state)
    {
        switch (hand)
        {
            case HandRole.RightHand:
                m_TriggerState[0] = state;
                break;
            case HandRole.LeftHand:
                m_TriggerState[1] = state;
                break;
            default:
                break;
        }
    }
}
