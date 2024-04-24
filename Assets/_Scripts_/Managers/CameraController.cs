using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed;

    public float minZoom;
    public float maxZoom;
    public float zoomSpeed;
    private float curZoom;

    public float minX;
    public float maxX;

    public float minY;
    public float maxY;

    private Vector3 curPosition;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        curZoom = transform.localPosition.z; 
        curPosition = transform.position;
    }

    private void Update()
    {
        // Zoom camera
        curZoom += Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed;
        curZoom = Mathf.Clamp(curZoom, minZoom, maxZoom);
        cam.orthographicSize = curZoom;

        // Move camera
        Vector3 horizontal = transform.right;
        Vector3 vertical = transform.up;

        float moveY = Input.GetAxisRaw("Horizontal");
        float moveX = Input.GetAxisRaw("Vertical");

        Vector3 dir = horizontal * moveY + vertical * moveX;

        curPosition = transform.position + moveSpeed * Time.deltaTime * dir;

        curPosition.x = Mathf.Clamp(curPosition.x, minX, maxX);
        curPosition.y = Mathf.Clamp(curPosition.y, minY, maxY);

        transform.position = curPosition;

    }
}
