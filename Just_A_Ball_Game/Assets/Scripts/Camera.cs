using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    private GameObject player;
    private Vector3 diff;


    // Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
        diff = transform.position - player.transform.position;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.transform.position + diff;
	}
}
