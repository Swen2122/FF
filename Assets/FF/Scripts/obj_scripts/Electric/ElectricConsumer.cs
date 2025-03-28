using UnityEngine;

public abstract class ElectricConsumer : MonoBehaviour, IElectric
{
    public float power = 0;
    public float minPower = 0;
    public float maxPower = 100;
    public void UpdatePower(float power)
    {
        this.power = power;
        if(power >= minPower)PowerAction();
        if(power > maxPower)PowerOverload();
    }
    protected abstract void PowerAction();
    protected abstract void PowerOverload();
}
