using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour {

    private Rigidbody rb;
    public float speed;
    public float jumpForce;
    public float boostForce;
    private bool isGrounded = false;
    public static int lives;
    public Text livesText;
    public static bool lost = false;
    public GameObject lostScreen;

    private void Start()
    {
        speed = 18;
        jumpForce = 15;
        boostForce = 30;
        lives = 3;
        livesText.text = "Lives: " + lives.ToString();
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody>();
        Debug.Log(speed);
        Debug.Log(jumpForce);
        Debug.Log(boostForce);
    }

    void FixedUpdate () {
        if (!lost){
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            rb.AddForce(new Vector3(horizontal, 0f, vertical) * speed);

            if (Input.GetKeyDown(KeyCode.Space) && (isGrounded))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }

        if (transform.position.y < -3){
            Debug.Log("Restart");

            GetComponent<TrailRenderer>().Clear();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.position = new Vector3(0, 0.5f, 0);
            lives -= 1;
            livesText.text = "Lives: " + lives.ToString();
            if (lives == 0){
                Time.timeScale = 1f;
                lostScreen.SetActive(true);
                lost = true;
            }

        }
    }

    void OnCollisionEnter(Collision col)
    {
        isGrounded = true;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Booster")
        {
            rb.AddForce(new Vector3(0, 0, 1) * boostForce, ForceMode.Impulse);
            Debug.Log("Boost");
        }
    }
}
