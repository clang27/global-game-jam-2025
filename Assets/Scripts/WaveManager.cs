using UnityEngine;

public class WaveManager : MonoBehaviour {

#region Dependencies
	// [SerializeField] private GameObject[] Entries;
#endregion

#region Attributes
	// public static GameManager Instance { get; private set; }
#endregion

#region Components
	private Transform _transform;
#endregion

#region Data
	private int _waveCount = 0;
	private readonly float _spawnInterval = 5f;
	private float _spawnTimer = 0f;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
    }

    private void Start() {
	    UiManager.Instance.SetWaveCount(_waveCount);
    }

    private void Update() {
	    _spawnTimer += Time.deltaTime;

	    if (_spawnTimer > _spawnInterval) {
		    EnemyManager.Instance.SpawnEnemy();
		    _spawnTimer = 0f;
	    }
    }
#endregion

#region Custom

	public void IncreaseWave() {
		_waveCount++;
		UiManager.Instance.SetWaveCount(_waveCount);
	}
#endregion

}

