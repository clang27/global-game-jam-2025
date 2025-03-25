using Enemies;
using Managers;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager {

#region Dependencies
	// [SerializeField] private GameObject[] Entries;
#endregion

#region Attributes
	public static EnemyManager Instance { get; private set; }
#endregion

#region Components
	private ParticleSystem _poofParticleSystemOne, _poofParticleSystemTwo, _poofParticleSystemThree;
#endregion

#region Data
	private int _systemCounter;
#endregion

#region Unity
    private void Awake() {
	    Instance = this;
	    _poofParticleSystemOne = transform.GetChild(0).GetComponent<ParticleSystem>();
	    _poofParticleSystemTwo = transform.GetChild(1).GetComponent<ParticleSystem>();
	    _poofParticleSystemThree = transform.GetChild(2).GetComponent<ParticleSystem>();
    }
#endregion

#region Custom
	public void Init() {}
	public void SceneChange(string sceneName) {
		foreach (var e in FindObjectsByType<EnemyBehavior>(FindObjectsSortMode.None)) {
			e.Init();
		}
	}
	public void Poof(Vector2 location) {
		var system = (_systemCounter++ % 3) switch {
			1 => _poofParticleSystemTwo,
			2 => _poofParticleSystemThree,
			_ => _poofParticleSystemOne
		};

		system.transform.position = location;
		system.Play();
	}
#endregion

}

