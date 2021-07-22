using UnityEngine;

public static class Tween
{
    public static float EaseInOutSine(float start, float end, float current)
    {
        end -= start;
        return -end / 2 * (Mathf.Cos(Mathf.PI * current / 1) - 1) + start;
    }
}
