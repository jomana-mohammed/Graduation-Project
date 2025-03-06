using UnityEngine;
using UnityEngine.EventSystems;

public class ARSelectableObject : MonoBehaviour
{
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool isDragging = false;
    private Camera arCamera;
    private float lastTapTime = 0f;
    private const float doubleTapThreshold = 0.3f;

    private void Start()
    {
        arCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = arCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                float currentTime = Time.time;
                if (currentTime - lastTapTime < doubleTapThreshold)
                {
                    Destroy(gameObject); // Double-tap detected, delete object
                    return;
                }
                lastTapTime = currentTime;

                isDragging = true;
                touchStartPos = touch.position;
            }
        }
        else if (touch.phase == TouchPhase.Moved && isDragging)
        {
            touchEndPos = touch.position;
            Vector2 delta = touchEndPos - touchStartPos;
            transform.position += new Vector3(delta.x * 0.001f, 0, delta.y * 0.001f);
            touchStartPos = touchEndPos;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            isDragging = false;
        }
    }
}
