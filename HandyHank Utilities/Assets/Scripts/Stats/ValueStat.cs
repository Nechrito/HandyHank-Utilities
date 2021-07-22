using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class ValueStat
{
    public UnityEvent<EntityBase> OnDepleted { get; set; } = new UnityEvent<EntityBase>();
    public UnityEvent<float> OnValueChange { get; set; } = new UnityEvent<float>();

    [HideInInspector]
    public EntityBase Owner;

    [HideInInspector]
    public Slider Slider;

    public float Value => this.CalculateFinalValue();

    public float MaxValue = 100;
    public float Percentage => this.Value / this.MaxValue * 100;
    public float BaseValue;

    public float LastChangeTick { get; private set; }

    private readonly List<ValueTicker>  tickers = new List<ValueTicker>();
    private readonly List<StatModifier> statModifiers = new List<StatModifier>();

    private bool isDepleted;

    public ValueStat(float amount)
    {
        if (amount > this.MaxValue)
        {
            this.MaxValue = amount;
        }

        this.BaseValue = amount;
    }

    public ValueStat()
    {
        this.BaseValue = this.MaxValue;
    }

    private void OnTick(float tickAmount)
    {
        this.Change(-tickAmount);
    }

    private void OnTickFinished(ValueTicker ticker)
    {
        this.tickers.Remove(ticker);
    }

    public void Update()
    {
        if (this.tickers.Count > 0)
        {
            foreach (var ticker in this.tickers)
            {
                ticker.Update();
            }
        }
    }

    public void Change(float amount)
    {
        this.BaseValue += amount;

        var value = this.Value;

        this.LastChangeTick = Time.realtimeSinceStartup;

        this.OnValueChange?.Invoke(amount);

        if (value > this.MaxValue && amount > 0)
        {
            this.BaseValue = this.MaxValue;
            return;
        }

        if (value <= 0 && !this.isDepleted)
        {
            this.isDepleted = true;
            this.OnDepleted?.Invoke(this.Owner);
        }

        if (this.Slider != null)
        {
            this.Slider.value = this.Value;
        }
    }

    public void AppendTicker(ValueTicker ticker)
    {
        this.tickers.Add(ticker);
        
        ticker.OnFinished.AddListener(OnTickFinished);
        ticker.OnTick.AddListener(OnTick);
    }

    public void AddModifier(StatModifier mod)
    {
        this.statModifiers.Add(mod);
    }

    public bool RemoveModifier(StatModifier mod)
    {
        return this.statModifiers.Remove(mod);
    }

    public float CalculateFinalValue()
    {
        float result = this.BaseValue;
        float percentage = 0;

        for (int i = 0; i < this.statModifiers.Count; i++)
        {
            var mod = this.statModifiers[i];

            switch (mod.Type)
            {
                case StatModType.Flat: 
                    result += mod.value;
                    break;

                case StatModType.PercentMult:
                    result *= 1 + mod.value;
                    break;

                case StatModType.PercentAdd:
                {
                    percentage += mod.value;

                    if (i + 1 >= this.statModifiers.Count || this.statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        result *= 1 + percentage;
                        percentage = 0;
                    }

                    break;
                }
            }
        }

        return (float)Math.Round(result, 4);
    }
}
