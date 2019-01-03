using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderStartup : MonoBehaviour {

    public Slider slide;

    private float min = 0;
    private float max = 1;
    private TerrainGenerator tg;
    // Use this for initialization
    void Start () {
        tg = (TerrainGenerator)FindObjectOfType(typeof(TerrainGenerator));
	}
	
	// Update is called once per frame
	void Update () {
        if(tg == null)
        {
            tg = (TerrainGenerator)FindObjectOfType(typeof(TerrainGenerator));
        }
        else
        {
            if(tg.MaxHeight != max)
            {
                max = tg.MaxHeight;
                slide.maxValue = max;
            }
            if(tg.MinHeight != min)
            {
                min = tg.MinHeight;
                slide.minValue = min;
                slide.value = (max + min) / 2;
            }
        }
    }

    private void OnGUI()
    {
        
    }
}
