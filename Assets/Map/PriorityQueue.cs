using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    List<int> intHeap = new List<int>();
    List<T> tHeap = new List<T>();

    public int Count {
        get {return intHeap.Count;}
    }

    public void Enqueue(T thing, int priority)
    {
        // put it at the end of the heap, and bubble up
        intHeap.Add(priority);
        tHeap.Add(thing);

        int pos = intHeap.Count - 1;
        while (intHeap[pos] < intHeap[pos / 2])
        {
            pos = HeapSwapUp(pos);
        }

        Debug.Log(string.Join(", ", intHeap.ToArray()));
    }

    public T Dequeue()
    {
        // Remember the most prioritized
        T thingy = tHeap[0];
        
        // Move the least prioritized over the most prioritized
        intHeap[0] = intHeap[intHeap.Count - 1];
        tHeap[0] = tHeap[intHeap.Count - 1];
        tHeap.RemoveAt(intHeap.Count - 1);
        intHeap.RemoveAt(intHeap.Count - 1);

        int pos = 0;
        while ((intHeap.Count > 2 * pos + 1 && intHeap[pos] > intHeap[2 * pos + 1]) || (intHeap.Count > 2 * pos + 2 && intHeap[pos] > intHeap[2 * pos + 2]))
        {
            pos = HeapSwapDown(pos);
        }

        Debug.Log(string.Join(", ", intHeap.ToArray()));
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
        if (intHeap.Count > child + 1 && intHeap[child] > intHeap[child + 1]) { child++; }
        Swap(pos, child);
        return child;
    }

    private void Swap(int a, int b)
    {
        int thing = intHeap[a];
        intHeap[a] = intHeap[b];
        intHeap[b] = thing;
        T thingy = tHeap[a];
        tHeap[a] = tHeap[b];
        tHeap[b] = thingy;
    }
}
