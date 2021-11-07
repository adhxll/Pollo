using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Operation
{
    private const byte size = sizeof(float);

    public static float[] FastCopy(this float[] source)
    {
        var destination = new float[source.Length];
        var length = source.Length * size;

        Buffer.BlockCopy(source, 0, destination, 0, length);
        return destination;
    }
}
