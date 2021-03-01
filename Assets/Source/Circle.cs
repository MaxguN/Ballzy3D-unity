using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Circle : MonoBehaviour {
	public float m_radius = 50;
	public float m_thickness = 5;
	public int m_triangle_number = 64;
	public Material m_material;
	public Filling m_fillingPrefab;
	public float m_fillingSpeed = 2f;

	public Material[] m_levels = new Material[6];

	public float m_start = Mathf.PI / 2;
	public float m_end = 5 * Mathf.PI / 2;

	private Vector3[] m_vertices;
	private Vector2[] m_uvs;
	private int[] m_triangles;

	private ArrayList m_balls = new ArrayList();
	private int m_level = 0;
	private Filling m_filling;
	private bool m_isFilling = false;
	private bool m_isFilled = false;
	private int m_score = 0;

	void Start() {
		if (!GetComponent<MeshFilter>()) {
			gameObject.AddComponent<MeshFilter>();
		}
		if (!GetComponent<MeshRenderer>()) {
			gameObject.AddComponent<MeshRenderer>();
		}

		GenerateCircleMesh(m_start, m_end);

		GetComponent<MeshRenderer>().materials = new Material[] { m_material };
		//GetComponent<Alive>().SetRadius(1);
	}

	// Update is called once per frame
	void Update () {
		if (m_balls.Count != 0 && m_isFilling) {
			float fill = Time.deltaTime * m_fillingSpeed / m_radius * m_balls.Count;
			
			m_end -= fill;
			m_filling.m_start -= fill;

			if (m_end <= m_start) {
				m_isFilling = false;
				m_end = m_start;
				m_filling.m_start = m_filling.m_end - 2 * Mathf.PI;
				m_isFilled = true;

				int score = GameObject.FindWithTag("Player").GetComponent<GameInstance>().AddScore(m_radius, m_level);
				GetComponent<CircleScore>().DisplayScore(score, m_radius);

				GenerateBall();
				m_filling.Complete();
			}

			GenerateCircleMesh(m_start, m_end);
			m_filling.GenerateCircleMesh(m_filling.m_start, m_filling.m_end);
		} else if (Application.isEditor) {
			GenerateCircleMesh(m_start, m_end);
		}
	}

	public Material GetCurrentMaterial() {
		if (m_level == 0) {
			Debug.Log("Not filling");
			return m_material;
		} else {
			Debug.Log("Filling");
			return m_levels[m_level - 1];
		}
	}

	protected void GenerateCircleMesh(float angleStart, float angleEnd) {
		Mesh mesh = GetComponent<MeshFilter>().sharedMesh; // TODO check use of mesh vs sharedMesh
		mesh.Clear();

		if (angleEnd == angleStart) {
			return;
		} else if (angleEnd < angleStart) {
			angleStart += angleEnd;
			angleEnd = angleStart - angleEnd;
			angleStart -= angleEnd;
		}

		if (angleEnd - angleStart > 2 * Mathf.PI) {
			angleEnd = angleStart + (2 * Mathf.PI);
		}

		float angle_delta = 2 * Mathf.PI / (m_triangle_number / 2);

		int verticesLength = 2 * Mathf.CeilToInt((angleEnd - angleStart) / angle_delta);
		int trianglesLength = verticesLength * 3;

		if (angleEnd - angleStart < 2 * Mathf.PI) {
			verticesLength += 2;
		}

		m_vertices = new Vector3[verticesLength];
		m_uvs = new Vector2[verticesLength];
		m_triangles = new int[trianglesLength];

		for (int i = 0; i < verticesLength; i += 2) {
			float radius_ext = m_radius + m_thickness / 2;
			float radius_int = m_radius - m_thickness / 2;

			float angle = angle_delta * i / 2 + angleStart;

			if (angle > angleEnd) {
				angle = angleEnd;
			}

			float ext_x = Mathf.Cos(angle) * radius_ext;
			float ext_z = Mathf.Sin(angle) * radius_ext;
			float int_x = Mathf.Cos(angle) * radius_int;
			float int_z = Mathf.Sin(angle) * radius_int;

			m_vertices[i] = new Vector3(ext_x, 0, ext_z);
			m_vertices[i + 1] = new Vector3(int_x, 0, int_z);

			m_uvs[i] = new Vector2(0, 0);
			m_uvs[i + 1] = new Vector2(0, 0);
		}

		for (int i = 0; i * 3 < trianglesLength; i += 1) {
			int a = (i % 2 == 0) ? i : i - 1;
			int b = ((i + 1) % 2 == 0) ? (i + 2) % verticesLength : (i + 1) % verticesLength;
			int c = (i % 2 == 0) ? (b + 2) % verticesLength : (b - 1) % verticesLength;

			m_triangles[3 * i] = a;
			m_triangles[3 * i + 1] = b;
			m_triangles[3 * i + 2] = c;
		}

		mesh.vertices = m_vertices;
		mesh.uv = m_uvs;
		mesh.triangles = m_triangles;

		SphereCollider sphereCollider = GetComponent<SphereCollider>();

		if (sphereCollider) {
			sphereCollider.radius = m_radius + m_thickness / 2;
		}
	}

	void GenerateBall() {
		if (m_balls.Count >= 3 && m_level < m_levels.Length) {
			foreach (Ball ball in m_balls) {
				ball.GetComponent<Alive>().Kill(false);
			}

			GameObject.FindWithTag("Player").GetComponent<BallGenerator>().NewBall(Mathf.Min(m_level + 1, m_levels.Length), transform.position);
		} else {
			GameObject.FindWithTag("Player").GetComponent<BallGenerator>().NewBall(Mathf.Max(1, m_level - 1), transform.position);
		}
	}

	void OnTriggerEnter(Collider other) {
		Ball ball = other.GetComponent<Ball>();

		if (ball) {
			if (m_level == 0) {
				m_level = ball.level;
				m_filling = Instantiate(m_fillingPrefab);
				m_filling.m_material = m_levels[m_level - 1];
				m_filling.m_radius = m_radius;
				m_filling.m_thickness = m_thickness;
				m_filling.m_triangle_number = m_triangle_number;

				m_filling.transform.SetParent(transform);
				m_filling.transform.localPosition = Vector3.zero;

				m_isFilling = true;

				//GetComponent<Alive>().SetColor(m_filling.m_material);
			}

			m_balls.Add(ball);
		}
	}

	void OnTriggerExit(Collider other) {
		Ball ball = other.GetComponent<Ball>();

		if (ball) {
			m_balls.Remove(ball);
		}
	}
}
