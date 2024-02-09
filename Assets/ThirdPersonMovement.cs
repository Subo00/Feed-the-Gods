using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;


    // Update is called once per frame
    void FixedUpdate()
    {
        //takes input from keyboard 
        float horiznotal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //normalize directions so that when its moving diagonaly the speed is not dubled
        Vector3 direction = new Vector3 (horiznotal, 0f, vertical).normalized; 
        
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction * speed * Time.deltaTime);
        }

    }
}
