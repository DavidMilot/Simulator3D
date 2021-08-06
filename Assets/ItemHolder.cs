using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField]
    private Enums.Items _itemHolder;
    private Enums.Items _itemHeld = Enums.Items.None;

    private SphereCollider _sphereCollider;

    // Start is called before the first frame update
    void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    public void Reset()
    {
        _itemHeld = Enums.Items.None;
        _sphereCollider.enabled = true;
    }

    public void PlaceItem(string itemName)
    {
        Enums.Items item;

        if (itemName == "syringe")
        {
            item = Enums.Items.Syringe;
        }
        else if (itemName == "sanitizer")
        {
            item = Enums.Items.Sanitizer;
        }
        else if (itemName == "glovebox")
        {
            item = Enums.Items.GloveBox;
        }
        else
        {
            item = Enums.Items.Vial;
        }

        _itemHeld = item;
        _sphereCollider.enabled = false;
    }

    public bool CorrectItemPlaced()
    {
        //Debug.Log(_itemHeld.ToString() + " " + _itemHolder.ToString());
        return _itemHeld == _itemHolder;
    }

    public void RemoveItem()
    {
        _itemHeld = Enums.Items.None;
        _sphereCollider.enabled = true;
    }
}
