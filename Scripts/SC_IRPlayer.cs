using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class SC_IRPlayer : MonoBehaviour
{
    public float gravity = 20.0f;
    public float jumpHeight = 2.5f;
    public int life = 3;
    public Rigidbody r;
    bool grounded = true;
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
        gravity = 2.5f * SC_GroundGenerator.instance.movingSpeed;
        r.AddForce(new Vector3(0, -gravity * r.mass, 0));
        grounded = false;
    }
    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
    void OnCollisionStay(Collision col)
    {
        if(col.gameObject.tag == "ground")
            grounded = true;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Finish")
        {
            if (life == 1)
            {
                life--;
                SC_GroundGenerator.instance.gameOver = true;
            }
            else if (life > 1)
                life--;
        }

        if (col.gameObject.tag == "life")
        {
            life++;
            col.gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
    void OnTriggerStay(Collider col)
    {
        grounded = false;
        if (col.gameObject.tag == "Finish")
        {
//            SC_GroundGenerator.instance.movingSpeed = 0;
            this.GetComponent<Renderer>().material.color = Color.white;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Finish")
        {
            this.GetComponent<Renderer>().material.color = Color.red;
        }
    }
    void OnGUI()
    {
        GUI.Label(new Rect(5, 25, 200, 25), "Life: " + ((int)life));
    }
}
