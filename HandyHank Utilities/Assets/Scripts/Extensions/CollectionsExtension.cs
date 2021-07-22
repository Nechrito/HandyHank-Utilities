using System.Collections.Generic;
using System.Linq;
using System;

public static class CollectionsExtension
{
    public static T[] RemoveAt<T>(this T[] array, int index)
    {
        if (index < 0)
        {
            return array;
        }

        if (index >= array.Length)
        {
            return array;
        }

        var newArray = new T[array.Length - 1];
        int index1 = 0;
        for (int index2 = 0; index2 < array.Length; ++index2)
        {
            if (index2 == index)
            {
                continue;
            }

            newArray[index1] = array[index2];
            ++index1;
        }

        return newArray;
    }

    public static T[] InsertAt<T>(this T[] array, int index)
    {
        if (index < 0)
        {
            return array;
        }

        if (index > array.Length)
        {
            return array;
        }

        var newArray = new T[array.Length + 1];
        int index1 = 0;
        for (int index2 = 0; index2 < newArray.Length; ++index2)
        {
            if (index2 == index) continue;

            newArray[index2] = array[index1];
            ++index1;
        }

        return newArray;
    }

    public static T GetRandom<T>(this T[] collection)
    {
        return collection[UnityEngine.Random.Range(0, collection.Length)];
    }

    public static T GetRandom<T>(this IList<T> collection)
    {
        return collection[UnityEngine.Random.Range(0, collection.Count)];
    }
    
    public static T GetRandom<T>(this IEnumerable<T> collection)
    {
        return collection.ElementAt(UnityEngine.Random.Range(0, collection.Count()));
    }

    public static T[] GetRandomCollection<T>(this IList<T> collection, int amount)
    {
        if (amount > collection.Count)
        {
            return null;
        }

        var randoms = new T[amount];
        var indexes = Enumerable.Range(0, amount).ToList();

        for (var i = 0; i < amount; i++)
        {
            var random = UnityEngine.Random.Range(0, indexes.Count);
            randoms[i] = collection[random];
            indexes.RemoveAt(random);
        }

        return randoms;
    }

    public static bool IsNullOrEmpty<T>(this T[] collection)
    {
        if (collection == null) 
            return true;

        return collection.Length == 0;
    }

    public static bool IsNullOrEmpty<T>(this IList<T> collection)
    {
        if (collection == null) 
            return true;

        return collection.Count == 0;
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        if (collection == null) 
            return true;

        return !collection.Any();
    }

    public static int IndexOfItem<T>(this IEnumerable<T> collection, T item)
    {
        if (collection == null)
        {
            return -1;
        }

        var index = 0;
        foreach (var i in collection)
        {
            if (Equals(i, item)) 
                return index;

            ++index;
        }

        return -1;
    }

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
    {
        if (!source.ContainsKey(key))
        {
            source[key] = value;
        }
        return source[key];
    }

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TValue> valueFactory)
    {
        if (!source.ContainsKey(key))
        {
            source[key] = valueFactory(key);
        }
        return source[key];
    }

    public static TValue GetOrAdd<TKey, TValue, TArg>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument)
    {
        if (!source.ContainsKey(key))
        {
            source[key] = valueFactory(key, factoryArgument);
        }
        return source[key];
    }

    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var element in source)
        {
            action(element);
        }
        return source;
    }

    public static IEnumerable<T> ForEach<T, R>(this IEnumerable<T> source, Func<T, R> func)
    {
        foreach (var element in source)
        {
            func(element);
        }
        return source;
    }

    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
    {
        int index = 0;
        foreach (var element in source)
        {
            action(element, index);
            ++index;
        }

        return source;
    }

    public static IEnumerable<T> ForEach<T, R>(this IEnumerable<T> source, Func<T, int, R> func)
    {
        int index = 0;
        foreach (var element in source)
        {
            func(element, index);
            ++index;
        }

        return source;
    }

    public static T MaxBy<T, S>(this IEnumerable<T> source, Func<T, S> selector) where S : IComparable<S>
    {
        if (source.IsNullOrEmpty())
        {
            return default;
        }

        return source.Aggregate((e, n) => selector(e).CompareTo(selector(n)) > 0 ? e : n);
    }

   
    public static T MinBy<T, S>(this IEnumerable<T> source, Func<T, S> selector) where S : IComparable<S>
    {
        if (source.IsNullOrEmpty())
        {
            return default;
        }

        return source.Aggregate((e, n) => selector(e).CompareTo(selector(n)) < 0 ? e : n);
    }

    public static IEnumerable<T> SingleToEnumerable<T>(this T source) => Enumerable.Empty<T>().Append(source);

    public static int FirstIndex<T>(this IList<T> source, Predicate<T> predicate)
    {
        for (int i = 0; i < source.Count; ++i)
        {
            if (predicate(source[i])) 
                return i;
        }

        return -1;
    }

    public static int FirstIndex<T>(this IEnumerable<T> source, Predicate<T> predicate)
    {
        int index = 0;
        foreach (var e in source)
        {
            if (predicate(e)) 
                return index;

            ++index;
        }

        return -1;
    }

    public static int LastIndex<T>(this IList<T> source, Predicate<T> predicate)
    {
        for (int i = source.Count - 1; i >= 0; --i)
        {
            if (predicate(source[i])) return i;
        }

        return -1;
    }

    public static int GetWeightedRandomIndex<T>(this IEnumerable<T> source, Func<T, double> weightSelector)
    {
        var weights = source.Select(weightSelector).Select(w => w < 0 ? 0 : w);
        var weightStages = weights.Select((w, i) => weights.Take(i + 1).Sum());
        var roll = new System.Random().NextDouble() * weights.Sum();
        return weightStages.FirstIndex(ws => ws > roll);
    }

    public static T GetWeightedRandom<T>(this IList<T> source, Func<T, double> weightSelector) => source[source.GetWeightedRandomIndex(weightSelector)];

    public static T GetWeightedRandom<T>(this IEnumerable<T> source, Func<T, double> weightSelector) => source.ElementAt(source.GetWeightedRandomIndex(weightSelector));
}
