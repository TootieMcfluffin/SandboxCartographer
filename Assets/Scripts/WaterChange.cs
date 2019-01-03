using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterChange : MonoBehaviour {

    public float WaterVal { get; set; }
    public Text box;
    public Slider slide;
    private float min;
    private float max;

    // Use this for initialization
    void Start () {

        min = slide.minValue;
        max = slide.maxValue;
        box.text = (((WaterVal - min) * 100) / (max - min)).ToString("0") + "%";
    }

    // Update is called once per frame
    void Update () {
        if (slide.minValue != min)
        {
            min = slide.minValue;
        }
        if (slide.maxValue != max)
        {
            max = slide.maxValue;
        }
        box.text = (((WaterVal - min) * 100) / (max - min)).ToString("0") + "%";
	}
}
