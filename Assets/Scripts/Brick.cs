using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    public GameObject brickParticles;

    void OnCollisionEnter() {
        var particles = Instantiate(brickParticles, transform.position, Quaternion.identity);
        GameManager.instance.DestroyBrick();
        Destroy(this.gameObject);
    }
}
