using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthPickUp : MonoBehaviour
{
    public event Action OnPickUp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var playerData = collision.gameObject.GetComponent<NewPhysicMovement>();
            if(playerData.health < 5 && !playerData.isPickingUp)
            {
                StartCoroutine(SetFalse(playerData));
                playerData.isPickingUp = true;
                playerData.health++;
                Color color = gameObject.GetComponent<Renderer>().material.color;
                color.a = 0;
                gameObject.GetComponent<Renderer>().material.color = color;
            }
        }
    }
    private IEnumerator SetFalse(NewPhysicMovement script)
    {
        yield return new WaitForSeconds(.1f);
        script.isPickingUp = false;
        OnPickUp.Invoke();
        Destroy(gameObject);
    }
}
