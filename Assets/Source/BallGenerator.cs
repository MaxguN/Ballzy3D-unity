using UnityEngine;
using System.Collections;

public class BallGenerator : MonoBehaviour {
	public Transform m_spawn;
	public Ball[] m_levels;

	private PlayerController m_playerController;

	// Use this for initialization
	void Start () {
		m_playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

		m_playerController.AddBall((Ball) Instantiate(m_levels[0], m_spawn.localPosition, Quaternion.identity));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NewBall(int level, Vector3 position) {
		m_playerController.AddBall((Ball)Instantiate(m_levels[level - 1], position, Quaternion.identity));
	}
}
