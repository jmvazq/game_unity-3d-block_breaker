using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour {

    void OnTriggerEnter()
    {
        GameManager.instance.LoseLife();
    }
}
