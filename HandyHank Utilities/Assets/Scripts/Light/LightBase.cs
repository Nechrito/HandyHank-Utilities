using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum FadeDirection
{
    FadeIn  =  1,
    FadeOut = -1,
    NoFade  =  0,
}

public enum LightActiveState
{
    Night,
    Always,
    Day,
}

[ExecuteInEditMode]
public abstract class LightBase : MonoBehaviour
{
    public Gradient LightningColor;

    public LightActiveState LightActiveState;

    public bool RandomizeProperties = true;

    [ShowIf(nameof(RandomizeProperties))]
    public float FadeSpeed = 0.8f;

    protected FadeDirection FadeDirection;

    protected float elapsedTimeActive;
    protected float durationToBeActive = 0.4f;

    protected bool SetColorInternally;
    protected bool IsActive;
    protected Light2D Light2D;
    protected float InitialIntensity;

    private void Start()
    {
        if (this.RandomizeProperties)
        {
            var range = 0.15f;
            this.Light2D.intensity += Random.Range(-range, range);
            this.FadeSpeed =  Random.Range(0.5f, 2.0f);
        }
    }

    protected virtual void Awake()
    {
        this.Light2D = this.GetComponent<Light2D>();
        this.InitialIntensity = this.Light2D.intensity;
    }

    protected virtual void Update()
    {
        if (this.FadeDirection == FadeDirection.NoFade)
        {
            this.IsActive = true;
            return;
        }

        //this.Light2D.intensity += (int)this.FadeDirection * this.FadeSpeed * Time.deltaTime;
        //this.Light2D.intensity = Mathf.Clamp01(this.Light2D.intensity * this.InitialIntensity);

        if (this.Light2D.intensity <= 0 || this.Light2D.intensity >= 1)
        {
            this.elapsedTimeActive  = 0;
            this.IsActive = this.FadeDirection == FadeDirection.FadeIn;
            this.FadeDirection = FadeDirection.NoFade;
        }
    }

    public void Fade(FadeDirection fadeDirection)
    {
        this.FadeDirection = fadeDirection;
    }

    public virtual void UpdateColor(float percentage)
    {
        this.Light2D.color = LightningColor.Evaluate(percentage);
    }

    public virtual void TimeUpdated(GameTime time)
    {
        switch (this.LightActiveState)
        {
            case LightActiveState.Always:
                    this.Light2D.color = this.LightningColor.Evaluate(time.TimeClamped);
                return;
            case LightActiveState.Day:
                this.FadeDirection = time.IsDay 
                    ? FadeDirection.FadeIn
                    : FadeDirection.FadeOut;
                break;
            case LightActiveState.Night:
                this.FadeDirection = time.IsNight 
                    ? FadeDirection.FadeIn
                    : FadeDirection.FadeOut;
                break;
        }

        if (this.SetColorInternally)
        {
            var top        = time.NightEndTime;
            var bottom     = time.NightStartTime;
            var current    = time.TimeClamped;
            var distance   = bottom - top;
            var percentage = Tween.EaseInOutSine(0.0f, 1.0f, current / distance);
            this.UpdateColor(percentage);
        }
    }
}
