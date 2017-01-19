using UnityEngine;
using System.Collections;

public class Erlenmeyer : MonoBehaviour {

    [SerializeField]
    [Range(0,3)]
    private int ShaderNb;
    // Use this for initialization
    bool isActivated = false;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
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
                    cam.GetComponent<ChromaticAberration>().isActive = true;
                    break;
                case (1):
                    cam.GetComponent<Hue_Shifting>().isActive = true;
                    break;
                case (2):
                    cam.GetComponent<MotionBlur>().isActive = true;
                    break;
                case (3):
                    cam.GetComponent<UnityStandardAssets.ImageEffects.Noise>().isActive = true;
                    break;

            }
            isActivated = true;
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MainCamera"))
        {
            activate();
        }
    }
}
