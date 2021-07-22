using System;
using UnityEngine;

[ExecuteInEditMode]
public class GameTimeManager : MonoBehaviour 
{
    public bool Paused;

    public float TimeMultiplier   = 1.00f;
    public float SecondsInFullDay = 60.0f * 24.0f; //60 * 60 * 24;

    [Range(0, 1)]
    public float CurrentTimeOfDay = 0.5f;

    [Space]
    [TextArea]
    public string Description;

    public GameTime InGameTime = new GameTime();

    private GameObject[] lightSources;
    private Sun sunComponent;

    private void Start()
    {
        this.lightSources = GameObject.FindGameObjectsWithTag("LightSource");
        this.sunComponent = GameObject.FindGameObjectWithTag("Sun").GetComponent<Sun>();

        this.SetTimeOfDay();
        this.SetTimeCycle();
    }

    private void Update() 
    {
        if (!this.Paused)
        {
            this.SetTimeOfDay();
            this.SetTimeCycle();
        }
    }

    private void SetTimeOfDay()
    {
        this.CurrentTimeOfDay += Time.deltaTime / this.SecondsInFullDay * this.TimeMultiplier;

        this.InGameTime.Hours   = (int)(this.CurrentTimeOfDay * 24.0f);
        this.InGameTime.Minutes = (int)(this.CurrentTimeOfDay * 60.0f);
        this.InGameTime.Seconds = (int)(this.CurrentTimeOfDay * 60.0f * 60.0f) % 60;

        if (this.InGameTime.Seconds > 60)
        {
            this.InGameTime.Minutes++;
            this.InGameTime.Seconds = 0;
        }

        if (this.InGameTime.Minutes > 60)
        {
            this.InGameTime.Hours++;
            this.InGameTime.Minutes = 0;
            this.InGameTime.Seconds = 1;
        }

        if (this.CurrentTimeOfDay > 1)
        {
            this.CurrentTimeOfDay = 0;
            this.InGameTime.Days++;

            if (this.InGameTime.Days > 30)
            {
                this.InGameTime.Days = 0;
                this.InGameTime.Months++;

                if (this.InGameTime.Months > 12)
                {
                    this.InGameTime.Years++;
                    this.InGameTime.Months = 0;
                }
            }
        }

        this.Description = this.InGameTime.ToString();

        this.InGameTime.TimeClamped = this.CurrentTimeOfDay;

        this.sunComponent.TimeUpdated(this.InGameTime);
    }

    private void SetTimeCycle()
    {
        foreach (var go in this.lightSources)
        {
            if (go)
            {
                var lightBase = go.GetComponent<LightBase>();
                if (lightBase)
                {
                    lightBase.TimeUpdated(this.InGameTime);
                }
            }
        }
    }
}