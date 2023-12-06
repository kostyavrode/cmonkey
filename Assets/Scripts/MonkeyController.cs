using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;
    public Transform startPosition;
    public bool isAlive;
    public float moveSpeed;
    public Joystick joystick;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        GameManager.onGameStarted += StartMove;
        if (rb==null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }
    private void OnDisable()
    {
        GameManager.onGameStarted -= StartMove;
    }
    private void Move()
    {
        if (transform.position.x < 12.8f && transform.position.x > 8.5f)
        {
            transform.position = new Vector3(transform.position.x - joystick.Horizontal*moveSpeed*1.5f*Time.deltaTime, transform.position.y + moveSpeed * Time.deltaTime, transform.position.z);
            transform.rotation = Quaternion.Euler(transform.rotation.x, -180f, transform.rotation.z - joystick.Horizontal * 15f);
        }
        else if (transform.position.x > 12.8f)
        {
            transform.position = new Vector3(transform.position.x-0.1f* moveSpeed * 2 * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x + 0.1f* moveSpeed * 2 * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }
    private void StartMove()
    {
        transform.position = startPosition.position;
        transform.Rotate(new Vector3(0, 180, 0));
        animator.SetBool("isMoving", true);
        isAlive = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Obstacle")
        {
            StartFalling();
        }
    }
    private void StartFalling()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isFalling", true);
        isAlive = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        StartCoroutine(GameManager.instance.ShowLosePanel());
    }
    private void Update()
    {
        if (GameManager.instance.IsGameStarted() && isAlive)
        {
            Move();
        }
    }
}
