using UnityEngine;
using System.Collections;
public class Erlenmeyer : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MainCamera"))
            cam.GetComponent<ParametricEffect>().Activate();
    }
}
