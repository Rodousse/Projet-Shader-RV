using UnityEngine;
using System.Collections;
public class Erlenmeyer : MonoBehaviour
{
    public enum EffectType
    {
        Noise,
        Hue_shifting,
        ChromaticAberration,
        MotionBlur
    }
    [SerializeField]
    Camera cam;

    [SerializeField]
    EffectType Effect;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("collider");
            switch (Effect)
            {
                case EffectType.Noise:
                    cam.GetComponent<UnityStandardAssets.ImageEffects.Noise>().Activate();
                    break;
                case EffectType.Hue_shifting:
                    cam.GetComponent<Hue_Shifting>().Activate();
                    break;
                case EffectType.ChromaticAberration:
                    cam.GetComponent<ChromaticAberration>().Activate();
                    break;
                case EffectType.MotionBlur:
                    cam.GetComponent<MotionBlur>().Activate();
                    break;
                default:
                    break;
            }
        }
    }
}
