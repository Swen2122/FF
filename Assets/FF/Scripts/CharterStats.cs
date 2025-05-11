using UnityEngine;

public class CharterStats : MonoBehaviour
{
    public StatsData statsData;
    public Controler controler;
    public Health health;
    public float damageModifier = 1f;
    public float speedModifier = 1f;
    void Awake()
    {
        if (statsData == null && controler == null)
        {
            SetSpeedModifier();
        }
    }
    public void SetSpeedModifier(float speed = 1f)
    {
        controler.runSpeed = statsData.speed * speed;
    }
}
