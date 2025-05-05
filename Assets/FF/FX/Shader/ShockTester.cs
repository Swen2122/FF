using UnityEngine;

public class ShockTester : MonoBehaviour
{
    public Material shockMaterial;
    private float shockTime;
    public float speed = 1f;
    public float maxTime = 1f;

    private void Update()
    {
        shockTime += Time.deltaTime;
        shockMaterial.SetFloat("_ShockTime", shockTime);
        if (shockTime > maxTime) // Хвиля зникла
        {
            shockTime = 0;
            shockMaterial.SetFloat("_ShockTime", 0);
        }
    }

    public void Initialize(Vector2 center)
    {
        shockMaterial.SetVector("_Center", new Vector4(center.x, center.y, 0, 0));
    }
}

