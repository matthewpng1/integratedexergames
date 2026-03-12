using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed = 5f; // Speed at which the object moves toward the center
    private Vector3 targetPosition; // Target position (center of the screen)
    public bool isInZone = false; // Flag to indicate if the object is in the scoring zone
    public bool hasBeenScored = false; // Flag to prevent multiple scoring on the same object

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the target position to the center (x=0), keeping the current y and z
        targetPosition = new Vector3(0f, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object toward the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the object has reached the zone
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            isInZone = true;
        }
    }
}
