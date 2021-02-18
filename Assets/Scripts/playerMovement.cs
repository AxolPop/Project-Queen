using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    public Transform target;

    public CharacterController controller;
    public Transform cam;


    public float speed = 6f;

    Image healthValue;
    public float playerHealth = 30;
    float maxHealth;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector3 moveVector;

    bool allowedToDamage = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "damage area" && allowedToDamage == true)
        {
            playerHealth -= 10;
            allowedToDamage = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "damage area" && allowedToDamage == false)
        {
            allowedToDamage = true;
        }
    }

    private void Start()
    {
        healthValue = gameObject.transform.Find("Health/Health Value").GetComponent<Image>();
        playerHealth = 30;
        maxHealth = playerHealth;
}

    void Update()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);


        }

        //REeset the MoveVector
        moveVector = Vector3.zero;

        //Check if cjharacter is grounded
        if (controller.isGrounded == false)
        {
            //Add our gravity Vecotr
            moveVector += Physics.gravity;
        }

        //Apply our move Vector , remeber to multiply by Time.delta
        controller.Move(moveVector * Time.deltaTime);

        health();
    }

    void health()
    {
        healthValue.fillAmount = playerHealth / maxHealth;
        Debug.Log(playerHealth);
    }
}