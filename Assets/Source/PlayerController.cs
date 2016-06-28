using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	public float m_moveSpeed = 1f;

	private Vector3 m_mousePosition;
	private Ball[] m_balls = new Ball[9];
	private Ball m_currentBall = null;

	private Vector3[] m_moveTarget = new Vector3[9];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		m_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		m_mousePosition.y = 1;

		MoveBall(Input.GetButton("Move"));

		for (int i = 1; i < 10; i += 1) {
			SelectBall(i, Input.GetButton("Select" + i));
		}
	}

	void FixedUpdate() {
		for (int i = 0; i < 9; i += 1) {
			if (m_moveTarget[i] != Vector3.zero) {
				float distance = m_moveSpeed * Time.deltaTime / Vector3.Distance(m_moveTarget[i], m_balls[i].transform.localPosition);
				float dx = distance * (m_moveTarget[i].x - m_balls[i].transform.position.x);
				float dz = distance * (m_moveTarget[i].z - m_balls[i].transform.position.z);

				if (distance >= 1) {
					dx = m_moveTarget[i].x - m_balls[i].transform.position.x;
					dz = m_moveTarget[i].z - m_balls[i].transform.position.z;

					m_moveTarget[i] = Vector3.zero;
				}

				m_balls[i].GetComponent<Rigidbody>().MovePosition(m_balls[i].transform.position + new Vector3(dx, 0, dz));

				m_balls[i].m_laser.SetPosition(0, m_balls[i].transform.position);

				if (m_moveTarget[i] == Vector3.zero) {
					m_balls[i].m_laser.SetPosition(1, m_balls[i].transform.position);
				} else {
					m_balls[i].m_laser.SetPosition(1, m_moveTarget[i]);
				}
			}
		}
	}

	public void AddBall(Ball ball) {
		bool addedBall = false;

		for (int i = 0; i < m_balls.Length; i += 1) {
			if (!m_balls[i]) {
				m_balls[i] = ball;
				ball.SetId(i);
				ball.m_number.GetComponent<TextMesh>().color = Color.black;

				if (!m_currentBall) {
					SelectBall(i + 1, true);
					//m_currentBall = ball;
				}

				addedBall = true;

				break;
			}
		}

		if (!addedBall) {
			Destroy(ball.gameObject);
		}
	}

	public void RemoveBall(Ball ball, bool loseCondition) {
		if (m_currentBall == ball) {
			m_currentBall = null;
		}

		for (int i = 0; i < m_balls.Length; i += 1) {
			if (m_balls[i] == ball) {
				m_balls[i] = null;
				m_moveTarget[i] = Vector3.zero;

				if (m_currentBall) {
					break;
				}
			} else if (!m_currentBall && m_balls[i]) {
				SelectBall(i + 1, true);
			}
		}

		if (loseCondition && !m_currentBall) {
			EndGame(false);
		}
	}

	private void SelectBall(int index, bool select) {
		if (select) {
			if (m_balls[index - 1]) {
				if (m_currentBall) {
					m_currentBall.m_number.GetComponent<TextMesh>().color = Color.black;
				}

				m_currentBall = m_balls[index - 1];
				m_currentBall.m_number.GetComponent<TextMesh>().color = Color.white;
			}
		}
	}

	private void MoveBall(bool move) {
		if (m_currentBall && move) {
			m_moveTarget[m_currentBall.GetId()] = m_mousePosition;
		}
	}

	private void EndGame(bool victory) {
		SceneManager.LoadScene("EndGame");
	}
}
