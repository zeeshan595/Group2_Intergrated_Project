using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DropDown
{
    public bool isClicked = false;
    public List<string> items = new List<string>();
    public int selectedItem = 0;

    public event EventHandler changedValue;
    public delegate void EventHandler(int i);

    public DropDown(List<string> items, int defaultItem)
    {
        this.items = items;
        this.selectedItem = defaultItem;
    }

    public int Draw(LevelEditor e)
    {
        if (isClicked)
        {
            for (int x = 0; x < items.Count; x++)
            {
                if (GUILayout.Button(items[x]))
                {
                    e.updateUndo(items[x] + " selected");
                    selectedItem = x;
                    isClicked = false;
                    changedValue(selectedItem);
                }
            }
        }
        else
        {
            if (GUILayout.Button(items[selectedItem]))
            {
                isClicked = true;
            }
        }
        return selectedItem;
    }

    public int Draw()
    {
        if (isClicked)
        {
            for (int x = 0; x < items.Count; x++)
            {
                if (GUILayout.Button(items[x]))
                {
                    selectedItem = x;
                    isClicked = false;
                    changedValue(selectedItem);
                }
            }
        }
        else
        {
            if (GUILayout.Button(items[selectedItem]))
            {
                isClicked = true;
            }
        }
        return selectedItem;
    }
}