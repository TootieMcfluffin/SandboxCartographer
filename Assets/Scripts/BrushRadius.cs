using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushRadius : MonoBehaviour {

    public float Radius { get; set; }
    public Text box;
    public Slider radSlider;

    // Use this for initialization
    void Start () {
        box.text = 10.ToString();
        radSlider.value = 10;
        Radius = 10;
    }

    // Update is called once per frame
    void Update () {
        box.text = Radius.ToString();
        DataHolder.BrushRadius = int.Parse(Radius.ToString());
	}
}
