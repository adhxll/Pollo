using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PerspectivePan : MonoBehaviour
{
    public Camera cam;
    private Vector3 touchStart; //Vector for storing user input touch
    private float minX, minY, maxX, maxY;
    public GameObject GameObjectWithMaxBounds; //max bounds panning tergantung object, contoh: background
    private SpriteRenderer mapRenderer;

    void Awake()
    {
        SetupPanningObjects();
    }


    private void Update()
    {
        PanCamera(); 
    }

    private void SetupPanningObjects() {
        //get sprite renderer dari si gameobject
        mapRenderer = GameObjectWithMaxBounds.GetComponent<SpriteRenderer>();

        cam = Camera.main;

        //bikin max min values berdasarkan gameobject
        minX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        maxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
        minY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        maxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
    }
    private void PanCamera() {
        if (Input.GetMouseButtonDown(0)) touchStart = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0)) {
            Vector3 difference = touchStart - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position =  ClampCamera(cam.transform.position + difference); 
        }
    }

    private Vector3 ClampCamera(Vector3 targetPosition) { 

        //"jepit" camera move bounds biar ga out of bounds pas geser2 camera
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;
        float camMinX = minX + camWidth;
        float camMaxX = maxX - camWidth;
        float camMinY = minY + camHeight;
        float camMaxY = maxY - camHeight;
        float newX = Mathf.Clamp(targetPosition.x, camMinX, camMaxX);
        float newY = Mathf.Clamp(targetPosition.y, camMinY, camMaxY);

        return new Vector3(newX, newY, targetPosition.z); 
    }
}


