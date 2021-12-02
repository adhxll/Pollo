using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PerspectivePan : MonoBehaviour
{
    public Camera cam;
    private Vector3 touchStart; //Vector for storing user input touch
    private float minX, minY, maxX, maxY, camMinX, camMinY, camMaxX, camMaxY, camHeight, camWidth;
    public GameObject GameObjectWithMaxBounds; //max bounds panning tergantung object, contoh: background
    private SpriteRenderer mapRenderer;
    private static bool enablePan = false; 
    void Awake()
    {
        SetupPanningObjects();
    }
    private void Start()
    {
        //inspector akan override function ini kalo ditaro di awake
        SetupCameraPosition(); 
    }


    private void Update()
    {
        PanCamera(); 
    }

    private void SetupPanningObjects() {
        //get sprite renderer dari si gameobject
        mapRenderer = GameObjectWithMaxBounds.GetComponent<SpriteRenderer>();

        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = cam.orthographicSize * cam.aspect;

        //bikin max min values berdasarkan gameobject
        minX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        maxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
        minY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        maxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;

        camMinX = minX + camWidth;
        camMaxX = maxX - camWidth;
        camMinY = minY + camHeight;
        camMaxY = maxY - camHeight;

        enablePan = true; 

    }

    private void SetupCameraPosition() {
        //biar cameranya gak menjorok ke tengah
        cam.transform.position = new Vector3(camMinX, 0, -1);
        Debug.Log("Camera position: " + cam.transform.position);
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

        //"jepit" camera move bounds biar ga out of bounds pas geser2 camera
        float newX = Mathf.Clamp(targetPosition.x, camMinX, camMaxX);
        float newY = Mathf.Clamp(targetPosition.y, 0, 0);

        return new Vector3(newX, newY, targetPosition.z); 
    }
    public static void SetPanning()
    {//Use this to disable/enable the panning
        if (enablePan) PerspectivePan.enablePan = false;
        else PerspectivePan.enablePan = true;
    }
}


