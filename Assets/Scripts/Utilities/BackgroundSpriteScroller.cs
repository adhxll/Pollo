using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpriteScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1f;

    private float rightEdge;
    private float leftEdge;
    private Vector3 distanceBetweenEdges;

    // Start is called before the first frame update
    void Start()
    {
        CalculateEdge();
        distanceBetweenEdges = new Vector3(rightEdge - leftEdge, 0f, 0f);
    }

    private void CalculateEdge()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        rightEdge = transform.position.x + spriteRenderer.bounds.extents.x / 3f;
        leftEdge = transform.position.x - spriteRenderer.bounds.extents.x / 3f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += scrollSpeed * Vector3.right * Time.deltaTime;

        if (PassedEdge())
            MoveRightSpriteToOppositeEdge();
    }

    private bool PassedEdge()
    {
        return scrollSpeed > 0 && transform.position.x > rightEdge ||
            scrollSpeed < 0 && transform.position.x < leftEdge;
    }

    private void MoveRightSpriteToOppositeEdge()
    {
        if (scrollSpeed > 0)
            transform.position -= distanceBetweenEdges;
        else
            transform.position += distanceBetweenEdges;
    }
}
