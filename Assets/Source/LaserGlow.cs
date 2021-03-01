using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LaserGlow : MonoBehaviour {
	public float m_duration = 500;
	public float m_width = 20;
	public float m_height = 0.3f;
	public Material m_material;
	public Color[] m_colors = new Color[3];
	public Color[] m_altColors = new Color[3];

	private float m_timer = 0;

	private Vector3[] m_vertices;
	private Vector2[] m_uvs;
	private int[] m_triangles;

	private Mesh m_mesh;

	// Use this for initialization
	void Start () {
		if (!GetComponent<MeshFilter>()) {
			gameObject.AddComponent<MeshFilter>();
		}
		if (!GetComponent<MeshRenderer>()) {
			gameObject.AddComponent<MeshRenderer>();
		}

		m_mesh = GetComponent<MeshFilter>().sharedMesh;
		GenerateLaserMesh();

		GetComponent<MeshRenderer>().materials = new Material[] { m_material };
	}

	// Update is called once per frame
	void Update () {
		if (Application.isEditor) {
			GenerateLaserMesh();
		}

		Color[] colors = new Color[m_mesh.vertexCount];

		m_timer += Time.deltaTime * 1000;

		while (m_timer > 2 * m_duration) {
			m_timer -= 2 * m_duration;
		}

		float factor = Mathf.Min(m_timer, 2 * m_duration - m_timer) / m_duration;

		for (int i = 0; i * 2 < colors.Length; i += 1) {
			colors[i * 2] = Color.Lerp(m_colors[i], m_altColors[i], factor);
			colors[i * 2 + 1] = Color.Lerp(m_colors[i], m_altColors[i], factor);
		}

		m_mesh.colors = colors;
	}

	protected void GenerateLaserMesh() {
		if (m_mesh) {
			m_mesh.Clear();

			int verticesLength = 6;
			int trianglesLength = 4 * 3;

			m_vertices = new Vector3[verticesLength];
			m_uvs = new Vector2[verticesLength];
			m_triangles = new int[trianglesLength];

			float left = -(m_width / 2);
			float right = -left;
			float top = m_height / 2;
			float bottom = -top;

			m_vertices[0] = new Vector3(left, 0, top);
			m_vertices[1] = new Vector3(right, 0, top);
			m_vertices[2] = new Vector3(left, 0, 0);
			m_vertices[3] = new Vector3(right, 0, 0);
			m_vertices[4] = new Vector3(left, 0, bottom);
			m_vertices[5] = new Vector3(right, 0, bottom);

			for (int i = 0; i < verticesLength; i += 1) {
				m_uvs[i] = new Vector2(0, 0);
			}

			m_triangles[0] = 0;
			m_triangles[1] = 1;
			m_triangles[2] = 2;
			m_triangles[3] = 2;
			m_triangles[4] = 1;
			m_triangles[5] = 3;
			m_triangles[6] = 2;
			m_triangles[7] = 3;
			m_triangles[8] = 4;
			m_triangles[9] = 4;
			m_triangles[10] = 3;
			m_triangles[11] = 5;

			m_mesh.vertices = m_vertices;
			m_mesh.uv = m_uvs;
			m_mesh.triangles = m_triangles;

			BoxCollider boxCollider = GetComponent<BoxCollider>();

			if (boxCollider) {
				boxCollider.size = new Vector3(m_width, 1, m_height);
			}
		}
	}
}
