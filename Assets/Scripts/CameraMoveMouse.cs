using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraMoveMouse : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private FinalFactorySystem sys;
    [SerializeField] private float dragSpeed = 2.0f;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float minZoom = 5.0f;
    [SerializeField] private float maxZoom = 15.0f;
    [SerializeField] private Vector3 minBounds;
    [SerializeField] private Vector3 maxBounds;
    [SerializeField] private List<GameObject> objectsToFollowZ = new List<GameObject>();
    [SerializeField] private float objectFollowZOffset;
    [SerializeField] private List<GameObject> objectsToFollowX = new List<GameObject>();
    [SerializeField] private float objectFollowXOffset;

    private Vector3 dragOrigin;
    private bool isDragging = false;
    private Vector3 resetCameraPosition;

    private void Start()
    {
        resetCameraPosition = _camera.transform.position;
    }

    private void LateUpdate()
    {
        HandleDrag();
        HandleReset();
        HandleZoom();
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 currentMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentMousePosition;
            Vector3 newPosition = _camera.transform.position + difference * dragSpeed * Time.deltaTime;

            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.z = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);

            _camera.transform.position = new Vector3(newPosition.x, _camera.transform.position.y, newPosition.z);

            for (int i = 0; i < objectsToFollowZ.Count; i++)
            {
                objectsToFollowZ[i].transform.position = new Vector3(objectsToFollowZ[i].transform.position.x, objectsToFollowZ[i].transform.position.y, newPosition.z + objectFollowZOffset);
            }
            
            for (int i = 0; i < objectsToFollowX.Count; i++)
            {
                objectsToFollowX[i].transform.position = new Vector3(newPosition.x + objectFollowXOffset, objectsToFollowX[i].transform.position.y, objectsToFollowX[i].transform.position.z);
            }
        }
    }

    private void HandleReset()
    {
        if (Input.GetMouseButton(1))
        {
            _camera.transform.position = resetCameraPosition;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newSize = _camera.orthographicSize - scroll * zoomSpeed;

        _camera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }

    public void ChangeBounds(bool increment)
    {
        switch (increment)
        {
            case true:
                minBounds = new Vector3(minBounds.x + sys.cameraPosIncrementX, minBounds.y, minBounds.z);
                maxBounds = new Vector3(maxBounds.x + sys.cameraPosIncrementX, maxBounds.y, maxBounds.z);
                break;
            case false:
                minBounds = new Vector3(minBounds.x - sys.cameraPosIncrementX, minBounds.y, minBounds.z);
                maxBounds = new Vector3(maxBounds.x - sys.cameraPosIncrementX, maxBounds.y, maxBounds.z);
                break;
        }
    }
}
