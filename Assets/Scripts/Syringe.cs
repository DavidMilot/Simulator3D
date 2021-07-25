/*
 * Author: David Milot
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : MonoBehaviour
{
    private IItem i_item;

    public void Setup(IItem item)
    {
        i_item = item;
    }

}
