using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmallHedge.SoundManager;


public class SourceTree : Source
{
    public override void DropResource(float dummy)
    {
        StartCoroutine(DropResourcesCoroutine(dummy));
        base.DropResource(dummy);
    }

    private IEnumerator DropResourcesCoroutine(float dummy)
    {

        foreach (var resource in resourceDrops)
        {
            if (resource.dropChance > dummy) continue;
            GameObject itemToDrop = itemManager.GetGameObject(resource.item.id);

            for (uint i = 0; i < resource.value; i++)
            {
                GameObject drop = Instantiate(itemToDrop, dropPoint.position, Quaternion.identity);
                Vector3 rotation = new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
                drop.GetComponent<ItemPickUp>().LaunchInDirection(rotation, 5f);
                SoundManager.PlaySound(SoundType.ItemSpawned);

                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
