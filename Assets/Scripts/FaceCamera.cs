using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
        transform.LookAt(Camera.main.transform);
        transform.rotation *= Quaternion.Euler(90, 0, 0);
	}
}
