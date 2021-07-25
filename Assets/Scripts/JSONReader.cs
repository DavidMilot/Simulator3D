using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSonReader : MonoBehaviour
{
    public Tutorial ReadJSONFile(TextAsset jsonFile)
    {
        Tutorial tutorial = JsonUtility.FromJson<Tutorial>(jsonFile.text);

        foreach (TutorialLine line in tutorial.tutorialLines)
        {
            Debug.Log("Found employee: " + line.text + " " + line.standby);
        }

        return tutorial;
    }
}
