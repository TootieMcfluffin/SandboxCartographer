using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeGeneration : MonoBehaviour {

    public InputField treeSites;
    public InputField treeNumber;
    public InputField radius;
    public InputField variance;
    public GameObject terrain;

    private TerrainGenerator tg;

    // Use this for initialization
    void Start () {
        tg = terrain.GetComponent<TerrainGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Generate()
    {
        if(treeSites.text == "")
        {
            DataHolder.ForestNumber = 5;
        }
        else
        {
            int sites = 0;
            int.TryParse(treeSites.text, out sites);
            DataHolder.ForestNumber = sites;
        }

        if (treeNumber.text == "")
        {
            DataHolder.AverageTrees = 1000;
        }
        else
        {
            int sites = 0;
            int.TryParse(treeNumber.text, out sites);
            DataHolder.AverageTrees = sites;
        }

        if (radius.text == "")
        {
            DataHolder.ForestRadius = 100;
        }
        else
        {
            float sites = 0;
            float.TryParse(radius.text, out sites);
            DataHolder.ForestRadius = sites;
        }

        if (variance.text == "")
        {
            DataHolder.TreeVariance = 0.3f;
        }
        else
        {
            float sites = 0;
            float.TryParse(variance.text, out sites);
            DataHolder.TreeVariance = sites;
        }
        tg.PlaceTrees();
    }
}
