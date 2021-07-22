using NaughtyAttributes;
using UnityEngine;

[ExecuteInEditMode]
public class LightFire : LightBase
{
    [ShowIf(nameof(RandomizeProperties))]
    public float FireSpeed = 0.5f;

    public AnimationCurve LightIntensityCurve;

    protected override void Awake()
    {
        this.SetColorInternally = true;

        base.Awake();
      
        if (this.RandomizeProperties)
        {
            this.FireSpeed = Random.Range(0.5f, 1.0f);
        }
    }

    protected override void Update()
	{
        base.Update();

        if (!this.IsActive)
        {
            return;
        }

        if (this.FadeDirection == FadeDirection.NoFade || this.LightActiveState == LightActiveState.Always)
        {
            var curveValue = this.LightIntensityCurve.Evaluate(Time.time * this.FireSpeed);

            if (!this.SetColorInternally)
            {
                this.UpdateColor(curveValue);
            }

            if (Application.isPlaying)
            {
                this.Light2D.intensity = this.InitialIntensity * curveValue;
            }
        }
    }
}
