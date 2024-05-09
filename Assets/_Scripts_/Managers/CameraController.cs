//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

/// <summary>
/// Controls the camera's movement and zoom within the game world.
/// </summary>
public class CameraController : MonoBehaviour
{
    public float moveSpeed;             // Speed at which the camera moves.

    public float minZoom;               //Minimum  zoom levels for the camera.
    public float maxZoom;               //Maximum zoom levels for the camera.

    public float zoomSpeed;             // Speed at which the camera zooms in and out.

    private float curZoom;              // Current zoom level of the camera.

    public float minX;                  //Minimum x coordinates the camera can move to.
    public float maxX;                  //Maximum x coordinates the camera can move to.

    public float minY;                  //Minimum y coordinates the camera can move to.
    public float maxY;                  //Maximum y coordinates the camera can move to.

    private Vector3 curPosition;        // Current position of the camera.

    private Camera cam;                 // Camera component reference.

    /// <summary>
    /// Initializes the camera's position and zoom level.
    /// </summary>
    private void Start()
    {
        // Get the main camera component.
        cam = Camera.main;

        // Initialize current zoom based on the camera's position.
        curZoom = transform.localPosition.z;

        // Initialize the current position of the camera.
        curPosition = transform.position;
    }

    /// <summary>
    /// Updates the camera's zoom and position each frame.
    /// </summary>
    private void Update()
    {
        // Adjust zoom based on the mouse scroll wheel.
        curZoom += Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed;
        curZoom = Mathf.Clamp(curZoom, minZoom, maxZoom);

        // Set the camera's orthographic size based on the current zoom level.
        cam.orthographicSize = curZoom;

        // Calculate the direction of movement.
        Vector3 horizontal = transform.right;
        Vector3 vertical = transform.up;

        float moveY = Input.GetAxisRaw("Horizontal");
        float moveX = Input.GetAxisRaw("Vertical");

        // Determine the direction and update the current position.
        Vector3 dir = horizontal * moveY + vertical * moveX;

        curPosition = transform.position + moveSpeed * Time.deltaTime * dir;

        curPosition.x = Mathf.Clamp(curPosition.x, minX, maxX);
        curPosition.y = Mathf.Clamp(curPosition.y, minY, maxY);

        // Update the camera's position.
        transform.position = curPosition;
    }
}
