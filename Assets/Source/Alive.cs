using UnityEngine;
using System.Collections;

public class Alive : MonoBehaviour {
	public ParticleSystem m_explosion;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	void SetColor(ParticleSystem ps, Material material) {
		ps.startColor = material.GetColor("_Color");
	}

	void SetRadius(ParticleSystem ps, float radius) {
		var shape = ps.shape;
		shape.shapeType = ParticleSystemShapeType.CircleEdge;
		shape.radius = radius;
	}

	void SetRectangle(ParticleSystem ps, float width, float height) {
		var shape = ps.shape;
		shape.shapeType = ParticleSystemShapeType.SingleSidedEdge;
		shape.radius = width / 2;
		ps.transform.localPosition += (height / 2) * Vector3.back;

		ParticleSystem top = Instantiate(ps);
		ps.transform.localPosition += height * Vector3.forward;
		ps.transform.localEulerAngles = -ps.transform.localEulerAngles;

		ParticleSystem left = Instantiate(ps);
		shape.radius = height;
		ps.transform.localPosition += (height / 2) * Vector3.back + (width / 2) * Vector3.left;
		ps.transform.localEulerAngles += 90 * Vector3.down;

		ParticleSystem right = Instantiate(ps);
		ps.transform.localPosition += width * Vector3.right;
		ps.transform.localEulerAngles += 180 * Vector3.up;
	}

	public void Kill(bool loseCondition = true) {
		Debug.Log("Killing" + gameObject.name);

		ParticleSystem explosion = Instantiate(m_explosion);
		explosion.transform.position = gameObject.transform.position;

		if (GetComponent<Ball>()) {
			SetColor(explosion, GetComponent<MeshRenderer>().materials[0]);
			SetRadius(explosion, 1);

			GetComponent<Ball>().Kill(loseCondition);
		} else if (GetComponent<Circle>()) {
			SetColor(explosion, GetComponent<Circle>().GetCurrentMaterial());
			SetRadius(explosion, GetComponent<Circle>().m_radius);

			Destroy(gameObject);
		} else if (GetComponent<Wall>()) {
			SetColor(explosion, GetComponent<MeshRenderer>().materials[0]);
			SetRectangle(explosion, GetComponent<Wall>().m_width, GetComponent<Wall>().m_height);

			Destroy(gameObject);
		} else {
			Destroy(gameObject);
		}
	}
}
