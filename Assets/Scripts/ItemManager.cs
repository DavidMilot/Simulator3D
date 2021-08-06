/*
 * Author: David Milot
 * Centralizes control of all items.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using UnityEngine.Events;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

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

    [SerializeField]
    private Image _windowImage;

    [SerializeField]
    private TextMeshProUGUI _windowDescription;

    [SerializeField]
    private Vector3 _itemHandOffset;

    private string _currentDisplayedItem = null;
    private string _lastDisplayedItem = null;

    private Dictionary<string, GameObject> _medItems = new Dictionary<string, GameObject>();
    private Dictionary<string, string> _medItemDescription = new Dictionary<string, string>();
    private Dictionary<string, ItemHolder> _medItemPlacement = new Dictionary<string, ItemHolder>();

    [SerializeField]
    private List<MonoBehaviour> _itemHolders;
    private Vector3[] _holderPositions;

    private UnityEvent _eventCheckAll;

    private string _itemHeld = "";

    public string ItemHeld
    {
        get
        {
            return _itemHeld;
        }
    }

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
    }

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
            _medItemDescription.Add(itemDescriptions[i].item, itemDescriptions[i].text);
        }

        _holderPositions = new Vector3[_itemHolders.Count];

        for (int i = 0; i < _itemHolders.Count; i++)
        {
            Debug.Log(_itemHolders[i].transform.position);
            _holderPositions[i] = _itemHolders[i].transform.position;
        }

        _currentDisplayedItem = "";
        _lastDisplayedItem = "";

        if (_eventCheckAll == null)
        {
            _eventCheckAll = new UnityEvent();
        }

        _eventCheckAll.AddListener(CheckOrder);
    }

    public void DisplayDescriptionWindow(string name, Vector3 itemPos, Camera cam)
    {
        if (_medItemDescription.ContainsKey(name))
        {
            _windowImage.enabled = true;
            _windowDescription.enabled = true;
        }
        else
        {
            _windowImage.enabled = false;
            _windowDescription.enabled = false;

            return;
        }

        //If the hovered over item exist in the dictionary, get the description text
        //and display the description window over the item.
        string desc;
        if (_medItemDescription.TryGetValue(name, out desc))
        {
            Vector3 pos = RectTransformUtility.WorldToScreenPoint(cam, itemPos);
            _windowImage.rectTransform.position = new Vector2(pos.x, pos.y);
            _windowDescription.text = _medItemDescription[name];

            _currentDisplayedItem = _medItemDescription[name];

            //Check the current and previous item to avoid repeated sound trigger.
            if (_currentDisplayedItem != _lastDisplayedItem)
            {
                AudioManager.Instance.PlaySound(Enums.AudioSoundEffects.DisplayDescription, false, true, itemPos);
                _lastDisplayedItem = _currentDisplayedItem;
            }
        }
    }

    public bool isItem(string name)
    {
        if (_medItemDescription.ContainsKey(name.ToLower()))
        {
            return true;
        }

        return false;
    }

    public void GrabItem(string itemName)
    {
        itemName = itemName.ToLower();
        if (_medItemDescription.ContainsKey(itemName.ToLower()))
        {
            if (itemName == "vial")
            {
                _medItems[itemName].GetComponent<Vial>().Grab(handanimations.Instance.HandPosition + new Vector3(0.04f, -0.024f, 0.0165f));
                AudioManager.Instance.PlaySound(Enums.AudioSoundEffects.PickupVial, false, true, _medItems[itemName].transform.position);
            } 
            else if (itemName == "syringe")
            {
                _medItems[itemName].GetComponent<Syringe>().Grab(handanimations.Instance.HandPosition);
                AudioManager.Instance.PlaySound(Enums.AudioSoundEffects.PickUpSyringe, false, true, _medItems[itemName].transform.position);
            }
            else if (itemName == "glovebox")
            {
                _medItems[itemName].GetComponent<GloveBox>().Grab(handanimations.Instance.HandPosition + new Vector3(0.065f, -0.036f, 0.0225f));
                AudioManager.Instance.PlaySound(Enums.AudioSoundEffects.PickupGloveBox, false, true, _medItems[itemName].transform.position);
            }
            else if (itemName == "sanitizer")
            {
                _medItems[itemName].GetComponent<Sanitizer>().Grab(handanimations.Instance.HandPosition);
                AudioManager.Instance.PlaySound(Enums.AudioSoundEffects.PickupSanitizer, false, true, _medItems[itemName].transform.position);
            }

            _itemHeld = itemName;

            //If the value in the dictionay exist, that means the item is placed in one of the holders
            //Access holder and invoke the function to remove item
            if (_medItemPlacement.ContainsKey(_itemHeld))
            {
                if (_medItemPlacement[_itemHeld] != null)
                {
                    _medItemPlacement[_itemHeld].RemoveItem();
                }
            }
        }
    }

    public void SetItem(Vector3 pos, ItemHolder holder)
    {
        if (_medItems.ContainsKey(_itemHeld))
        {
            //Store reference to where the item is being placed in
            if (!_medItemPlacement.ContainsKey(_itemHeld))
            {
                _medItemPlacement.Add(_itemHeld, null);
            }
            _medItemPlacement[_itemHeld] = holder;

            //Set position and drop item
            _medItems[_itemHeld].transform.position = pos;
            if (_itemHeld == "vial")
            {
                _medItems[_itemHeld].GetComponent<Vial>().Drop();
            }
            else if (_itemHeld == "syringe")
            {
                _medItems[_itemHeld].GetComponent<Syringe>().Drop();
            }
            else if (_itemHeld == "glovebox")
            {
                _medItems[_itemHeld].GetComponent<GloveBox>().Drop();
            }
            else if (_itemHeld == "sanitizer")
            {
                _medItems[_itemHeld].GetComponent<Sanitizer>().Drop();
            }
            AudioManager.Instance.PlaySound(Enums.AudioSoundEffects.DropItem, false, true, _medItems[_itemHeld].transform.position);

            _itemHeld = "";
        }

        _eventCheckAll.Invoke();
    }

    private void CheckOrder()
    {
        Debug.Log("CheckOrder");

        for (int i = 0; i < _itemHolders.Count; i++)
        {
            if (!_itemHolders[i].GetComponent<ItemHolder>().CorrectItemPlaced())
            {
                DialogueManager.Instance.BadOrderText();
                return;
            }
        }
        DialogueManager.Instance.GoodOrderText();
    }

    public void ResetAll()
    {
        //Randomize all positions of ItemHolders here
        System.Random random = new System.Random();
        _holderPositions = _holderPositions.OrderBy(x => random.Next()).ToArray();

        for (int i = 0; i < _itemHolders.Count; i++)
        {
            _itemHolders[i].transform.position = _holderPositions[i];
            _itemHolders[i].GetComponent<ItemHolder>().Reset();
        }

        //Set all med item placements to null
        List<string> keys = new List<string>(_medItemPlacement.Keys);
        foreach (string key in keys)
        {
            _medItemPlacement[key] = null;
        }

        //Reset all med item positions to top of shelf
        _medItems["syringe"].GetComponent<Syringe>().Reset();
        _medItems["vial"].GetComponent<Vial>().Reset();
        _medItems["sanitizer"].GetComponent<Sanitizer>().Reset();
        _medItems["glovebox"].GetComponent<GloveBox>().Reset();
    }
}