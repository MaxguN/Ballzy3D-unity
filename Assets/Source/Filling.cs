using UnityEngine;
using System.Collections;

public class Filling : Circle {
	public float m_effectDuration = 0.5f;

	private float m_effectTimer = 0;

	// Use this for initialization
	void Start () {
		m_end = m_start = Mathf.PI / 2;

		if (!GetComponent<MeshFilter>()) {
			gameObject.AddComponent<MeshFilter>();
		}
		if (!GetComponent<MeshRenderer>()) {
			gameObject.AddComponent<MeshRenderer>();
		}

		GetComponent<MeshRenderer>().materials = new Material[] { m_material };

		Material materialAlpha = Instantiate(m_material);
		Color colorAlpha = materialAlpha.GetColor("_Color");
		colorAlpha.a = 0.5f;
		materialAlpha.SetFloat("_Mode", 2);
		materialAlpha.SetColor("_Color", colorAlpha);
		materialAlpha.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		materialAlpha.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		materialAlpha.SetInt("_ZWrite", 0);
		materialAlpha.DisableKeyword("_ALPHATEST_ON");
		materialAlpha.EnableKeyword("_ALPHABLEND_ON");
		materialAlpha.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		materialAlpha.renderQueue = 3000;

		transform.GetChild(0).GetComponent<MeshRenderer>().materials = new Material[] { materialAlpha };
		transform.GetChild(1).GetComponent<MeshRenderer>().materials = new Material[] { materialAlpha };
	}
	
	// Update is called once per frame
	void Update () {
		if (m_effectTimer > 0) {
			Material material = transform.GetChild(1).GetComponent<MeshRenderer>().materials[0];
			Color color = material.GetColor("_Color");
			color.a = m_effectTimer / (2 * m_effectDuration);
			material.SetColor("_Color", color);

			GenerateDiskMesh(m_start, m_radius * (((1 - (m_effectTimer / m_effectDuration)) * 4) + 1) , transform.GetChild(1).GetComponent<MeshFilter>().mesh);
			m_effectTimer -= Time.deltaTime;

			if (m_effectTimer <= 0) {
				Destroy(transform.GetChild(1).gameObject);
				Debug.Log("Filling over");
			}
		}
	}

	protected void GenerateDiskMesh(float angleStart, float radius, Mesh mesh) {
		mesh.Clear();

		float angle_delta = 2 * Mathf.PI / (m_triangle_number / 2);

		int verticesLength = (m_triangle_number / 2) + 1;
		int trianglesLength = (m_triangle_number / 2) * 3;

		Vector3[] vertices = new Vector3[verticesLength];
		Vector2[] uvs = new Vector2[verticesLength];
		int[] triangles = new int[trianglesLength];

		vertices[0] = Vector3.zero;
		for (int i = 1; i < verticesLength; i += 1) {
			float radius_int = radius - m_thickness / 2;

			float angle = angle_delta * i + angleStart;
			
			float int_x = Mathf.Cos(angle) * radius_int;
			float int_z = Mathf.Sin(angle) * radius_int;

			vertices[i] = new Vector3(int_x, 0, int_z);
			uvs[i] = new Vector2(0, 0);
		}

		for (int i = 0; i * 3 < trianglesLength; i += 1) {
			int a = 0;
			int b = ((i + 2) % verticesLength) + Mathf.FloorToInt((i + 2) / verticesLength);
			int c = (i + 1) % verticesLength;

			triangles[3 * i] = a;
			triangles[3 * i + 1] = b;
			triangles[3 * i + 2] = c;
		}

		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
	}

	public void Complete() {
		Debug.Log("Filling complete");
		GenerateDiskMesh(m_start, m_radius, transform.GetChild(0).GetComponent<MeshFilter>().mesh);

		m_effectTimer = m_effectDuration;
	}
}
