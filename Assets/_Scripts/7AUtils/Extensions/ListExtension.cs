using System.Collections.Generic;

public static class ListExtensions
{
    public static T Random<T>(this List<T> list)
    {
        if (list.Count == 0) return default(T);

        return list[UnityEngine.Random.Range(0, list.Count)];
    }


    public static void Shuffle<T>(this List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}