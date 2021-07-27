/*
 * Author: David Milot
 * Centralizes control of all items.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset _itemJSONFile;

    [SerializeField]
    private GameObject _sanitizer;

    [SerializeField]
    private GameObject _vial;

    [SerializeField]
    private GameObject _syringe;

    [SerializeField]
    private GameObject _gloveBox;

    private Dictionary<string, GameObject> _medItems = new Dictionary<string, GameObject>();

    private Dictionary<string, string> _medItemDescription = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        IItem medItem = new MedItem();
        _sanitizer.AddComponent<Sanitizer>();
        _sanitizer.GetComponent<Sanitizer>().Setup(medItem);
        _medItems.Add("sanitizer", _sanitizer);

        medItem = new MedItem();
        _syringe.AddComponent<Syringe>();
        _syringe.GetComponent<Syringe>().Setup(medItem);
        _medItems.Add("syringe", _syringe);

        medItem = new MedItem();
        _gloveBox.AddComponent<GloveBox>();
        _gloveBox.GetComponent<GloveBox>().Setup(medItem);
        _medItems.Add("glovebox", _gloveBox);

        medItem = new MedItem();
        _vial.AddComponent<Vial>();
        _vial.GetComponent<Vial>().Setup(medItem);
        _medItems.Add("vial", _vial);

        JSonReader jReader = new JSonReader();
        ItemDescription[] itemDescriptions = jReader.ReadItemJSONFile(_itemJSONFile);

        for (int i = 0; i < itemDescriptions.Length; i++)
        {
            _medItemDescription.Add(itemDescriptions[i].text, itemDescriptions[i].item);
        }
    }

    public string GetItemDescription(string name)
    {
        string desc;

        if (_medItemDescription.TryGetValue(name, out desc))
        {
            return desc;
        }

        return null;
    }
}
