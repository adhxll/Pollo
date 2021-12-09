using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PerspectivePan : MonoBehaviour
{
    [SerializeField] private Camera cam = null;
    [SerializeField] private GameObject GameObjectWithMaxBounds = null; // Max bounds panning tergantung object, contoh: background

    private Vector3 touchStart; // Vector for storing user input touch
    private float minX, minY, maxX, maxY, camMinX, camMinY, camMaxX, camMaxY, camHeight, camWidth;
    private static bool enablePan = false;

    void Awake()
    {
        SetupPanningObjects();
    }

    private void Start()
    {
        // Inspector akan override function ini kalo ditaro di awake
        SetupCameraPosition(); 
    }

    private void Update()
    {
        PanCamera(); 
    }

    private void SetupPanningObjects() {

        var renderer = GameObjectWithMaxBounds.GetComponent<Renderer>();

        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = cam.orthographicSize * cam.aspect;

        // Bikin max min values berdasarkan gameobject
        minX = renderer.transform.position.x - renderer.bounds.size.x / 2f;
        maxX = renderer.transform.position.x + renderer.bounds.size.x / 2f;
        minY = renderer.transform.position.y - renderer.bounds.size.y / 2f;
        maxY = renderer.transform.position.y + renderer.bounds.size.y / 2f;

        camMinX = minX + camWidth;
        camMaxX = maxX - camWidth;
        camMinY = minY + camHeight;
        camMaxY = maxY - camHeight;

        enablePan = true; 

    }

    private void SetupCameraPosition() {
        // Biar cameranya gak menjorok ke tengah
        cam.transform.position = new Vector3(camMinX, 0, -1);
        //Debug.Log("Camera position: " + cam.transform.position);
    }

    private void PanCamera() {

        if (enablePan) {
            if (Input.GetMouseButtonDown(0)) touchStart = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButton(0))
            {
                Vector3 difference = touchStart - cam.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position = ClampCamera(cam.transform.position + difference);
            }
        }
    }

    private Vector3 ClampCamera(Vector3 targetPosition) { 

        // "Jepit" camera move bounds biar ga out of bounds pas geser2 camera
        float newX = Mathf.Clamp(targetPosition.x, camMinX, camMaxX);
        float newY = Mathf.Clamp(targetPosition.y, 0, 0);

        return new Vector3(newX, newY, targetPosition.z);

    }

    public static void SetPanning()
    {
        // Use this to disable/enable the panning
        if (enablePan) PerspectivePan.enablePan = false;
        else PerspectivePan.enablePan = true;
    }
}


