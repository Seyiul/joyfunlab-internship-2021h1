using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody))]
public class SC_IRPlayer : MonoBehaviour
{
    public int sceneIndex = 1;


    public float gravity = 20.0f;
    public float jumpHeight = 2.5f;
    public int life = 3;
    public Rigidbody r;
    bool grounded = false;
    Vector3 defaultScale;
    bool crouch = false;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        r.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        r.freezeRotation = true;
        r.useGravity = false;
        defaultScale = transform.localScale;
    }
    void Update()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            r.velocity = new Vector3(r.velocity.x, CalculateJumpVerticalSpeed(), r.velocity.z);
        }
        //float h = Input.GetAxisRaw("Horizontal") * 2.25f;
        //transform.Translate(Vector3.right * h * Time.deltaTime * 1.0f);
        if (Input.GetKeyDown(KeyCode.A) && grounded)
        {
            if (transform.position.x != -2.25)
                transform.Translate(new Vector3(-2.25f, r.velocity.y, r.velocity.z));
        }
        if (Input.GetKeyDown(KeyCode.D) && grounded)
        {
            if (transform.position.x != 2.25)
                transform.Translate(new Vector3(2.25f, r.velocity.y, r.velocity.z));
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // We apply gravity manually for more tuning control
        gravity = Mathf.Round(2.5f * SC_GroundGenerator.instance.movingSpeed);
        r.AddForce(new Vector3(0, -gravity * r.mass, 0));
        grounded = false;
    }
    void OnCollisionStay()
    {
        grounded = true;
    }
    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish" || collision.gameObject.tag == "Trap Tile")
        {
            //print("GameOver!");
            if (life == 1)
            {
                life--;
                SC_GroundGenerator.instance.gameOver = true;

            }
            else if(life>1)
                life--;
        }
        if (collision.gameObject.tag == "Battle Tile")
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "life")
        {
            life++;
            col.gameObject.GetComponent<Renderer>().enabled = false;
        }
        
    }
    void OnTriggerExit(Collider col)
    {
    }
    void OnGUI()
    {
        GUI.Label(new Rect(300, 30, 200, 25), "Life: " + ((int)life));
    }
}