using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float paddleSpeed = 1f;

    Vector3 _playerPos; // original position of our paddle

    void Start()
    {
        _playerPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.inputAllowed)
        {
            float xPos = transform.position.x + (Input.GetAxis("Horizontal") * paddleSpeed); // update xPos based on player input
            _playerPos = new Vector3(Mathf.Clamp(xPos, -7.5f, 7.5f), transform.position.y, transform.position.z); // limit xPos to designated paddle area
            transform.position = _playerPos; // update paddle's position
        }
    }
}
