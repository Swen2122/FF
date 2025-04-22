using UnityEngine;
using UnityEngine.Rendering.Universal;
public class EnergyActivation : ElectricConsumer
{
    public GameObject[] objects;
    public SpriteRenderer sprite;  
    protected override void PowerAction()
    {
        sprite.color = Color.green;
        foreach(GameObject obj in objects)
        {
            IActivate activate = obj.GetComponent<IActivate>();
            if(activate != null)activate.On();
        }
    }
    protected override void PowerOverload()
    {
        foreach(GameObject obj in objects)
        {
            IActivate activate = obj.GetComponent<IActivate>();
            if(activate != null)activate.Off();
        }
    }
}
