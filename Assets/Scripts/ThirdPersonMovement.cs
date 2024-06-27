using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
   
    private float turnSmoothVelocity;

    private bool isUIActive = false;

    private ItemManager itemManager;

    private void Start()
    {
        itemManager = ItemManager.Instance;
    }

    void Update()
    {
      
        if (isUIActive) return;

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

    public void DropItemFromPlayerPos(uint itemID)
    {
        //Dropt it from player's position 
        Vector3 playerPos = transform.position + transform.forward * 3; //magic number 3
        Quaternion playerRot = transform.rotation;

        //Add Instantiate 
        GameObject itemToDrop = itemManager.GetGameObject(itemID);
        GameObject drop = Instantiate(itemToDrop, playerPos, playerRot);
        drop.GetComponent<ItemPickUp>().LaunchInDirection(transform.forward + transform.up, 5f); //magic number 5
    }

    public void ToggleUI(bool isActive)
    {
        isUIActive = isActive;
    }
}

