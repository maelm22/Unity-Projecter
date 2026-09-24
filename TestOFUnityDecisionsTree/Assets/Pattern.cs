using UnityEngine;

internal class Pattern
{
    public int frequency;
    public Color[] pixels;
    public int size;

    public Pattern(Color[] pixels, int size)
    {
        this.pixels = pixels;
        this.size = size;
        frequency = 1;
    }

    public bool CheckRightEdge(Pattern neighborPattern)
    {
        for (var i = 0; i < size; i++)
            if (pixels[i * size + (size - 1)] != neighborPattern.pixels[i * neighborPattern.size])
                return false;

        return true;
    }

    public bool CheckLeftEdge(Pattern neighborPattern)
    {
        for (var i = 0; i < size; i++)
            if (pixels[i * size] != neighborPattern.pixels[i * neighborPattern.size + (neighborPattern.size - 1)])
                return false;

        return true;
    }

    public bool CheckTopEdge(Pattern neighborPattern)
    {
        for (var i = 0; i < size; i++)
            if (pixels[(size - 1) * size + i] != neighborPattern.pixels[i])
                return false;

        return true;
    }

    public bool CheckBottomEdge(Pattern neighborPattern)
    {
        for (var i = 0; i < size; i++)
            if (pixels[i] != neighborPattern.pixels[(neighborPattern.size - 1) * neighborPattern.size + i])
                return false;

        return true;
    }
}