using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public float initialVelocity = 600f; // used to add energy to our ball and start movement

    Rigidbody _rigidBody;
    bool _ballInPlay = false;

    // Before initialization
    void Awake () {
        _rigidBody = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Fire1") && !_ballInPlay && GameManager.instance.inputAllowed)
        {
            transform.parent = null;
            _ballInPlay = true;
            _rigidBody.isKinematic = false;
            _rigidBody.AddForce(new Vector3(initialVelocity, initialVelocity, 0)); // REVISE this later, see how we can randomize direction
        }
    }
}
