using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnergyLight : ElectricConsumer
{
    private Light2D light2d;
    private void Awake() 
    {
        light2d = GetComponent<Light2D>();
    }

    protected override void PowerAction()
    {
        light2d.intensity = power/10;
        light2d.enabled = true;
    }

    protected override void PowerOverload()
    {
        light2d.enabled = false;
    }
}
