using UnityEngine;

public class CameraPathFollower : MonoBehaviour
{
    public Transform[] pathPoints;
    public Transform player;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;

    public int currentPointIndex = 0;

    private void Start()
    {
        // Set the initial camera position and rotation to the first point on the path
        transform.position = pathPoints[currentPointIndex].position;
        transform.rotation = pathPoints[currentPointIndex].rotation;
    }

    private void Update()
    {
        // Find the closest point on the path to the player
        int closestPointIndex = FindClosestPointIndex();

        // Move and rotate towards the closest point on the path
        MoveTowardsPoint(pathPoints[closestPointIndex]);
        RotateTowardsPoint(pathPoints[closestPointIndex]);
    }

    private void MoveTowardsPoint(Transform targetPoint)
    {
        // Move the camera towards the target point using Lerp for smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        // Check if the camera has reached the target point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Move to the next point on the path
            currentPointIndex = (currentPointIndex + 1) % pathPoints.Length;
        }
    }

    private void RotateTowardsPoint(Transform targetPoint)
    {
        // Rotate the camera towards the target point using Slerp for smooth rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetPoint.rotation, rotationSpeed * Time.deltaTime);
    }

    private int FindClosestPointIndex()
    {
        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;

        // Find the closest point on the path to the player
        for (int i = 0; i < pathPoints.Length; i++)
        {
            float distance = Vector3.Distance(pathPoints[i].position, player.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}