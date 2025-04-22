using UnityEngine;

[CreateAssetMenu(fileName = "TectacleSO", menuName = "Scriptable Objects/TectacleSO")]
public class TectacleSO : ScriptableObject
{
    public LineRendererSettings lineRendererSettings;
    public int tentacleCount = 5;
    
}
