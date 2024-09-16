using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveTouch : MonoBehaviour
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
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                dragOrigin = _camera.ScreenToWorldPoint(touch.position);
                isDragging = true;
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }

            if (isDragging && touch.phase == TouchPhase.Moved)
            {
                Vector3 currentTouchPosition = _camera.ScreenToWorldPoint(touch.position);
                Vector3 difference = dragOrigin - currentTouchPosition;
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
    }

    private void HandleReset()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if ((touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began) && touch1.tapCount == 2)
            {
                _camera.transform.position = resetCameraPosition;
            }
        }
    }

    private void HandleZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            float newSize = _camera.orthographicSize + deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;

            _camera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
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
