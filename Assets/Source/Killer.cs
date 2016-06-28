using UnityEngine;
using System.Collections;

public class Killer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		Alive livingThing = other.GetComponent<Alive>();

		if (livingThing) {
			livingThing.Kill();
		}
	}
}
