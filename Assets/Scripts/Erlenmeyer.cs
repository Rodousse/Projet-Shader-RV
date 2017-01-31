using UnityEngine;
using System.Collections;
public class Erlenmeyer : MonoBehaviour
{
    [SerializeField]
    [Range(0,3)]
    private int ShaderNb;
    // Use this for initialization
    bool isActivated = false;

    public bool canBeUsed = false;

    System.Action callback;

    void Start ()
    {
        callback = CallbackOnUse;
	}
	
    void CallbackOnUse()
    {
        isActivated = false;
        canBeUsed = false;
    }

    [SerializeField]
    Camera cam;
    void activate()
    {
        if (!isActivated)
        {
            Debug.Log("activation");
            switch (ShaderNb)
            {
                case (0):
                    cam.GetComponent<ChromaticAberration>().Activate(callback);
                    break;
                case (1):
                    cam.GetComponent<Hue_Shifting>().Activate(callback);
                    break;
                case (2):
                    cam.GetComponent<MotionBlur>().Activate(callback);
                    break;
                case (3):
                    cam.GetComponent<UnityStandardAssets.ImageEffects.Noise>().Activate(callback);
                    break;

            }
            isActivated = true;
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MainCamera") && canBeUsed)
        {
            activate();
        }
    }
}
