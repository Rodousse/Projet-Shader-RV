using UnityEngine;

public class LightManager : IActivable
{
    [SerializeField, Tooltip("0 : Off, 1 : On")]
    Material[] m_materials;
    [SerializeField, Tooltip("Les lumières à éteindre")]
    GameObject[] m_lights;

    sealed protected override void Refresh()
    {
        foreach (GameObject light in m_lights)
        {
            print(light.GetComponent<MeshRenderer>());


            light.GetComponent<MeshRenderer>().sharedMaterial = m_materials[m_enabled ? 0 : 1];


            light.GetComponentInChildren<Light>().enabled = m_enabled;
        }
    }
}
