using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class FogBounds : MonoBehaviour
{
    public Volume fogVolume;

    public float maxFogDensity;
    public float minFogDensity;
    public float fogSpeed;

    private bool outsideBounds;

    private void Start()
    {
        VolumeProfile profile = fogVolume.sharedProfile;
        if (profile.TryGet<Fog>(out var fog))
        {
            fog.meanFreePath.value = 400;
        }
    }

    private void Update()
    {
        if (outsideBounds)
        {
            VolumeProfile profile = fogVolume.sharedProfile;
            if (profile.TryGet<Fog>(out var fog))
            {
                if (fog.meanFreePath.value > minFogDensity)
                {
                    fog.meanFreePath.value -= fogSpeed;
                }
            }
        }
        else
        {
            VolumeProfile profile = fogVolume.sharedProfile;
            if (profile.TryGet<Fog>(out var fog))
            {
                if (fog.meanFreePath.value < maxFogDensity)
                {
                    fog.meanFreePath.value += fogSpeed;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        outsideBounds = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        outsideBounds = false;
    }
}
