using UnityEngine;
using UnityEngine.Rendering;

public class UnderwaterDepth : MonoBehaviour
{
    [Header("Depth Parameters")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private int depth = 0;

    [Header("Post Processing Volume")]
    [SerializeField] private Volume postProcessingVol;

    [Header("Post Processing Profiles")]
    [SerializeField] private VolumeProfile surface;
    [SerializeField] private VolumeProfile underwater;

    void Update()
    {
        if (mainCamera.position.y < depth)
        {
            EnableEffects(true);
        }
        else
        {
            EnableEffects(false);
        }
    }

    private void EnableEffects(bool active)
    {
        if (active)
        {
            RenderSettings.fog = true;
            postProcessingVol.profile = underwater;
        }
        else
        {
            RenderSettings.fog = false;
            postProcessingVol.profile = surface;
        }
    }
}
