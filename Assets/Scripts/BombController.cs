using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float powerMultiplier = 10f; // The multiplier for the power of the shot
    public float maxPower = 100f; // The maximum power of the shot
    public float delayBetweenLives = 3;

    private Rigidbody2D rb; // The Rigidbody2D component of the bomb
    private Animator anim;
    private Vector3 dragStartPosition; // The starting position of the drag
    private Vector3 dragEndPosition; // The ending position of the drag
    public float maxDragDistance;
    private bool isDragging = false; // Whether or not the player is currently dragging the bomb
    private float power; // The power of the shot
    private bool isMoving = false;
    private bool blownUp = false;

    public LineRenderer lineRenderer;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (blownUp || SceneManager.instance.sceneOver)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.5f);
        }
        else
        {
            if (Vector2.Distance(rb.velocity, Vector2.zero) > 0.7f) // Bomb is moving
            {
                isMoving = true;
                rb.freezeRotation = true;

                // Calculate the direction of the velocity vector
                Vector2 direction = rb.velocity.normalized;
                // Calculate the angle from the direction vector
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                // Set the rotation of the bomb to match the direction of the velocity vector
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            else if (Vector2.Distance(rb.velocity, Vector2.zero) <= 0.7f) // Bomb is stopped
            {
                isMoving = false;
                rb.velocity = Vector2.zero;
                if(SceneManager.instance.currentDurability < SceneManager.instance.maxDurability)
                {
                    SceneManager.instance.DecrementClock();
                }
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
                print(dragPercentage);
                power = Mathf.Clamp(dragPercentage * powerMultiplier, 0f, maxPower);

                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position);
                float maxDistance = 0.5f * ((power / maxPower) * maxDragDistance);
                Vector3 direction = (dragStartPosition - dragEndPosition).normalized;
                Vector3 secondPos = transform.position + direction * Mathf.Clamp(dragDistance, 0, maxDistance);
 
                lineRenderer.SetPosition(1, secondPos);
            }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                isDragging = false;
                lineRenderer.enabled = false;
                HitBomb();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                BlowUp();
            }
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
            GameManager.instance.soundEffectSource.PlayOneShot(GameManager.instance.soundEffects.hit);

            StartCoroutine(BombSqueeze(1.5f, 0.2f, 0.05f));
        }
    }

    void SetAnimation()
    {
        anim.SetFloat("total_vel", Mathf.Abs(rb.velocity.x + rb.velocity.y));
    }

    public void BlowUp()
    {
        GameManager.instance.soundEffectSource.PlayOneShot(GameManager.instance.soundEffects.explosion);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        blownUp = true;
        StartCoroutine(WaitToReset());
    }

    private IEnumerator WaitToReset()
    {
        anim.Play("Explosion");
        IEnumerator shake = Camera.main.GetComponent<CameraController>().Shake();
        StartCoroutine(shake);
        yield return new WaitForSeconds(delayBetweenLives);
        SceneManager.instance.LoseLife();
        blownUp = false;
        // Modify explosion properties
        transform.rotation = Quaternion.Euler(0, 0, 90);
        transform.localScale = Vector3.one;

        anim.Play("BombIdle");
        StopCoroutine(WaitToReset());
        StopCoroutine(shake);
    }

    // Adapted from Press Start on Youtube
    IEnumerator BombSqueeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 2.0)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 2.0)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }

    }
}
