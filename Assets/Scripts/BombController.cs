using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float powerMultiplier = 10f; // The multiplier for the power of the shot
    public float maxPower = 100f; // The maximum power of the shot
    public float delayBetweenLives = 3;
    private bool delayingReset;

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
        delayingReset = false;
    }

    void Update()
    {
        if(Vector2.Distance(rb.velocity, Vector2.zero) > 0.7f) // Bomb is moving
        {
            isMoving = true;
            rb.freezeRotation = true;
        }
        else if(Vector2.Distance(rb.velocity, Vector2.zero) <= 0.7f) // Bomb is stopped
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
            if (!delayingReset)
            {
                SceneManager.instance.DecrementClock();
            }
        }

        if (Input.GetMouseButtonDown(0) && !isMoving && !delayingReset)
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
            print(dragPercentage);
            power = Mathf.Clamp(dragPercentage * powerMultiplier, 0f, maxPower);
            //print(power);

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            float maxDistance = 0.5f * ((power / maxPower) * maxDragDistance);
            print(maxDistance);
            Vector3 direction = (dragStartPosition - dragEndPosition).normalized;
            Vector3 secondPos = transform.position + direction * Mathf.Clamp(dragDistance, 0, maxDistance);
            print(secondPos);
            lineRenderer.SetPosition(1, secondPos);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            lineRenderer.enabled = false;
            HitBomb();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !delayingReset)
        {
            BlowUp();
        }

        SetAnimation();
    }

    void HitBomb()
    {
        SceneManager.instance.currentDurability--;
        SceneManager.instance.sceneDurability++;
        if(SceneManager.instance.currentDurability < 0)
        {
            BlowUp();
        }
        else
        {
            Vector2 direction = (dragStartPosition - dragEndPosition).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rb.AddForce(direction * power);
        }
    }

    void SetAnimation()
    {
        anim.SetFloat("total_vel", Mathf.Abs(rb.velocity.x + rb.velocity.y));
    }

    public void BlowUp()
    {
        // Play blowup sound
        // Show explosion animation
        // Decrement scene totalLives
        // Reset position
        rb.velocity = Vector2.zero;
        StartCoroutine(WaitToReset());
    }

    private IEnumerator WaitToReset()
    {
        delayingReset = true;
        anim.Play("Explosion");
        yield return new WaitForSeconds(delayBetweenLives);
        SceneManager.instance.LoseLife();
        delayingReset = false;
        anim.Play("BombIdle");
        StopCoroutine(WaitToReset());
    }
}
