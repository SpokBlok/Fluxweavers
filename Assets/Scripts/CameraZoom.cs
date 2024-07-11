using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera cam;
    public float moveSpeed = 5f;
    public float zoomSpeed = 1f;
    public float minOrthographicSize = 1f;
    public float maxOrthographicSize = 10f;

    private float accumulatedScroll = 0f;
    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    public TilesCreationScript Tiles; // Assuming this script manages your hexagonal tiles

    // Hexagonal grid parameters
    public float hexWidth = 1f;  // Width of a hexagon
    public float hexHeight = 1f; // Height of a hexagon

    void Start()
    {
        cam = Camera.main;
        InitializeTiles(); // Initialize Tiles here
        CalculateCameraConstraints();
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    void InitializeTiles()
    {
        Tiles = FindObjectOfType<TilesCreationScript>(); // Find TilesCreationScript in the scene
        if (Tiles == null)
        {
            Debug.LogError("TilesCreationScript not found or not assigned to CameraControl.");
        }
    }

    void HandleMovement()
    {
        if (cam == null)
        {
            Debug.LogError("Main camera not found.");
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, vertical, 0).normalized;
        cam.transform.position += direction * moveSpeed * Time.deltaTime;

        // Clamp camera position within map bounds
        float clampedX = Mathf.Clamp(cam.transform.position.x, mapMinX, mapMaxX);
        float clampedY = Mathf.Clamp(cam.transform.position.y, mapMinY, mapMaxY);

        cam.transform.position = new Vector3(clampedX, clampedY, cam.transform.position.z);
    }

    void HandleZoom()
    {
        if (cam == null)
        {
            Debug.LogError("Main camera not found.");
            return;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        accumulatedScroll += scroll;

        // Apply zoom once per frame
        if (!Mathf.Approximately(accumulatedScroll, 0f))
        {
            cam.orthographicSize -= accumulatedScroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minOrthographicSize, maxOrthographicSize);

            // Recalculate camera movement constraints based on updated orthographic size
            CalculateCameraConstraints();

            accumulatedScroll = 0f; // Reset accumulated scroll
        }
    }

    void CalculateCameraConstraints()
    {
        if (Tiles == null)
        {
            Debug.LogError("TilesCreationScript not initialized.");
            return;
        }

        // Calculate camera's half-width and half-height based on orthographic size and aspect ratio
        float cameraHalfWidth = cam.orthographicSize * cam.aspect;
        float cameraHalfHeight = cam.orthographicSize;

        // Calculate actual map bounds accounting for hexagon shape
        float mapHalfWidth = (Tiles.returnColCount() - 1) * (hexWidth * 0.75f);
        float mapHalfHeight = (Tiles.returnRowCount() - 1) * (hexHeight + hexHeight / 2f);

        // Update clamp values for camera position based on new orthographic size
        mapMinX = -mapHalfWidth + cameraHalfWidth;
        mapMaxX = mapHalfWidth - cameraHalfWidth;
        mapMinY = -mapHalfHeight + cameraHalfHeight;
        mapMaxY = mapHalfHeight - cameraHalfHeight;
    }
}
