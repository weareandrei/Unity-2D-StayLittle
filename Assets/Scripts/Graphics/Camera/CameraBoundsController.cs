using UnityEngine;
using Cinemachine;

public class CameraBoundsController : MonoBehaviour
{
    public GameObject leftBound;
    public GameObject rightBound;

    private CinemachineVirtualCamera virtualCamera;
    private Camera mainCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 cameraPosition = transform.position;
        Vector3 targetPosition = cameraPosition;

        // Get the camera's viewport coordinates (between 0 and 1)
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(cameraPosition);

        // Check if the camera is beyond the left bound
        if (viewportPosition.x < 0f)
        {
            targetPosition.x = leftBound.transform.position.x;
        }

        // Check if the camera is beyond the right bound
        if (viewportPosition.x > 1f)
        {
            targetPosition.x = rightBound.transform.position.x;
        }

        // Move the camera to the target position
        transform.position = targetPosition;
    }
}