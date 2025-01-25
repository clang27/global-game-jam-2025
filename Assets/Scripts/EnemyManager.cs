using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

#region Dependencies	
	[SerializeField] private List<GameObject> enemyPrefabs;
	[SerializeField] private int poolSize;
	private BubbleBehavior _bubble;
#endregion

#region Attributes
	public static EnemyManager Instance { get; private set; }
#endregion

#region Components
	private Transform _transform;
	private Transform _poolTransform;
#endregion

#region Data
	private List<AiController> _pooledEnemies = new();
	private List<AiController> _attackingEnemies = new();

#endregion

#region Unity
    private void Awake() {
	    Instance = this;
		_transform = transform;
		_poolTransform = _transform.GetChild(0);
		_bubble = FindFirstObjectByType<BubbleBehavior>();
    }

    private void Start() {
	    foreach (var prefab in enemyPrefabs) {
		    for (var i = 0; i < poolSize; i++) {
			    var enemy = Instantiate(prefab);
			    enemy.name = prefab.name + i;
			    MoveOutOfPlay(enemy.GetComponent<AiController>());
			    _pooledEnemies.Add(enemy.GetComponent<AiController>());
		    }
	    }
    }
#endregion

#region Custom

	public void SpawnEnemy() {
		MoveInPlay(_pooledEnemies[0]);
		_attackingEnemies.Add(_pooledEnemies[0]);
		_pooledEnemies.RemoveAt(0);
	}
	
	public void DespawnEnemy(AiController ai) {
		MoveOutOfPlay(ai);
		_pooledEnemies.Add(ai);
		_attackingEnemies.Remove(ai);
	}
	private void MoveOutOfPlay(AiController t) {
		t.transform.position = new Vector3(0f, -5000f);
		t.transform.SetParent(_poolTransform);
	}
	private void MoveInPlay(AiController t) {
		var flyInTime = 3f;
		
		t.transform.SetParent(null);
		
		var randomRadian = Random.Range(0f, 2 * Mathf.PI);
		var startPoint = _bubble.transform.position + new Vector3(Mathf.Cos(randomRadian) * 20f, Mathf.Sin(randomRadian) * 20f, 0f);
		startPoint += (Vector3) _bubble.Velocity * flyInTime;
		var aimPoint = _bubble.transform.position + new Vector3(Mathf.Cos(randomRadian) * _bubble.Radius * 0.8f, Mathf.Sin(randomRadian) * _bubble.Radius * 0.8f, 0f);
		aimPoint += (Vector3) _bubble.Velocity * flyInTime;
		
		t.transform.position = startPoint;

		t.FlyingIn = true;
		t.transform.DOMove(aimPoint, flyInTime)
			.SetEase(Ease.Linear)
			.OnComplete(() => t.FlyingIn = false);
	}
#endregion

}

