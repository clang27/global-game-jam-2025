using UnityEngine;

public class Parallax : MonoBehaviour {

	private float length, startpos;
	private GameObject cam;
	public float parallaxEffect;
	private Transform _transform;

	private void Start() {
		_transform = transform;
		startpos = _transform.position.y;
		length = GetComponent<SpriteRenderer>().bounds.size.y;
		cam = GameObject.FindGameObjectWithTag("MainCamera");
	}

	private void Update() {
		var temp = (cam.transform.position.y * (1 - parallaxEffect));
		var dist = (cam.transform.position.y * parallaxEffect);

		_transform.position = new Vector3(_transform.position.x, startpos + dist, _transform.position.z);

		if (temp > startpos + length) startpos += length;
		else if (temp < startpos - length) startpos -= length;
	}

}

