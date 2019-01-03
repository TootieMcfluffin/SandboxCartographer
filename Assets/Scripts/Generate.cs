using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Generate : MonoBehaviour {

    public InputField pointCount;
    public InputField width;
    public InputField height;
    public InputField elev;
    public InputField samp;
    public InputField freq;
    public InputField octave;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Generation()
    {
        //Set point value
        if(pointCount.text == "")
        {
            DataHolder.PointCount = 12000;
        }
        else
        {
            int points = 0;
            int.TryParse(pointCount.text.ToString(), out points);
            DataHolder.PointCount = points;
        }

        //Set width value;
        if (width.text == "")
        {
            DataHolder.MapWidth = 800;
        }
        else
        {
            int points = 0;
            int.TryParse(width.text.ToString(), out points);
            DataHolder.MapWidth = points;
        }

        //Set height value;
        if (height.text == "")
        {
            DataHolder.MapHeight = 800;
        }
        else
        {
            int points = 0;
            int.TryParse(height.text.ToString(), out points);
            DataHolder.MapHeight = points;
        }

        //Set elevation scale
        if (elev.text == "")
        {
            DataHolder.ElevationScale = 230;
        }
        else
        {
            float points = 0;
            float.TryParse(elev.text.ToString(), out points);
            DataHolder.ElevationScale = points;
        }

        //Set sample size
        if (samp.text == "")
        {
            DataHolder.SampleSize = 1f;
        }
        else
        {
            float points = 0;
            float.TryParse(samp.text.ToString(), out points);
            DataHolder.SampleSize = points;
        }

        //Set frequency base
        if (freq.text == "")
        {
            DataHolder.FrequencyBase = 2f;
        }
        else
        {
            float points = 0;
            float.TryParse(freq.text.ToString(), out points);
            DataHolder.FrequencyBase = points;
        }

        //Set octaves
        if (octave.text == "")
        {
            DataHolder.Octaves = 4;
        }
        else
        {
            int points = 0;
            int.TryParse(octave.text.ToString(), out points);
            DataHolder.Octaves = points;
        }

        SceneManager.LoadScene("SampleScene");
    }
}
