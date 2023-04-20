using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The object that the camera will follow
    public float followSpeed = 5f; // The speed at which the camera follows the target
    public float minZoom = 5f; // The minimum zoom level
    public float maxZoom = 15f; // The maximum zoom level
    public float zoomSpeed = 5f; // The speed at which the camera zooms in and out
    public float cameraDragSpeed = 5;
    public float shakeMagnitude;
    public float shakeDuration;

    private Camera cam; // The Camera component
    private Vector3 initialPosition; // The initial position of the camera
    private bool isFollowingTarget = true; // Whether or not the camera is currently following the target
    private SceneManager sceneManager;

    void Start()
    {
        cam = GetComponent<Camera>();
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
        initialPosition = transform.position;
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void LateUpdate()
    {
        // Check if the player clicked on the target or not
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit && hit.transform == target)
            {
                // If the player clicked on the target, stop following the target and allow the camera to move around
                isFollowingTarget = true;
            }
            else
            {
                isFollowingTarget = false;
            }
        }
        if (!isFollowingTarget)
        {
            // Move the camera around
            if (Input.GetMouseButton(0))
            {
                float moveX = Input.GetAxis("Mouse X");
                float moveY = Input.GetAxis("Mouse Y");
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x - moveX, transform.position.y - moveY, transform.position.z), cameraDragSpeed * Time.deltaTime);
                
            }
            else if(Input.GetMouseButtonUp(0))
            {
                isFollowingTarget = true;
            }
        }
        else
        {
            Vector3 targetPos = new Vector3 (target.position.x, target.position.y, transform.position.z);
            // Follow the target
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, sceneManager.minSceneBounds.x, sceneManager.maxSceneBounds.x),
            Mathf.Clamp(transform.position.y, sceneManager.minSceneBounds.y, sceneManager.maxSceneBounds.y),
            transform.position.z);

        // Zoom in and out
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);   
    }


    public IEnumerator Shake()
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / shakeDuration;
            float damper = 1f - Mathf.Clamp(2f * percentComplete - 1f, 0f, 1f);

            float x = Random.value * 2f - 1f;
            float y = Random.value * 2f - 1f;
            x *= shakeMagnitude * damper;
            y *= shakeMagnitude * damper;

            transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            yield return null;
        }

        transform.position = originalPosition;
    }
}

