/*
 * Author: David Milot
 * Controls cam movement and grabbing items.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _mouseSpeed = 10f;

    private bool _emptyHand = true;
    private float _grabDelayTime = 0.25f;
    private float _grabDelayTimer = 0.0f;

    private Camera _cam;
    private float _yaw = 0f;
    private float _pitch = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        transform.localRotation = Quaternion.Euler(5.0f, 0f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        //Check raycast as well as make sure the delay between grabs is sufficient
        if (Physics.Raycast(ray, out hit, 10.0f))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);

            ItemManager.Instance.DisplayDescriptionWindow(hit.transform.tag.ToLower(), hit.transform.position, _cam);

            if (_emptyHand)
            {
                if (ItemManager.Instance.isItem(hit.transform.tag) && Input.GetMouseButton(0) && _grabDelayTimer > _grabDelayTime)
                {
                    {
                        ItemManager.Instance.GrabItem(hit.transform.tag);
                        handanimations.Instance.SetHandGrab("Grab" + hit.transform.tag);
                        _emptyHand = false;
                        _grabDelayTimer = 0.0f;
                    }
                }
            }
            else
            {
                if (hit.transform.tag == "ItemSpot" && Input.GetMouseButton(0) && _grabDelayTimer > _grabDelayTime)
                {
                    if(hit.transform.gameObject.GetComponent<ItemHolder>() != null)
                    {
                        hit.transform.gameObject.GetComponent<ItemHolder>().PlaceItem(ItemManager.Instance.ItemHeld);
                        ItemManager.Instance.SetItem(hit.transform.position, hit.transform.gameObject.GetComponent<ItemHolder>());
                        _emptyHand = true;
                        handanimations.Instance.SetHandToIdle();
                        _grabDelayTimer = 0.0f;
                    }
                }
            }
        }

        _yaw += Input.GetAxis("Mouse X") * _mouseSpeed * Time.deltaTime;
        _pitch -= Input.GetAxis("Mouse Y") * _mouseSpeed * Time.deltaTime;

        _pitch = Mathf.Clamp(_pitch, -15.0f, 15.0f);
        _yaw = Mathf.Clamp(_yaw, -25.0f, 25.0f);
        _grabDelayTimer += Time.deltaTime;

        transform.localRotation = Quaternion.Euler(_pitch, _yaw, 0);
    }
}
