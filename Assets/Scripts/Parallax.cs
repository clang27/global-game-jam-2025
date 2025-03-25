using UnityEngine;

public class Parallax : MonoBehaviour {

	[SerializeField][Range(0f, 10f)] private float parallaxEffectX;
	[SerializeField][Range(0f, 10f)] private float parallaxEffectY;
	
	private float _height, _length;
	private Vector2 _startPos;
	private GameObject _cam;

	private Transform _transform;

	private void Start() {
		_transform = transform;
		_startPos = _transform.position;
		_length = GetComponent<SpriteRenderer>().bounds.size.x;
		_height = GetComponent<SpriteRenderer>().bounds.size.y;
		_cam = GameObject.FindGameObjectWithTag("MainCamera");
	}

	private void Update() {
		var xDist = _cam.transform.position.x * parallaxEffectX;
		var yDist = _cam.transform.position.y * parallaxEffectY;

		_transform.position = new Vector3(_startPos.x + xDist, _startPos.y + yDist, _transform.position.z);
	}

}

