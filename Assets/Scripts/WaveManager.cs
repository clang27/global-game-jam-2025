using UnityEngine;

public class WaveManager : MonoBehaviour {
	
#region Properties
	[SerializeField] private int maxWaves = 3;
#endregion

#region Attributes
	public static WaveManager Instance { get; private set; }
	public bool CompletedWaves => _waveCount == maxWaves;
#endregion

#region Data
	private int _waveCount = 0;
#endregion

#region Unity
    private void Awake() {
	    Instance = this;
    }
#endregion

#region Custom
	public void Init() {
		_waveCount = 0;
		
		UiManager.Instance.SetWaveCount(_waveCount);
	}

	public void StartWave() {
		_waveCount++;
		UiManager.Instance.SetWaveCount(_waveCount);

		switch (_waveCount) {
			case 1:
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Swordfish);
				EnemyManager.Instance.SpawnEnemy(100f, AiStyle.Swordfish);
				EnemyManager.Instance.SpawnEnemy(150f, AiStyle.Swordfish);
				EnemyManager.Instance.SpawnEnemy(200f, AiStyle.Swordfish);
				break;
			case 2:
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Swordfish);
				EnemyManager.Instance.SpawnEnemy(100f, AiStyle.Swordfish);
				EnemyManager.Instance.SpawnEnemy(150f, AiStyle.Swordfish);
				EnemyManager.Instance.SpawnEnemy(200f, AiStyle.Swordfish);
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Swordfish);
				EnemyManager.Instance.SpawnEnemy(100f, AiStyle.Swordfish);
				EnemyManager.Instance.SpawnEnemy(150f, AiStyle.Swordfish);
				EnemyManager.Instance.SpawnEnemy(200f, AiStyle.Swordfish);
				break;
			case 3:
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Swordfish);
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Swordfish);
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Swordfish);
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Swordfish);
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
				break;
			case 9:
				break;
			case 10:
				break;
		}
	}
#endregion

}

