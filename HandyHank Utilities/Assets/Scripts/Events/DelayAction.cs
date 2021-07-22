using System;
using System.Collections.Generic;

public static class DelayedAction
{
    private class DelayItem
    {
        public Action Action;
        public float Delay;
        public float Tick;
    }

    private static readonly List<DelayItem> Query = new List<DelayItem>();

    public static void Queue(Action action, float seconds)
    {
        Query.Add(new DelayItem { Action = action, Delay = seconds * 1000, Tick = Environment.TickCount });
    }

    public static void Update()
    {
        for (int i = 0; i < Query.Count; i++)
        {
            if (Environment.TickCount - Query[i].Tick <= Query[i].Delay)
                continue;

            Query[i].Action();
            Query.RemoveAt(i);

            if (Query.Count <= 0)
                break;

        }
    }
}
