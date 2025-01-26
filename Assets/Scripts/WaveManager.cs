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
		
		UiManager.Instance.SetWaveCount(_waveCount, maxWaves);
	}

	public void StartWave() {
		_waveCount++;
		UiManager.Instance.SetWaveCount(_waveCount, maxWaves);

		switch (_waveCount) {
			case 1:
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(75f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(100f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(125f, AiStyle.Jellyfish);
				break;
			case 2:
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(75f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(100f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(125f, AiStyle.Jellyfish);
				break;
			case 3:
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(75f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(100f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(125f, AiStyle.Jellyfish);
				break;
			case 4:
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(75f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(100f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(125f, AiStyle.Jellyfish);
				break;
			case 5:
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(75f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(100f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(125f, AiStyle.Jellyfish);
				break;
			case 6:
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(75f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(100f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(125f, AiStyle.Jellyfish);
				break;
			case 7:
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(75f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(100f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(125f, AiStyle.Jellyfish);
				break;
			case 8:
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(75f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(100f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(125f, AiStyle.Jellyfish);
				break;
			case 9:
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(75f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(100f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(125f, AiStyle.Jellyfish);
				break;
			case 10:
				EnemyManager.Instance.SpawnEnemy(50f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(75f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(100f, AiStyle.Jellyfish);
				EnemyManager.Instance.SpawnEnemy(125f, AiStyle.Jellyfish);
				break;
		}
	}
#endregion

}

