using UnityEngine;

public class GateManager : MonoBehaviour, IActivate
{
    [SerializeField] private GateManipulator gate;
    private void Update()
    {
        if (Input.GetKeyDown("j"))
        {
            Close();
        }
        if (Input.GetKeyDown("k"))
        {
            Open();
        }
    }
    public void Open()
    {
        gate.OpenGate();
    }
    public void Close()
    {
        gate.CloseGate();
    }
    public void On()
    {
        Open();
    }
    public void Off()
    {
        Close();
    }
}
