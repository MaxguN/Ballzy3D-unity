using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Wall : MonoBehaviour {
	public Material m_material;

	public float m_thickness = 0.1f;
	public float m_width = 5f;
	public float m_height = 1f;

	private Vector3[] m_vertices;
	private Vector2[] m_uvs;
	private int[] m_triangles;
	
	void Start() {
		if (!GetComponent<MeshFilter>()) {
			gameObject.AddComponent<MeshFilter>();
		}
		if (!GetComponent<MeshRenderer>()) {
			gameObject.AddComponent<MeshRenderer>();
		}

		GenerateRectangleMesh();

		GetComponent<MeshRenderer>().materials = new Material[] { m_material };
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.isEditor) {
			GenerateRectangleMesh();
		}
	}

	protected void GenerateRectangleMesh() {
		Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
		mesh.Clear();

		int verticesLength = 12;
		int trianglesLength = 8 * 3;

		m_vertices = new Vector3[verticesLength];
		m_uvs = new Vector2[verticesLength];
		m_triangles = new int[trianglesLength];

		float left_in = -((m_width - m_thickness) / 2);
		float left_out = -(m_width / 2);
		float right_in = (m_width - m_thickness) / 2;
		float right_out = m_width / 2;
		float top_in = (m_height - m_thickness) / 2;
		float top_out = m_height / 2;
		float bottom_in = -((m_height - m_thickness) / 2);
		float bottom_out = -(m_height / 2);

		m_vertices[0] = new Vector3(left_in, 0, top_in);
		m_vertices[1] = new Vector3(left_out, 0, top_out);
		m_vertices[2] = new Vector3(right_in, 0, top_out);
		m_vertices[3] = new Vector3(right_in, 0, top_in);
		m_vertices[4] = new Vector3(right_out, 0, top_out);
		m_vertices[5] = new Vector3(right_out, 0, bottom_in);
		m_vertices[6] = new Vector3(right_in, 0, bottom_in);
		m_vertices[7] = new Vector3(right_out, 0, bottom_out);
		m_vertices[8] = new Vector3(left_in, 0, bottom_out);
		m_vertices[9] = new Vector3(left_in, 0, bottom_in);
		m_vertices[10] = new Vector3(left_out, 0, bottom_out);
		m_vertices[11] = new Vector3(left_out, 0, top_in);

		for (int i = 0; i < verticesLength; i += 1) {
			m_uvs[i] = new Vector2(0, 0);
		}

		m_triangles[0] = 11;
		m_triangles[1] = 1;
		m_triangles[2] = 2;
		m_triangles[3] = 2;
		m_triangles[4] = 3;
		m_triangles[5] = 0;
		m_triangles[6] = 2;
		m_triangles[7] = 4;
		m_triangles[8] = 5;
		m_triangles[9] = 5;
		m_triangles[10] = 6;
		m_triangles[11] = 3;
		m_triangles[12] = 5;
		m_triangles[13] = 7;
		m_triangles[14] = 8;
		m_triangles[15] = 8;
		m_triangles[16] = 9;
		m_triangles[17] = 6;
		m_triangles[18] = 8;
		m_triangles[19] = 10;
		m_triangles[20] = 11;
		m_triangles[21] = 11;
		m_triangles[22] = 0;
		m_triangles[23] = 9;

		mesh.vertices = m_vertices;
		mesh.uv = m_uvs;
		mesh.triangles = m_triangles;

		BoxCollider boxCollider = GetComponent<BoxCollider>();

		if (boxCollider) {
			boxCollider.size = new Vector3(m_width, 1, m_height);
		}
	}
}
