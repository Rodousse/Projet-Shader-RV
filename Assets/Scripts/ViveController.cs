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
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Pots"))
        {
            isInContact = true;
            erl = other.gameObject;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            Debug.Log("OpenDoor");
            other.gameObject.GetComponent<Door>().Open();
        }
    }
    void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("Pots") && isHandled == false)
        {
            Debug.Log("exit coll");
            isInContact = false;
        }
    }
    void Update()
    {
        if(isInContact && erl)
        {
            if (isHandled)
            {
                erl.GetComponent<Rigidbody>().isKinematic = true;
                erl.transform.rotation = transform.rotation;// transform.rotation * Quaternion.Inverse(other.transform.rotation);
                erl.transform.parent = this.transform;
                erl.transform.localPosition = Vector3.zero;
                erl.GetComponent<Erlenmeyer>().canBeUsed = true;
            }
            else
            {
                erl.GetComponent<Erlenmeyer>().canBeUsed = false;
                erl.GetComponent<Rigidbody>().isKinematic = false;
                erl.transform.parent = null;
                erl = null;
            }
        }
    }
}
