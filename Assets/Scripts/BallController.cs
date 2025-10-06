using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [SerializeField] private float bounceForce = 1.5f;
    private Rigidbody rb;

    private bool hasBouncedThisFrame = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        hasBouncedThisFrame = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasBouncedThisFrame) return;

        Vector3 normal = collision.contacts[0].normal;

        if (collision.gameObject.CompareTag("Platform") && normal.y > 0.5f)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * bounceForce, ForceMode.VelocityChange);
            hasBouncedThisFrame = true;
        }
        else if (collision.gameObject.CompareTag("Deadly"))
        {
            GameManager.Instance.OnDeadlyHit();
            Debug.Log("Ball hit deadly wedge!");
        }
    }
}
