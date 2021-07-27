/*
 * Author: David Milot
 * Loads and stores dialogue text for the tutorial and controls message flow.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField]
    private TextAsset _tutorialFile;

    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private Image _enter;

    private TutorialLine[] _tutorialLines;
    private int _tutorialLineIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            GameObject.Destroy(Instance);
        }

        _enter.enabled = false;
    }

    private void Start()
    {
        LoadTutorialFile();
    }

    public void LoadTutorialFile()
    {
        JSonReader jReader = new JSonReader();
        _tutorialLines = jReader.ReadTutorialJSONFile(_tutorialFile).tutorial;
    }

    public void InitTutorial()
    {
        _text.text = _tutorialLines[_tutorialLineIndex].text;

        if (_tutorialLines[_tutorialLineIndex].standby == "[ENTER]")
        {
            _enter.enabled = true;
        }
        else
        {
            _enter.enabled = false;
        }
    }

    public bool StepNextTutorial()
    {
        return true;
    }

    public string GetText()
    {
        return _tutorialLines[_tutorialLineIndex].text;
    }

    public string GetStandby()
    {
        return _tutorialLines[_tutorialLineIndex].standby;
    }
}
