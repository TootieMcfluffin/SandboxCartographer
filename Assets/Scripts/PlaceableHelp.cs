using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableHelp : MonoBehaviour {

    public void ChangeFir()
    {
        DataHolder.PlaceableName = "Fir";
    }

    public void ChangeOak()
    {
        DataHolder.PlaceableName = "Oak";
    }

    public void ChangePalm()
    {
        DataHolder.PlaceableName = "Palm";
    }

    public void ChangePoplar()
    {
        DataHolder.PlaceableName = "Poplar";
    }

    public void ChangeHouse()
    {
        DataHolder.PlaceableName = "House";
    }

    public void ChangeCastle()
    {
        DataHolder.PlaceableName = "Castle";
    }
    public void ChangeRock()
    {
        DataHolder.PlaceableName = "Rock";
    }

    public void ToggleNames()
    {
        DataHolder.ShowNames = !DataHolder.ShowNames;
    }
}
