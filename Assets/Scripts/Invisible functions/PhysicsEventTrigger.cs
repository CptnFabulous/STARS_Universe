using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Place this component on a collider so it can invoke collider-based functions from elsewhere.
/// </summary>
public class PhysicsEventTrigger : MonoBehaviour
{
    [Header("Collisions")]
    public UnityEvent<Collision> onCollisionEnter;
    public UnityEvent<Collision> onCollisionStay;
    public UnityEvent<Collision> onCollisionExit;
    [Header("Triggers")]
    public UnityEvent<Collider> onTriggerEnter;
    public UnityEvent<Collider> onTriggerStay;
    public UnityEvent<Collider> onTriggerExit;

    new public Collider collider
    {
        get
        {
            if (c == null)
            {
                c = GetComponent<Collider>();
            }
            return c;
        }
    }
    Collider c;

    public void OnCollisionEnter(Collision collision) => onCollisionEnter.Invoke(collision);
    public void OnCollisionStay(Collision collision) => onCollisionStay.Invoke(collision);
    public void OnCollisionExit(Collision collision) => onCollisionExit.Invoke(collision);
    public void OnTriggerEnter(Collider other) => onTriggerEnter.Invoke(other);
    public void OnTriggerStay(Collider other) => onTriggerStay.Invoke(other);
    public void OnTriggerExit(Collider other) => onTriggerExit.Invoke(other);
}
