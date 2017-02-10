using UnityEngine;
using System.Collections;

public class ColliderFollow : MonoBehaviour {

    [SerializeField]
    Transform m_camera;

    CapsuleCollider m_collider;
    
	void Start () {
        m_collider = GetComponent<CapsuleCollider>();
    }
	
	void Update () {
        m_collider.center = new Vector3(m_camera.transform.localPosition.x, 0.6f, m_camera.transform.localPosition.z);
    }
}
