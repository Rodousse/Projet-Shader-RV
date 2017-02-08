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

                if (!erl.GetComponent<ConfigurableJoint>())
                {
                    erl.GetComponent<Rigidbody>().isKinematic = true;
                    erl.transform.rotation = transform.rotation;// transform.rotation * Quaternion.Inverse(other.transform.rotation);
                    if (!erl.GetComponent<ViveController>())
                    {
                        tr.GetComponent<ConfigurableJoint>().connectedBody = erl.GetComponent<Rigidbody>();
                    }
                        //erl.transform.parent = tr;// this.transform;
                    erl.transform.localPosition = Vector3.zero;
                }
                else
                {
                    erl.GetComponent<Rigidbody>().AddForce(( transform.position - m_previousPos) * 10);

                    //m_previousPos = transform.position;
                }
            }
            else
            {
                m_animator.SetBool("grab", false);

                if (!erl.GetComponent<ConfigurableJoint>())
                {
                    erl.GetComponent<Rigidbody>().isKinematic = false;
                    if(!erl.GetComponent<ViveController>())
                        erl.transform.parent = null;
                    erl = null;
                }
            }
        }
    }
}
