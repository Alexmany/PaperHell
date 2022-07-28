using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDraw : MonoBehaviour
{
    public Paper _paper;
    public int ready_int;
    public int current_int;

    [Space]
    public bool letsDraw;
    public Transform brush;
    public Vector2 minBorder;
    public Vector2 maxBorder;    

    LineRenderer currentLineRenderer;

    Vector2 lastPos;
    Camera m_camera;

    bool firstTime = true;

    private void Start()
    {
        currentLineRenderer = GetComponent<LineRenderer>();
        m_camera = Camera.main;
    }

    private void Update()
    {       
        if (Input.touchCount > 0 && letsDraw)
        {
            if (firstTime)
                CreateBrush();
            else
                PointToMousePos();
        }

        if (current_int > ready_int)
            _paper.ReadyToGo();
    }

    void CreateBrush()
    {       
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y));
        brush.position = mousePos;

        currentLineRenderer.SetPosition(0, brush.localPosition);
        currentLineRenderer.SetPosition(1, brush.localPosition);

        firstTime = false;
    }

    void AddAPoint(Vector2 pointPos)
    {
        current_int++;
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;        

        if (pointPos.x < minBorder.x)
            pointPos.x = minBorder.x;

        if (pointPos.x > maxBorder.x)
            pointPos.x = maxBorder.x;

        if (pointPos.y < minBorder.y)
            pointPos.y = minBorder.y;

        if (pointPos.y > maxBorder.y)
            pointPos.y = maxBorder.y;
        
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    void PointToMousePos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y));
        brush.position = mousePos;

        if (lastPos != (Vector2)brush.localPosition)
        {
            AddAPoint(brush.localPosition);
            lastPos = brush.localPosition;
        }
    }
}
