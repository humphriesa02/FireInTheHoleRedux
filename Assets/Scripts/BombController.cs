using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float powerMultiplier = 10f; // The multiplier for the power of the shot
    public float maxPower = 100f; // The maximum power of the shot

    private Rigidbody2D rb; // The Rigidbody2D component of the bomb
    private Animator anim;
    private Vector3 dragStartPosition; // The starting position of the drag
    private Vector3 dragEndPosition; // The ending position of the drag
    public float maxDragDistance;
    private bool isDragging = false; // Whether or not the player is currently dragging the bomb
    private float power; // The power of the shot
    private bool isMoving = false;

    public LineRenderer lineRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(Vector2.Distance(rb.velocity, Vector2.zero) > 0.7f)
        {
            isMoving = true;
            rb.freezeRotation = true;
        }
        else if(Vector2.Distance(rb.velocity, Vector2.zero) <= 0.7f)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
        }

        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                dragStartPosition = Input.mousePosition;
            }
        }

        if (isDragging)
        {
            dragEndPosition = Input.mousePosition;
            float dragDistance = Vector3.Distance(dragStartPosition, dragEndPosition);
            float dragPercentage = dragDistance / maxDragDistance;
            power = Mathf.Clamp(dragPercentage * powerMultiplier, 0f, maxPower);

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + (dragStartPosition - dragEndPosition).normalized * dragDistance / maxDragDistance);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            lineRenderer.enabled = false;
            Vector2 direction = (dragStartPosition - dragEndPosition).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            print(power);
            rb.AddForce(direction * power);
        }

        SetAnimation();
    }

    void SetAnimation()
    {
        anim.SetFloat("total_vel", Mathf.Abs(rb.velocity.x + rb.velocity.y));
    }
}
