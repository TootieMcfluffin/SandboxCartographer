using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePlaceableStuff : MonoBehaviour {

    public InputField nameBox;
    public InputField notesBox;

    private CameraDrag cameraScript;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SaveChanges()
    {
        cameraScript = GameObject.Find("Main Camera").GetComponent<CameraDrag>();
        GameObject text = cameraScript.modelToName[DataHolder.SelectedObject];
        text.GetComponent<TextMesh>().text = nameBox.text;
        cameraScript.nameToNotes[text] = notesBox.text;
    }
}
