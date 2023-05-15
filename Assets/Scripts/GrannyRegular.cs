using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyRegular : MonoBehaviour
{
    public float speed;
    public Vector2 startingDirection;
    Vector2 currentDirection;
    public float timeBetweenDirectionChange;
    Animator anim;
    Rigidbody2D rb;
    public float startingWalkTime;
    float currentWalkTime;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentDirection = startingDirection;
        currentWalkTime = startingWalkTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWalkTime <= 0)
        {
            StartCoroutine(PickNewDirection());
        }
        else
        {
            currentWalkTime -= Time.deltaTime;
            rb.velocity = currentDirection * speed * Time.deltaTime;
            anim.SetFloat("move_x", currentDirection.x);
            anim.SetFloat("move_y", currentDirection.y);
        }   
    }

    private IEnumerator PickNewDirection()
    {
        currentWalkTime = startingWalkTime;
        yield return new WaitForSeconds(timeBetweenDirectionChange);
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(-1, 1);
        currentDirection = new Vector2(randomX, randomY);
        StopAllCoroutines();
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<BombController>().BlowUp();
            Destroy(this);
        }
    }
}
