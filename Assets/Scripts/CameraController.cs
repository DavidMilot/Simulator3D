using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private float mouseSpeed = 10f;

    private float yaw = 0f;
    private float pitch = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        transform.localRotation = Quaternion.Euler(5.0f, 0f, 0); 
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10.0f))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);

            if (Input.GetMouseButtonDown(1))
            {
            }
        }

        yaw += Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
