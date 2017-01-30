using UnityEngine;
using System.Collections;
using Valve.VR;
using System.Collections.Generic;
public class ViveControllerManager : MonoBehaviour
{

    [SerializeField]
    UnityEngine.UI.Text debugField;

    [SerializeField]
    UnityEngine.UI.Text debugField2;

    [SerializeField]
    ViveController m_leftStick;

    [SerializeField]
    ViveController m_rightStick;

    private Vector3 m_rPos;
    private Vector3 m_lPos;


    [SerializeField, Range(0, 10)]
    float m_gapTreshold;

    [SerializeField, Range(0, 2)]
    float m_cycleTimeMax = 2f;
    float m_ct = 0.0f;


    bool startCycle = false;
    bool inverseCycle = false;
    List<Vector3> m_points = new List<Vector3>();

	void Update ()
    {
        m_rPos = m_rightStick.transform.position;
        m_lPos = m_leftStick.transform.position;
        print(m_rPos);
        print(m_lPos);
        print((m_rPos.y - m_lPos.y)>= m_gapTreshold);
        debugField.text=(m_rPos.y - m_lPos.y).ToString();
        if((m_rPos.y-m_lPos.y)>=m_gapTreshold)
        {
            if(!startCycle)
            {
                m_points.Clear();
                m_points = new List<Vector3>();
                startCycle = true;
                inverseCycle = false;
                m_points.Add(m_rPos);
            }
        }

        if(startCycle)
        {
            m_ct += Time.deltaTime;
            if(m_ct>m_cycleTimeMax)
            {
                startCycle = false;
                m_ct = 0;

            }
            else
            {
                if (m_points[m_points.Count - 1].y>m_rPos.y+0.5f)
                {
                    m_ct = 0;
                    startCycle = false;
                    inverseCycle = false;
                    m_points.Clear();
                    return;
                }
                m_points.Add(m_rPos);
                if(m_rPos.y<=m_lPos.y)
                {
                    inverseCycle = true;

                }
                if ((m_lPos.y-m_rPos.y)>=m_gapTreshold && inverseCycle)
                {
                    Run();
                }
            }
        }
    }

    [SerializeField]
    GameObject areaZone;

    void Run()
    {
        var rotation = this.transform.rotation;
        NavMeshHit hit;
        Vector3 nv = Vector3.MoveTowards(transform.position, transform.forward * 0.5f, 0f);
        if(NavMesh.SamplePosition(nv, out hit, 0.1f, NavMesh.AllAreas))
        {
            debugField2.text = nv.ToString();
            Debug.DrawLine(areaZone.transform.position, nv, Color.red, 222f, true);
            areaZone.transform.Translate(nv);
        }

    }
}
