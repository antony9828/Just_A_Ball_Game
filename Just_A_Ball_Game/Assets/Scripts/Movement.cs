using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour {

    private Rigidbody rb;
    public float speed;
    public float jumpForce;
    public float boostForce;
    private bool isGrounded = false;
    public static int lives;
    public Text livesText;
    public Text firstSteps;
    public static bool lost = false;
    public GameObject lostScreen;
    private bool buttonActive = true;
    private GameObject button;
    public GameObject platform;
    public GameObject[] deathzones;
    private IEnumerator coroutine;

    private void Start()
    {
        lives = 3;
        livesText.text = "Lives: " + lives.ToString();
        if (firstSteps != null)
        {
            firstSteps.text = "WELCOME! TO MOVE AROUND USE \"W\", \"A\", \"S\" AND \"D\"";
        }
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody>();
        coroutine = Timer(2.0f);
        StartCoroutine(coroutine);
    }

    private IEnumerator Timer(float waitTime)
    {
        while ((true) && (SceneManager.GetActiveScene().buildIndex == 3))
        {
            deathzones[0].SetActive(true);
            deathzones[1].SetActive(false);
            deathzones[2].SetActive(false);
            deathzones[3].SetActive(true);
            deathzones[4].SetActive(false);
            deathzones[5].SetActive(false);
            Debug.Log("stage 1");
            yield return new WaitForSeconds(waitTime);
            deathzones[0].SetActive(false);
            deathzones[1].SetActive(true);
            deathzones[2].SetActive(false);
            deathzones[3].SetActive(false);
            deathzones[4].SetActive(true);
            deathzones[5].SetActive(false);
            Debug.Log("stage 2");
            yield return new WaitForSeconds(waitTime);
            deathzones[0].SetActive(false);
            deathzones[1].SetActive(false);
            deathzones[2].SetActive(true);
            deathzones[3].SetActive(false);
            deathzones[4].SetActive(false);
            deathzones[5].SetActive(true);
            Debug.Log("stage 3");
            yield return new WaitForSeconds(waitTime);
        }
    }

    void FixedUpdate () {

        //controls
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

        //out of bounds
        if (transform.position.y < -3){
            Debug.Log("Restart");
            Reset();


        }

        //button
        if ((!buttonActive) && (button != null)){
            button.transform.position = Vector3.MoveTowards(button.transform.position, button.transform.position - new Vector3(0,1,0), Time.deltaTime);
        }

        //platform
        if (!buttonActive){
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, platform.transform.position - new Vector3(0, 1, 0), Time.deltaTime * 3);
        }
        if ((platform != null) && platform.transform.position.y <= 0){
            buttonActive = true;
            platform.transform.position = new Vector3(platform.transform.position.x, 0, platform.transform.position.z);
        }


    }

    private void Reset()
    {
        GetComponent<TrailRenderer>().Clear();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = new Vector3(0, 0.5f, 0);
        lives -= 1;
        livesText.text = "Lives: " + lives.ToString();
        if (lives == 0)
        {
            Time.timeScale = 1f;
            lostScreen.SetActive(true);
            lost = true;
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
            Debug.Log("Boost");

            switch (other.gameObject.GetComponent<Booster>().direction.ToString()){
                case ("z"):
                    rb.AddForce(new Vector3(0, 0, 1) * boostForce, ForceMode.Impulse);
                    break;
                case ("zr"):
                    rb.AddForce(new Vector3(0, 0, -1) * boostForce, ForceMode.Impulse);
                    break;
                case ("x"):
                    rb.AddForce(new Vector3(1, 0, 0) * boostForce, ForceMode.Impulse);
                    break;
                case ("xr"):
                    rb.AddForce(new Vector3(-1, 0, 0) * boostForce, ForceMode.Impulse);
                    break;
                case ("y"):
                    rb.AddForce(new Vector3(0, 1, 0) * boostForce, ForceMode.Impulse);
                    break;
                case ("yr"):
                    rb.AddForce(new Vector3(0, -1, 0) * boostForce, ForceMode.Impulse);
                    break;
            }

           }
        if (other.gameObject.name == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("NextLevel");
        }
        if (other.gameObject.name == "JumpHelp")
        {
            firstSteps.text = "TO JUMP PRESS \"SPACE\"";
        }
        if (other.gameObject.name == "BoosterHelp")
        {
            firstSteps.text = "THIS IS A BOOSTER IT WILL GIVE YOU MORE SPEED";
        }
        if (other.gameObject.name == "NextLevelHelp")
        {
            firstSteps.text = "FOLLOW THE PARTICLES, IT WILL TAKE YOU TO THE NEXT LEVEL";
        }
        if ((other.gameObject.name == "ButtonPressed") && (buttonActive))
        {
            Debug.Log("Button");
            buttonActive = false;
            button = GameObject.FindGameObjectWithTag("Button");
            Destroy(button, 1);
            Destroy(other);
        }
        if ((other.gameObject.name == "DeathZone"))
        {
            Reset();
        }
        if ((other.gameObject.name == "Teleporter"))
        {
            GetComponent<TrailRenderer>().Clear();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.position = other.gameObject.GetComponent<Teleporter>().destination;
        }
        if ((other.gameObject.name == "Enemy"))
        {
            Reset();
        }
    }
}
