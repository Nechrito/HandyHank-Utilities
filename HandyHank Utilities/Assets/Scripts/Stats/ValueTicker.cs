using UnityEngine;
using UnityEngine.Events;

public class ValueTicker
{
    public UnityEvent<float> OnTick;
    public UnityEvent<ValueTicker> OnFinished;

    public bool IsTicking => !IsFinished && this.tickCount < this.TotalTicks;
    public bool IsFinished;

    public float AmountPerTick { get; set; }
    public int   TotalTicks    { get; set; }
    public float Correction    { get; set; } = 1.25f;
    public float Delay         { get; set; } = 0.50f;

    private int tickCount;
    private float previousTickTime;

    public ValueTicker()
    {
        this.OnTick     = new UnityEvent<float>();
        this.OnFinished = new UnityEvent<ValueTicker>();

        this.previousTickTime = Time.realtimeSinceStartup;
    }

    public void Update()
    {
        if (this.IsFinished)
            return;

        if (!this.IsTicking)
            return;

        if (Time.realtimeSinceStartup - this.previousTickTime > this.Delay)
        {
            this.tickCount++;
            this.previousTickTime = Time.realtimeSinceStartup;

            this.OnTick?.Invoke(this.AmountPerTick * Random.Range(1.0f, this.Correction));

            if (!this.IsTicking)
            {
                this.IsFinished = true;
                this.OnFinished?.Invoke(this);
            }
        }
    }
}
