using UnityEngine;
using System.Collections;

public class Erlenmeyer : MonoBehaviour {

    [SerializeField]
    [Range(0,2)]
    private int ShaderNb;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void activate()
    {
        switch(ShaderNb)
        {
            case(0):
                Camera.main.GetComponent<ChromaticAberration>().enabled = true;
                break;
            case (1):
                Camera.main.GetComponent<Hue_Shifting>().enabled = true;
                break;
            case (2):
                Camera.main.GetComponent<TrailEffect>().enabled = true;
                break;

        }
    }
}
