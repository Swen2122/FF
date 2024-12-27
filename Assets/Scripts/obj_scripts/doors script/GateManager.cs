using UnityEngine;

public class GateManager : MonoBehaviour
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
}
