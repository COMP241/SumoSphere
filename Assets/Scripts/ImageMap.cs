using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class ImageMap
{
    public int Id;
    public float Ratio;
    public Line[] Lines;

    public static ImageMap FromJson(string json)
    {
        return JsonUtility.FromJson<ImageMap>(json);
    }

    public override string ToString()
    {
        return string.Format("MAP\nid:{0}\nratio:{1}\nLines:\n\t{2}", Id, Ratio, string.Join("\n\t", Lines.Select(l => l.ToString()).ToArray()));
    }
}

[Serializable]
public enum MapColor
{
    Black = 0,
    Red = 1,
    Green = 2,
    Blue = 3,
    Cyan = 4,
    Magenta = 5,
    Yellow = 6
}

[Serializable]
public class Line
{
    public MapColor Color;
    public bool Loop;
    public Point[] Points;

    public override string ToString()
    {
        return string.Format("{0} {1} [{2}]", Color, Loop ? "Looping" : "Open", string.Join(",", Points.Select(p => p.ToString()).ToArray()));
    }
}

[Serializable]
public class Point
{
    public float X;
    public float Y;

    public override string ToString()
    {
        return string.Format("({0}, {1})", X, Y);
    }
}