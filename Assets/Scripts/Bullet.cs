using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 10f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject,5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider != null)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider != null)
        {
            Destroy(this.gameObject);
        }
    }
}
