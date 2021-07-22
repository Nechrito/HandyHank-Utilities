using System;

public class GameTime
{
    public float TimeClamped { get; set; }

    public float NightEndTime = 0.15f;
    public float NightStartTime = 0.75f;

    public bool IsDay   => this.TimeClamped > this.NightEndTime && this.TimeClamped < this.NightStartTime;
    public bool IsNight => !this.IsDay;

    public int Years { get; set; }  = DateTime.UtcNow.Year;
    public int Months { get; set; } = DateTime.UtcNow.Month;
    public int Days { get; set; }   = DateTime.UtcNow.Day;

    public int Hours   { get; set; }
    public int Minutes { get; set; }
    public int Seconds { get; set; }

     public override string ToString()
    {
        int year   = this.Years;
        int month  = this.Months;
        int day    = this.Days;
        int hour   = this.Hours;
        int minute = this.Minutes;
        int seconds= this.Seconds;
        //var monthInDetail = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        return $"{day}\\{month}-{year}" + $"\n{hour}h {minute}m {seconds}s";
    }
}