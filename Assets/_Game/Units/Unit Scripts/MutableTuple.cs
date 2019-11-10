using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MutableTuple is exactly what it sounds like: a Tuple-Like object that is mutable
/// </summary>

public class MutableTuple<T1, T2>
{
    public T1 Item1;
    public T2 Item2;

    public MutableTuple(T1 first, T2 second)
    {
        Item1 = first;
        Item2 = second;
    }
}
