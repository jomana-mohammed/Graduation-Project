using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.XR.CoreUtils;
using UnityEngine.EventSystems;



public class Funr : MonoBehaviour
{
    public GameObject SpawnableFurniture;
    public XROrigin sessionOrigin;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    private GameObject selectedObject = null;
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    private void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            if (isButtonPressed()) return;

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                selectedObject = hit.transform.gameObject;
                return;
            }

            bool collision = raycastManager.Raycast(touch.position, raycastHits, TrackableType.PlaneWithinPolygon);

            if (collision)
            {
                GameObject _object = Instantiate(SpawnableFurniture);
                _object.transform.position = raycastHits[0].pose.position;
                _object.transform.rotation = raycastHits[0].pose.rotation;
                _object.AddComponent<ARSelectableObject>();  // Attach movement & deletion script
            }

            foreach (var planes in planeManager.trackables)
            {
                planes.gameObject.SetActive(false);
            }

            planeManager.enabled = false;
        }
    }

    public bool isButtonPressed()
    {
        return EventSystem.current.currentSelectedGameObject?.GetComponent<Button>() != null;
    }

    public void SwitchFurniture(GameObject furniture)
    {
        SpawnableFurniture = furniture;
    }
}