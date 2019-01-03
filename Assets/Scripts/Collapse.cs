using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collapse : MonoBehaviour {

    public Button button;
    public GameObject objectToCollapse;

    private bool isCollapsed = false;

    public bool IsCollapsed
    {
        get { return isCollapsed; }
        set { isCollapsed = value; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CollapseLeft()
    {
        isCollapsed = !isCollapsed;
        if(isCollapsed)
        {
            RectTransform move = objectToCollapse.GetComponent(typeof(RectTransform)) as RectTransform;
            move.transform.localPosition = new Vector3(move.transform.localPosition.x - move.rect.width, move.transform.localPosition.y);
            button.transform.localPosition = new Vector3(button.transform.localPosition.x - move.rect.width, button.transform.localPosition.y);
            button.GetComponentInChildren<Text>().text = ">";
        }
        else
        {
            RectTransform move = objectToCollapse.GetComponent(typeof(RectTransform)) as RectTransform;
            move.transform.localPosition = new Vector3(move.transform.localPosition.x + move.rect.width, move.transform.localPosition.y);
            button.transform.localPosition = new Vector3(button.transform.localPosition.x + move.rect.width, button.transform.localPosition.y);
            button.GetComponentInChildren<Text>().text = "<";
        }
    }

    public void CollapseRight()
    {
        isCollapsed = !isCollapsed;
        if (isCollapsed)
        {
            RectTransform move = objectToCollapse.GetComponent(typeof(RectTransform)) as RectTransform;
            move.transform.localPosition = new Vector3(move.transform.localPosition.x + move.rect.width, move.transform.localPosition.y);
            button.transform.localPosition = new Vector3(button.transform.localPosition.x + move.rect.width, button.transform.localPosition.y);
            button.GetComponentInChildren<Text>().text = "<";
        }
        else
        {
            RectTransform move = objectToCollapse.GetComponent(typeof(RectTransform)) as RectTransform;
            move.transform.localPosition = new Vector3(move.transform.localPosition.x - move.rect.width, move.transform.localPosition.y);
            button.transform.localPosition = new Vector3(button.transform.localPosition.x - move.rect.width, button.transform.localPosition.y);
            button.GetComponentInChildren<Text>().text = ">";
        }
    }
}
