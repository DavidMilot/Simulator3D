using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class JSonReader : MonoBehaviour
{
    public Tutorial ReadTutorialJSONFile(TextAsset jsonFile)
    {
        Assert.IsNotNull(jsonFile);

        Tutorial tutorial = JsonUtility.FromJson<Tutorial>(jsonFile.text);

        /*
        foreach (TutorialLine line in tutorial.tutorial)
        {
            Debug.Log("Found tutorial: " + line.text + " " + line.standby);
        }*/

        return tutorial;
    }

    public ItemDescription[] ReadItemJSONFile(TextAsset jsonFile)
    {
        Assert.IsNotNull(jsonFile);

        Item item = JsonUtility.FromJson<Item>(jsonFile.text);

        return item.item;
    }
}
