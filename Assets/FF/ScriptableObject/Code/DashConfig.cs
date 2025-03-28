using UnityEngine;

[CreateAssetMenu(fileName = "DashConfig", menuName = "FF/Movement/DashConfig")]
public class DashConfig : ScriptableObject
{
    public float dashCooldown;
    public float dashDistance;
    public float dashSpeed;
    
}
