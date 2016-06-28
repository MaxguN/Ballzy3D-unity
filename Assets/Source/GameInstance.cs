using UnityEngine;
using System.Collections;

public class GameInstance : MonoBehaviour {
	public int m_scoreFactor = 37;

	private TextMesh m_scoreDisplay;
	private int m_score = 0;

	// Use this for initialization
	void Start () {
		m_scoreDisplay = GameObject.FindWithTag("Score").GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int AddScore(float radius, int level) {
		int score = Mathf.RoundToInt(radius * m_scoreFactor * level);

		m_score += score;
		UpdateScore();

		return score;
	}

	private void UpdateScore() {
		m_scoreDisplay.text = "Score : " + m_score;
	}
}
