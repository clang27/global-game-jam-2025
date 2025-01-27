using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private List<GameObject> islandPrefabs;
	[SerializeField] private float distanceUntilDespawn;
	[SerializeField] private int maxIslands;
	[SerializeField] private float islandSpacing = 5f;
#endregion

#region Attributes
	public static IslandManager Instance { get; private set; }
#endregion

#region Components
	private Transform _transform;
#endregion

#region Data
	private readonly List<GameObject> _generatedIslands = new();
#endregion

#region Unity
    private void Awake() {
	    Instance = this;
		_transform = transform;
    }
	
	private void FixedUpdate() {
		var player = GameManager.Instance.Player;

		for (var index = 0; index < _generatedIslands.Count; index++) {
			var go = _generatedIslands[index];
			
			if (player.transform.position.y - go.transform.position.y > distanceUntilDespawn) {
				Destroy(go);
				_generatedIslands.RemoveAt(index);
			}
		}

		if (_generatedIslands.Count < maxIslands) {
			var distanceUp = Random.Range(30f, 100f);
			var distanceLeft = Random.Range(10f, 150f) * (Random.Range(0, 2) == 0 ? -1 : 1);
			var goalPosition = player.transform.position + new Vector3(distanceLeft, distanceUp);
			var tooClose = _generatedIslands
				.Select(island2 => Vector2.Distance(goalPosition, island2.transform.position))
				.Any(dist => dist < islandSpacing);

			if (!tooClose) {
				var island = Instantiate(islandPrefabs[Random.Range(0, islandPrefabs.Count)]);
				island.transform.position = player.transform.position;
				island.transform.position += new Vector3(distanceLeft, distanceUp);
				_generatedIslands.Add(island);	
			}
		}
	}
#endregion

#region Custom
	public void Init() {
		foreach (var go in _generatedIslands) {
			Destroy(go);
		}
		
		_generatedIslands.Clear();
	}
#endregion

}

