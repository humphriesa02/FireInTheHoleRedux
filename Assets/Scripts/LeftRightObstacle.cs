using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightObstacle : MonoBehaviour
{
    public Vector2 leftPosition;
    public Vector2 rightPosition;
    private Vector2 destinationVector;
    public float speed;

    private void Start()
    {
        destinationVector = leftPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, leftPosition) < 0.5f)
        {
            destinationVector = rightPosition;
        }
        else if(Vector2.Distance(transform.position, rightPosition) < 0.5f)
        {
            destinationVector = leftPosition;
        }
        float speedOffset = Random.Range(0.1f, 1f);
        transform.position = Vector2.Lerp(transform.position, destinationVector, Time.deltaTime * (speed + speedOffset));
    }
}
