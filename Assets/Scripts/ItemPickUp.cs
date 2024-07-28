using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData item;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (InventoryManager.Instance.Add(item) == true)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void LaunchInDirection(Vector3 direction, float launchForce = 7f)
    {
        // Ensure the object has a Rigidbody component
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply force in the specified direction
            rb.AddForce(direction.normalized * launchForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("Item does not have a Rigidbody component.");
        }
    }
}
