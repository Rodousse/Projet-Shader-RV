using UnityEngine;
using System.Collections;
using Valve.VR;
[RequireComponent(typeof(SteamVR_TrackedController))]
public class ViveController : MonoBehaviour
{
    public float Range = 2500;
    private SteamVR_TrackedController controller;
	// Use this for initialization
	void OnEnable ()
    {
        controller = GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked += HandleTriggerEvent;
        controller.TriggerUnclicked += HandleTriggerEventRelease;
	}
    [SerializeField]
    GameObject erl;


    bool isHandled = false;
    void HandleTriggerEvent(object sender, ClickedEventArgs e)
    {
        isHandled = true;
    }

    void HandleTriggerEventRelease(object sender, ClickedEventArgs e)
    {
        isHandled = false;
    }
    bool isInContact = false;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pots"))
        {
            isInContact = true;
            erl = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Pots"))
        {
            isInContact = false;
        }
    }
    void Update()
    {
        if(isHandled==false)
        {
            if(erl)
            {
                erl.GetComponent<Rigidbody>().isKinematic = false;
                erl.transform.parent = null;
                erl = null;
            }
        }
        else
        {
            if(isInContact)
            {
                erl.transform.position = this.transform.localPosition;
                erl.GetComponent<Rigidbody>().isKinematic = true;
                erl.transform.rotation = transform.rotation;// transform.rotation * Quaternion.Inverse(other.transform.rotation);
                erl.transform.parent = this.transform;
            }
        }
    }
}
