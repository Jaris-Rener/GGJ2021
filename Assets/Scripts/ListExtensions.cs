using System.Collections.Generic;

public static class ListExtensions
{
    public static T RandomElement<T>(this IList<T> enumerable)
    {
        return enumerable[UnityEngine.Random.Range(0, enumerable.Count)];
    }
}