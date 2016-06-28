using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	public int level = 0;
	public LineRenderer m_laser;
	public TextMesh m_number;

	private int m_id = 0;
	private Rigidbody m_rigidbody;

	// Use this for initialization
	void Start () {
		m_rigidbody = GetComponent<Rigidbody>();
		//GetComponent<Alive>().SetColor(GetComponent<MeshRenderer>().materials[0]);
		//GetComponent<Alive>().SetRadius(1);
	}
	
	// Update is called once per frame
	void Update () {
		if (m_rigidbody.velocity != Vector3.zero) {
			m_rigidbody.velocity = Vector3.zero;
			m_rigidbody.angularVelocity = Vector3.zero;
			transform.rotation = Quaternion.identity;
		}
	}

	public void SetId(int id) {
		m_id = id;
		m_number.text = "" + (m_id + 1);
	}

	public int GetId() {
		return m_id;
	}

	public void Kill(bool loseCondition = true) {
		GameObject.FindWithTag("Player").GetComponent<PlayerController>().RemoveBall(this, loseCondition);
		Destroy(gameObject);
	}
}
