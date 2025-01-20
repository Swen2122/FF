using System.Collections;
using UnityEngine;

public static class WaterEffects
{
    public static void CreateWaterExplosion(this WaterSystem water, Vector2 position, float radius = 2f, float amount = 0.5f, float pressure = 3f, int points = 8)
    {
        for (int i = 0; i < points; i++)
        {
            float angle = i * Mathf.PI * 2f / points;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            water.AddWater(position + offset, amount, pressure);
        }
    }

    public static IEnumerator CreateWaterStream(this WaterSystem water, Vector2 position, float duration, float ratePerSecond = 10f, float amount = 0.1f, float pressure = 1f)
    {
        float timer = 0f;
        while (timer < duration)
        {
            water.AddWater(position, amount, pressure);
            yield return new WaitForSeconds(1f / ratePerSecond);
            timer += 1f / ratePerSecond;
        }
    }

    public static void CreateWaterSplash(this WaterSystem water, Vector2 position, Vector2 direction, float spread = 30f, int drops = 5)
    {
        for (int i = 0; i < drops; i++)
        {
            float angle = Random.Range(-spread, spread);
            Vector2 rotatedDir = Quaternion.Euler(0, 0, angle) * direction;
            water.AddWater(position + rotatedDir.normalized * Random.Range(0f, 1f),
                          amount: Random.Range(0.1f, 0.3f),
                          pressure: Random.Range(1f, 2f));
        }
    }
}
