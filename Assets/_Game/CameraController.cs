using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    const float EXTRA_SPACE = 1; // world units, which mostly align with tiles
    const float PAN_SNAPPINESS = 0.1f; // percent (of )
    const float PAN_SPEED = 0.05f; //  per second

    // if the user moves the camera, this will move in unison.
    // but an external script can change this and the camera won't snap around.
    private Vector3 _targetPos = new Vector3(0,0,-10);
    public Vector3 targetPos {
        set { _targetPos = fixBounds(value); }
        get {return _targetPos;}
    }

    const float ZOOM_SNAPPINESS = 0.1f; // fraction more correct each draw
    // same but for zoom
    public float targetZoom = 5;

    // initialization
    public static CameraController instance;
    private Camera camera;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        camera = this.GetComponent<Camera>();
    }

    // TODO: use deltaTime.
    void Update()
    {
        // user input
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            targetZoom -= Input.GetAxis("Mouse ScrollWheel");
            camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel");
            targetZoom = Math.Max(3, Math.Min(5, targetZoom));
            camera.orthographicSize = Math.Max(3, Math.Min(5, camera.orthographicSize));
        }

        // sorry if the lines wrap lmao
        Vector3 userVel = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) { userVel += camera.orthographicSize * PAN_SPEED * Vector3.up; }
        if (Input.GetKey(KeyCode.A)) { userVel += camera.orthographicSize * PAN_SPEED * Vector3.left; }
        if (Input.GetKey(KeyCode.S)) { userVel += camera.orthographicSize * PAN_SPEED * Vector3.down; }
        if (Input.GetKey(KeyCode.D)) { userVel += camera.orthographicSize * PAN_SPEED * Vector3.right; }
        targetPos += userVel;
        transform.position += userVel; // itll overstep bounds, but thats ok, ish.


        // if desynced, move until correct.
        float zoomDiff = camera.orthographicSize - targetZoom;
        if (zoomDiff != 0)
        {
            if (Math.Abs(zoomDiff) > 0.01)
            {
                camera.orthographicSize += (targetZoom - camera.orthographicSize) * ZOOM_SNAPPINESS;
            }
            else
            {
                camera.orthographicSize = targetZoom;
            }
        }

        float squareDist = (transform.position - targetPos).sqrMagnitude;
        if (squareDist != 0)
        {
            if (squareDist > 0.0001)
            {
                // yes, ik this isn't the intended use of lerp.
                transform.position = Vector3.Lerp(transform.position, targetPos, PAN_SNAPPINESS);
            }
            else
            {
                transform.position = targetPos;
            }
        }

    }

    // bounds helpers
    private Vector2 negativeLimit;
    private Vector2 positiveLimit;

    private void createLimits()
    {   
        // Super inefficient, but it'll only get called once.
        // also singletons lmao.
        Vector3 left = MapMath.MapToWorld(0, 0);
        Vector3 right = MapMath.MapToWorld(MapController.instance.mapWidth, MapController.instance.mapHeight);
        Vector3 up = MapMath.MapToWorld(MapController.instance.mapWidth, 0);
        Vector3 down = MapMath.MapToWorld(0, MapController.instance.mapHeight);

        // the extra space is twice as big in the y direction, but thats fine.
        negativeLimit = new Vector2(left.x - 0.5f, down.y) - Vector2.one * EXTRA_SPACE;
        positiveLimit = new Vector2(right.x - 0.5f, up.y) + Vector2.one * EXTRA_SPACE;
    }

    private Vector3 fixBounds(Vector3 value)
    {
        if (negativeLimit == Vector2.zero) {this.createLimits();} 
        value.z = -10;

        float halfScreenWidth = targetZoom * camera.aspect;
        float halfScreenHeight = targetZoom;

        // if the screen cannot fit between the two limits, set the camera to their midpoint.
        // else
            // if it goes too far negative, set it in that limit
            // if it goes too far positive, set it to that limit

        if (2 * halfScreenWidth > positiveLimit.x - negativeLimit.x) {value.x = (negativeLimit.x + positiveLimit.x)/2;}
        else
        {
            value.x = Math.Max(value.x, negativeLimit.x + halfScreenWidth);
            value.x = Math.Min(value.x, positiveLimit.x - halfScreenWidth);
        }

        if (2 * halfScreenHeight > positiveLimit.y - negativeLimit.y) {value.y = (negativeLimit.y + positiveLimit.y)/2;}
        else
        {
            value.y = Math.Max(value.y, negativeLimit.y + halfScreenHeight);
            value.y = Math.Min(value.y, positiveLimit.y - halfScreenHeight);
        }

        return value;
    }
}
