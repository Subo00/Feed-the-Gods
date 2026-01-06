using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    HashSet<Collider> collidersInside = new HashSet<Collider>();
    private void OnTriggerEnter(Collider other)
    {
        collidersInside.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        collidersInside.Remove(other);
    }

    public bool isColliderEmpty()
    {
        collidersInside.RemoveWhere(collider => collider == null);
        ColliderCheck();

        return collidersInside.Count == 0;
    }

    private void ColliderCheck()
    {
        //collidersInside.RemoveWhere(collider => collider.tag == "Player");
        collidersInside.RemoveWhere(collider => collider.tag == "PlayerInteract");
        collidersInside.RemoveWhere(collider => collider.tag == "Floor");
        collidersInside.RemoveWhere(collider => collider.tag == "PlayerController");

    }
}
