/*
 * Author: David Milot
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : MonoBehaviour
{
    private IItem i_item;
    private bool _grabbed = false;
    private Vector3 _startPos;
    private ItemHolder _setHolder;

    private void Start()
    {
        _startPos = transform.position;
    }

    public void Setup(IItem item)
    {
        i_item = item;
        _grabbed = false;
    }

    public bool Grabbed
    {
        get
        {
            return _grabbed;
        }
    }

    public void Grab(Vector3 pos)
    {
        Debug.Log("Grab");
        _grabbed = true;
        transform.position = pos;
    }

    public void Drop()
    {
        _grabbed = false;
    }

    public void Reset()
    {
        transform.position = _startPos;
        Drop();
    }
}
