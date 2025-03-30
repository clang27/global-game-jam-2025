using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KitchenMagnet : MonoBehaviour {

#region Dependencies
	[SerializeField] private float waitTime = 2f;
	[SerializeField] private float fallTime = 1f;
#endregion	
	
#region Data
	private Coroutine _marchOverCoroutine;
	private List<FallingObject> _knivesAndPans;
	private float _timer;
	private int _pointer;
#endregion

#region Unity
    private void Awake() {
		_knivesAndPans = GetComponentsInChildren<FallingObject>().ToList();
		_pointer = _knivesAndPans.Count - 1; // Object farthest to right
    }
	
	private void FixedUpdate() {
		_timer += Time.fixedDeltaTime;

		if (_timer >= waitTime) {
			_timer = 0f;
			
			if (_pointer == -1) {
				foreach (var k in _knivesAndPans) {
					k.ResetPosition();
				}
				_pointer = _knivesAndPans.Count - 1;

				_timer = waitTime / 2f; // Start with some wait time
			} else {
				_knivesAndPans[_pointer].Launch(fallTime);
				_pointer--;
			}

		}
	}
#endregion

}

