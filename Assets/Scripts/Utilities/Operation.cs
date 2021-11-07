using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Operation
{
    public static float[] FastCopy(this float[] source)
    {
        var destination = new float[source.Length];
        var size = sizeof(float);
        Buffer.BlockCopy(source, 0, destination, 0, source.Length * size);
        return destination;
    }
}
