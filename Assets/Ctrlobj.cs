using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrefabType
{
    Cube,
    Hon,
    Ve
}

public class Ctrlobj : MonoBehaviour
{
    public PrefabType type;
    private Rigidbody rb;

    private bool isWait;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.velocity.magnitude < 0.1f)
        {
            //transform.localPosition
        }

        if (transform.position.y <= -0.2f)
        {
            if (transform.parent != null)
            {
                transform.parent = null;
            }

            Destroy(this.gameObject);
        }
    }

    public void Down()
    {
        transform.localPosition -= Vector3.up;
    }

    private void OnCollisionEnter(Collision other)
    {
        rb.freezeRotation = true;
        if (transform.parent == null)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Stuff"))
            {
                transform.SetParent(other.transform);
                transform.localPosition = Vector3.up;
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                transform.SetParent(other.transform.parent);
                transform.localPosition = Vector3.up * (other.gameObject.transform.childCount + 1);
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        rb.useGravity = false;
    }
}