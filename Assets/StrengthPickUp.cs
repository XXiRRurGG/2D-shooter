using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StrengthPickUp : MonoBehaviour
{
    public event Action OnPickUp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var script =collision.gameObject.GetComponent<NewPhysicMovement>();
            if (!script.isPickingUp)
            {
                StartCoroutine(SetFalse(script));
                script.isPickingUp = true;
                script.damage++;
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
