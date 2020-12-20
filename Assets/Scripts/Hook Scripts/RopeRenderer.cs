using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{

    private LineRenderer lineRenderer;
    public Transform startPosition;
    private float lineWidth = 0.05f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.enabled = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void renderLine(Vector3 endPosition, bool enableRenderer)
    {
        if(enableRenderer)
        {
            if(!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
            }

            lineRenderer.positionCount = 2;
        }

        else
        {
            lineRenderer.positionCount = 0;
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
            }

        }

        if(lineRenderer.enabled)
        {
            Vector3 temp = startPosition.position;
            temp.z = 0f;

            startPosition.position = temp;

            temp = endPosition;
            temp.z = 0f;

            endPosition = temp;

            lineRenderer.SetPosition(0, startPosition.position);
            lineRenderer.SetPosition(1, endPosition);
        }
    }
}
