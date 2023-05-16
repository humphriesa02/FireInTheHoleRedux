using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrannyRegular : MonoBehaviour
{
    public float speed;
    public Vector2 startingDirection;
    Vector2 currentDirection;
    public float timeBetweenDirectionChange;
    public float yMaxLimit;
    public float yMinLimit;
    public TextMeshProUGUI granniesKilledText;
    public GameObject howCouldYou;
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
            currentDirection.x += 1f;
            currentDirection.y += 1f;

            if (currentDirection.x > 1f)
                currentDirection.x = -1f;
            if (currentDirection.y > 1f)
                currentDirection.y = -1f;
            print(currentDirection);
            currentWalkTime = startingWalkTime;
        }
        else
        {
            currentWalkTime -= Time.deltaTime;
            rb.velocity = currentDirection * speed * Time.deltaTime;
            transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, yMinLimit, yMaxLimit));
            anim.SetFloat("move_x", currentDirection.x);
            anim.SetFloat("move_y", currentDirection.y);
        }   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<BombController>().BlowUp();
            GameManager.instance.soundEffectSource.PlayOneShot(GameManager.instance.soundEffects.grannyDeath);
            granniesKilledText.text = "1";
            howCouldYou.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
