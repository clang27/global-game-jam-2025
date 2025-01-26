using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

#region Dependencies	
	[SerializeField] private List<GameObject> enemyPrefabs;
	[SerializeField] private int poolSize;
#endregion

#region Attributes
	public static EnemyManager Instance { get; private set; }
#endregion

#region Components
	private Transform _transform;
	private Transform _poolTransform;
#endregion

#region Data
	private readonly List<AiController> _pooledEnemies = new();
	private readonly List<AiController> _attackingEnemies = new();

#endregion

#region Unity
    private void Awake() {
	    Instance = this;
		_transform = transform;
		_poolTransform = _transform.GetChild(0);
    }
#endregion

#region Custom

	public void Init() {
		foreach (var enemy in _pooledEnemies) {
			Destroy(enemy.gameObject);
		}
		
		foreach (var enemy in _attackingEnemies) {
			Destroy(enemy.gameObject);
		}
		
		_pooledEnemies.Clear();
		_attackingEnemies.Clear();
		
		foreach (var prefab in enemyPrefabs) {
			for (var i = 0; i < poolSize; i++) {
				var enemy = Instantiate(prefab);
				enemy.name = prefab.name + i;
				enemy.GetComponent<AiController>().enabled = false;
				MoveOutOfPlay(enemy.GetComponent<AiController>());
				_pooledEnemies.Add(enemy.GetComponent<AiController>());
			}
		}
	}

	public void SpawnEnemy(float distanceFromBubble, AiStyle aiStyle) {
		var ai = _pooledEnemies.First(enemy => enemy.Type == aiStyle);
		
		MoveInPlay(ai, distanceFromBubble);
		ai.enabled = true;
		_attackingEnemies.Add(ai);
		_pooledEnemies.Remove(ai);
		UiManager.Instance.SetEnemiesRemaining(_attackingEnemies.Count);
	}
	
	public void DespawnEnemy(AiController ai) {
		MoveOutOfPlay(ai);
		
		ai.enabled = false;
		_pooledEnemies.Add(ai);
		_attackingEnemies.Remove(ai);
		UiManager.Instance.SetEnemiesRemaining(_attackingEnemies.Count);
		if (_attackingEnemies.Count == 0) {
			GameManager.Instance.WaveDone();
		}
	}
	private void MoveOutOfPlay(AiController t) {
		t.transform.position = new Vector3(0f, -5000f);
		t.transform.SetParent(_poolTransform);
	}
	private void MoveInPlay(AiController t, float distanceFromBubble) {
		t.transform.SetParent(null);

		var bubble = GameManager.Instance.Bubble;
		var randomRadian = Random.Range(0f, Mathf.PI);
		var startPoint = bubble.transform.position + new Vector3(Mathf.Cos(randomRadian) * distanceFromBubble, Mathf.Sin(randomRadian) * distanceFromBubble, 0f);
		
		t.transform.position = startPoint;
		t.FlyingIn = true;
		t.BoostStats();
	}
#endregion

}

