using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera cam;
    public float moveSpeed = 5f;
    public float zoomSpeed = 1f;
    public float minOrthographicSize = 1f;

    private float accumulatedScroll = 0f;
    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    public TilesCreationScript Tiles; // Assuming this script manages your hexagonal tiles

    // Hexagonal grid parameters
    public float hexWidth = 1f;  // Width of a hexagon
    public float hexHeight = 1f; // Height of a hexagon

    private float maxOrthographicSize;
    private bool isFullMapInView = false;

    public float clampedX;
    public float clampedY;

    void Start()
    {
        cam = Camera.main;
        maxOrthographicSize = 15;
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

        if (!isFullMapInView)
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                direction.y += 1;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                direction.y -= 1;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                direction.x -= 1;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                direction.x += 1;
            }

            cam.transform.position += direction * moveSpeed * Time.deltaTime;

            // Clamp camera position within map bounds
            float clampedX = Mathf.Clamp(cam.transform.position.x, mapMinX, mapMaxX);
            float clampedY = Mathf.Clamp(cam.transform.position.y, mapMinY, mapMaxY);

            // Set the clamped camera position
            cam.transform.position = new Vector3(clampedX, clampedY, cam.transform.position.z);
        }
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
            float newOrthographicSize = cam.orthographicSize - accumulatedScroll * zoomSpeed;
            newOrthographicSize = Mathf.Clamp(newOrthographicSize, minOrthographicSize, maxOrthographicSize);

            if (newOrthographicSize != cam.orthographicSize)
            {
                cam.orthographicSize = newOrthographicSize;

                // Recalculate camera movement constraints based on updated orthographic size
                CalculateCameraConstraints();

                // Gradually move the camera towards the center of the map
                if (cam.orthographicSize >= maxOrthographicSize)
                {
                    cam.transform.position = new Vector3(0, 0, cam.transform.position.z);
                }
                else
                {
                    Vector3 mapCenter = new Vector3((mapMinX + mapMaxX) / 2, (mapMinY + mapMaxY) / 2, cam.transform.position.z);
                    cam.transform.position = Vector3.Lerp(cam.transform.position, mapCenter, Time.deltaTime * zoomSpeed);
                }

                // Check if the entire map is in view
                isFullMapInView = cam.orthographicSize >= maxOrthographicSize;
            }

            accumulatedScroll = 0f; // Reset accumulated scroll
        }
    }

    void CalculateCameraConstraints()
    {
        mapMinX = -(maxOrthographicSize - cam.orthographicSize) * cam.aspect;
        mapMaxX = (maxOrthographicSize - cam.orthographicSize) * cam.aspect;
        mapMinY = -(maxOrthographicSize - cam.orthographicSize) / (cam.aspect * 2);
        mapMaxY = (maxOrthographicSize - cam.orthographicSize) / (cam.aspect * 2);
    }
}
