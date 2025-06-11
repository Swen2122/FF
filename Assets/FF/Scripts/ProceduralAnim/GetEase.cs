using UnityEngine;
public static class GetEase
{
    public static float Linear(float x)
    {
        return x;
    }

    public static float EaseInSine(float x)
    {
        return 1f - Mathf.Cos((x * Mathf.PI) / 2f);
    }

    public static float EaseOutSine(float x)
    {
        return Mathf.Sin((x * Mathf.PI) / 2f);
    }

    public static float EaseInOutSine(float x)
    {
        return -(Mathf.Cos(Mathf.PI * x) - 1f) / 2f;
    }

    public static float EaseInQuad(float x)
    {
        return x * x;
    }

    public static float EaseOutQuad(float x)
    {
        return 1f - (1f - x) * (1f - x);
    }

    public static float EaseInOutQuad(float x)
    {
        return x < 0.5f ? 2f * x * x : 1f - Mathf.Pow(-2f * x + 2f, 2f) / 2f;
    }

    public static float EaseInCubic(float x)
    {
        return x * x * x;
    }

    public static float EaseOutCubic(float x)
    {
        return 1f - Mathf.Pow(1f - x, 3f);
    }

    public static float EaseInOutCubic(float x)
    {
        return x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
    }

    public static float EaseInQuart(float x)
    {
        return x * x * x * x;
    }

    public static float EaseOutQuart(float x)
    {
        return 1f - Mathf.Pow(1f - x, 4f);
    }

    public static float EaseInOutQuart(float x)
    {
        return x < 0.5f ? 8f * x * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 4f) / 2f;
    }

    public static float EaseInQuint(float x)
    {
        return x * x * x * x * x;
    }

    public static float EaseOutQuint(float x)
    {
        return 1f - Mathf.Pow(1f - x, 5f);
    }

    public static float EaseInOutQuint(float x)
    {
        return x < 0.5f ? 16f * x * x * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 5f) / 2f;
    }

    public static float EaseInExpo(float x)
    {
        return x == 0f ? 0f : Mathf.Pow(2f, 10f * x - 10f);
    }

    public static float EaseOutExpo(float x)
    {
        return x == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * x);
    }

    public static float EaseInOutExpo(float x)
    {
        if (x == 0f) return 0f;
        if (x == 1f) return 1f;
        return x < 0.5f
            ? Mathf.Pow(2f, 20f * x - 10f) / 2f
            : (2f - Mathf.Pow(2f, -20f * x + 10f)) / 2f;
    }

    public static float EaseInCirc(float x)
    {
        return 1f - Mathf.Sqrt(1f - Mathf.Pow(x, 2f));
    }

    public static float EaseOutCirc(float x)
    {
        return Mathf.Sqrt(1f - Mathf.Pow(x - 1f, 2f));
    }

    public static float EaseInOutCirc(float x)
    {
        return x < 0.5f
            ? (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * x, 2f))) / 2f
            : (Mathf.Sqrt(1f - Mathf.Pow(-2f * x + 2f, 2f)) + 1f) / 2f;
    }

    public static float EaseOutElastic(float x)
    {
        float c4 = (2f * Mathf.PI) / 3f;
        if (x == 0f) return 0f;
        if (x == 1f) return 1f;
        return Mathf.Pow(2f, -10f * x) * Mathf.Sin((x * 10f - 0.75f) * c4) + 1f;
    }
    public static float EaseOutBounce(float x)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;
        if (x < 1f / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2f / d1)
        {
            x -= 1.5f / d1;
            return n1 * x * x + 0.75f;
        }
        else if (x < 2.5f / d1)
        {
            x -= 2.25f / d1;
            return n1 * x * x + 0.9375f;
        }
        else
        {
            x -= 2.625f / d1;
            return n1 * x * x + 0.984375f;
        }
    }

    public static float EaseInBounce(float x)
    {
        return 1f - EaseOutBounce(1f - x);
    }

    public static float EaseInOutBounce(float x)
    {
        return x < 0.5f
            ? (1f - EaseOutBounce(1f - 2f * x)) / 2f
            : (1f + EaseOutBounce(2f * x - 1f)) / 2f;
    }

    public static float EaseInOutElastic(float x)
    {
        float c5 = (2f * Mathf.PI) / 4.5f;
        if (x == 0f) return 0f;
        if (x == 1f) return 1f;
        return x < 0.5f
            ? -(Mathf.Pow(2f, 20f * x - 10f) * Mathf.Sin((20f * x - 11.125f) * c5)) / 2f
            : (Mathf.Pow(2f, -20f * x + 10f) * Mathf.Sin((20f * x - 11.125f) * c5)) / 2f + 1f;
    }
}
