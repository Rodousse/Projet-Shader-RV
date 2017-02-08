using UnityEngine;
using System.Collections;
using Valve.VR;
[RequireComponent(typeof(SteamVR_TrackedController))]
public class ViveController : MonoBehaviour
{
    public float Range = 2500;
    private SteamVR_TrackedController controller;
    private Vector3 m_previousPos;

    Animator m_animator;
    // Use this for initialization
    void OnEnable()
    {
        m_animator = GetComponentInChildren<Animator>();
        controller = GetComponent<SteamVR_TrackedController>();
        controller.PadClicked += HandleTriggerEvent;
        controller.PadUnclicked += HandleTriggerEventRelease;
    }
    [SerializeField]
    GameObject erl;


    bool isHandled = false;
    void HandleTriggerEvent(object sender, ClickedEventArgs e)
    {
        isHandled = true;

        
        m_previousPos = transform.position;
    }

    void HandleTriggerEventRelease(object sender, ClickedEventArgs e)
    {
        isHandled = false;
        tr.gameObject.GetComponent<ConfigurableJoint>().connectedBody = null;
        
    }
    bool isInContact = false;
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() && isHandled == false && other.gameObject != gameObject
            && !other.gameObject.CompareTag("MainCamera") && !other.gameObject.GetComponent<ViveController>())
        {
            isInContact = false;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        IActivable activable = other.GetComponent<IActivable>();

        if (activable)
            activable.Switch();
        
    }
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() && other.gameObject != gameObject && !other.gameObject.CompareTag("MainCamera") 
            && !other.gameObject.GetComponent<ViveController>())//CompareTag("Pots"))
        {
            isInContact = true;
            if (!erl && isHandled)
            {
                erl = other.gameObject;
                if (!erl.GetComponent<ViveController>() && !erl.GetComponent<ConfigurableJoint>())
                {
                    erl.transform.position = tr.transform.position;
                    tr.gameObject.GetComponent<ConfigurableJoint>().connectedBody = erl.GetComponent<Rigidbody>();
                }
            }
            erl = other.gameObject;
            
        }

    }
    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Rigidbody>() && isHandled == false && other.gameObject != gameObject && !other.CompareTag("MainCamera") && !other.GetComponent<ViveController>())
        {
            isInContact = false;
        }
    }

    [SerializeField]
    Transform tr;
    void Update()
    {
        if (isInContact && erl)
        {
            if (isHandled)
            {
                m_animator.SetBool("grab", true);

                if (erl.GetComponent<ConfigurableJoint>())
                { 
                    erl.GetComponent<Rigidbody>().AddForce(( transform.position - m_previousPos) * 10);
                }
            }
            else
            {
                m_animator.SetBool("grab", false);
                
                
                
                if (!erl.GetComponent<ConfigurableJoint>())
                {
                    
                    erl = null;
                }
            }
        }
    }
}
