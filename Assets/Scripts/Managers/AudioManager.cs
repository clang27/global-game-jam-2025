using DG.Tweening;
using UnityEngine;

namespace Managers {
	public class AudioManager : MonoBehaviour, IManager {

	#region Dependencies
		[Header("Music")] 
		[SerializeField] private AudioClip songOne;
		[SerializeField] private AudioClip songTwo;
		[SerializeField] private AudioClip songThree;
	#endregion

	#region Attributes
		public static AudioManager Instance { get; private set; }
		public AudioSource SfxSource => _sfxSource;
		public bool PlayingLowAirTheme { get; private set; }
	#endregion

		#region Components
		private AudioSource _musicSource, _sfxSource;
		#endregion

		#region Data
		// private Coroutine _marchOverCoroutine;
		#endregion

		#region Unity
		private void Awake() {
			Instance = this;
		
			_musicSource = transform.GetChild(0).GetComponent<AudioSource>();
			_sfxSource = transform.GetChild(1).GetComponent<AudioSource>();
		}
		#endregion

		#region Custom

		public void Init() {
			PlayGameTheme();
			AdjustMusicVolume(SettingsAndPauseManager.MasterVolume * SettingsAndPauseManager.MusicVolume);
			AdjustSfxVolume(SettingsAndPauseManager.MasterVolume * SettingsAndPauseManager.SfxVolume);
		}
		
		public void SceneChange(string sceneName) {}

		public void AdjustMusicVolume(float f) {
			_musicSource.volume = f;
		}
		
		public void AdjustSfxVolume(float f) {
			_sfxSource.volume = f;
		}
		
		public void PlayGameTheme() {
			PlayingLowAirTheme = false;
			PlaySong(songOne);
		}

		public void PlayLowAirTheme() {
			PlayingLowAirTheme = true;
			PlaySong(songThree);
		}
	
		private void PlaySong(AudioClip ac) {
			_musicSource.clip = ac;
			_musicSource.Play();
		}

		public void StopSong() {
			_musicSource.Stop();
		}

		public void PlaySfx(AudioClip ac) {
			if (ac == null) {
				return;
			}
			
			_sfxSource.pitch = 1f;
			_sfxSource.PlayOneShot(ac);
		}
		#endregion

	}
}

