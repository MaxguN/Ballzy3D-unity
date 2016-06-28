using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	public float m_speed = 1;
	public float m_acceleration = 1;

	private float m_timer = 0;
	private float m_threshold = 180; //seconds

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		m_timer += Time.deltaTime;
		float speed = m_speed + m_speed * m_acceleration * m_timer / m_threshold;

		transform.localPosition = transform.localPosition + Vector3.forward * speed * Time.deltaTime;
	}
}
