using UnityEngine;

public class WaveManager : MonoBehaviour {

#region Dependencies
	// [SerializeField] private GameObject[] Entries;
#endregion

#region Attributes
	public static WaveManager Instance { get; private set; }
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
	    Instance = this;
		_transform = transform;
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
	public void Init() {
		_waveCount = 0;
		_spawnTimer = 0f;
		
		UiManager.Instance.SetWaveCount(_waveCount);
	}

	public void IncreaseWave() {
		_waveCount++;
		UiManager.Instance.SetWaveCount(_waveCount);
	}
#endregion

}

