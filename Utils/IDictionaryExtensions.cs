namespace Utils;

public static class IDictionaryExtensions
{
    public static void IncreaseOrAdd<T>(this IDictionary<T, int> source, T key, int count = 1)
    {
        if (source.TryGetValue(key, out _))
        {
            source[key] += count;
        }
        else
        {
            source.Add(key, count);
        }
    }

    public static void AddToListOrAdd<TKey, TValue>(this IDictionary<TKey, List<TValue>> source, TKey key, TValue value)
    {
        if (source.TryGetValue(key, out _))
        {
            source[key].Add(value);
        }
        else
        {
            source.Add(key, new List<TValue> { value });
        }
    }
}