using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    List<float> priorityHeap = new List<float>();
    List<T> tHeap = new List<T>();

    const float EPSILON = 0.0000001f;
    int counter = 0;

    public int Count {
        get {return priorityHeap.Count;}
    }

    public void Enqueue(T thing, int priority)
    {
        // put it at the end of the heap, and bubble up
        priorityHeap.Add(priority + EPSILON * counter);
        counter++;
        tHeap.Add(thing);

        int pos = priorityHeap.Count - 1;
        while (priorityHeap[pos] < priorityHeap[pos / 2])
        {
            pos = HeapSwapUp(pos);
        }

        // Debug.Log(string.Join(", ", priorityHeap.ToArray()));
    }

    public T Dequeue()
    {
        // Remember the most prioritized
        T thingy = tHeap[0];
        
        // Move the least prioritized over the most prioritized
        priorityHeap[0] = priorityHeap[priorityHeap.Count - 1];
        tHeap[0] = tHeap[priorityHeap.Count - 1];
        tHeap.RemoveAt(priorityHeap.Count - 1);
        priorityHeap.RemoveAt(priorityHeap.Count - 1);

        int pos = 0;
        while ((priorityHeap.Count > 2 * pos + 1 && priorityHeap[pos] > priorityHeap[2 * pos + 1]) || (priorityHeap.Count > 2 * pos + 2 && priorityHeap[pos] > priorityHeap[2 * pos + 2]))
        {
            pos = HeapSwapDown(pos);
        }

        // Debug.Log(string.Join(", ", priorityHeap.ToArray()));
        return thingy;
    }

    // public void Remove(T element)
    // {
    //     int pos = tHeap.IndexOf(element);
    //     if (pos != -1) { return; }
        
    // }

    // boring heap stuff
    // private int GetHeapParent(int pos)
    // {
    //     return pos / 2;
    // }

    // private int GetHeapChild(int pos)
    // {
    //     return pos * 2 + 1; // + 1 for the other one.
    // }

    private int HeapSwapUp(int pos)
    {
        Swap(pos, pos / 2);
        return pos / 2;
    }

    private int HeapSwapDown(int pos)
    {
        int child = 2 * pos + 1;
        if (priorityHeap.Count > child + 1 && priorityHeap[child] > priorityHeap[child + 1]) { child++; }
        Swap(pos, child);
        return child;
    }

    private void Swap(int a, int b)
    {
        float thing = priorityHeap[a];
        priorityHeap[a] = priorityHeap[b];
        priorityHeap[b] = thing;
        T thingy = tHeap[a];
        tHeap[a] = tHeap[b];
        tHeap[b] = thingy;
    }
}
