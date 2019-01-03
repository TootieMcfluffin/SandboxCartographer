using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHolder {

    //MAP GENERATION
    public static int PointCount { get; set; }
    public static int MapWidth { get; set; }
    public static int MapHeight { get; set; }
    public static float ElevationScale { get; set; }
    public static float SampleSize { get; set; }
    public static float FrequencyBase { get; set; }
    public static int Octaves { get; set; }

    //TREE GENERATION
    public static int ForestNumber { get; set; }
    public static int AverageTrees { get; set; }
    public static float ForestRadius { get; set; }
    public static float TreeVariance { get; set; }

    //PLACEABLES STUFF
    public static string PlaceableName { get; set; }
    public static bool ShowNames { get; set; }
    public static GameObject SelectedObject { get; set; }
    public static int BrushRadius { get; set; }
}
