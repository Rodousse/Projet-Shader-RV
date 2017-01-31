using UnityEngine;
using System.Collections;

public abstract class IActivable : MonoBehaviour {

    [SerializeField]
    protected bool m_enabled = false;

    void OnValidate()
    {
        if(Time.frameCount > 0)
            Refresh();
    }

    protected abstract void Refresh();

    public void Switch()
    {
        m_enabled = !m_enabled;
        Refresh();
    }
}
