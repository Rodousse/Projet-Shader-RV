using UnityEngine;

public class LightManager : MonoBehaviour {

    [SerializeField, Tooltip("0 : Off, 1 : On")]
    Material[] m_materials;
    [SerializeField, Tooltip("Les lumières à éteindre")]
    GameObject[] m_lights;

    bool m_state = true;

    public void Switch()
    {
        m_state = !m_state;

        foreach (GameObject light in m_lights)
        {
            light.GetComponent<MeshRenderer>().sharedMaterial = m_materials[m_state ? 0 : 1];
            light.GetComponentInChildren<Light>().enabled = m_state;
        }
    }
}
