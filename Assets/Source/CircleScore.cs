using UnityEngine;
using System.Collections;

public class CircleScore : MonoBehaviour {
	public float m_duration = 2;

	private TextMesh m_scoreMesh;
	private float m_radius = 0;

	private float m_distance = 0;
	private float m_countdown = 0;  

	// Use this for initialization
	void Start () {
		m_scoreMesh = transform.GetChild(0).GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (m_scoreMesh) {
			if (m_countdown > 0) {
				m_countdown -= Time.deltaTime;

				float distance = Mathf.Max(0, m_radius - 1 / (m_duration * (m_duration - m_countdown)));
				float delta = distance - m_distance;

				m_scoreMesh.transform.localPosition += delta * Vector3.forward;

				m_distance = distance;
			} else if (m_distance > 0) {
				Destroy(m_scoreMesh.gameObject);

				m_distance = 0;
			}
		}
	}

	public void DisplayScore(int score, float radius) {
		m_scoreMesh.text = "+" + score;
		m_radius = radius + 1.5f;

		m_countdown = m_duration;
	}
}
