using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour {

#region Dependencies
	[Header("Music")] 
	[SerializeField] private AudioClip songOne;
	[SerializeField] private AudioClip songTwo;
	[Header("SFX")] 
	[SerializeField] private AudioClip beep;
	[SerializeField] private AudioClip great, good, miss;
#endregion

#region Attributes
	public static AudioManager Instance { get; private set; }
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
	public void PlayGameTheme() {
		if (_musicSource.isPlaying) {
			_musicSource.DOFade(0f, 0.1f).OnComplete(() => {
				_musicSource.Stop();
				_musicSource.clip = songOne;
				_musicSource.Play();
				_musicSource.DOFade(1f, 0.1f);
			});
		} else {
			PlaySong(songOne);
		}
	}
	
	public void PlayShopTheme() {
		if (_musicSource.isPlaying) {
			_musicSource.DOFade(0f, 0.1f).OnComplete(() => {
				_musicSource.Stop();
				_musicSource.clip = songTwo;
				_musicSource.Play();
				_musicSource.DOFade(1f, 0.1f);
			});
		} else {
			PlaySong(songTwo);
		}
	}
	
	private void PlaySong(AudioClip ac, float pitch = 1f, float vol = 1f) {
		_musicSource.clip = ac;
		_musicSource.pitch = pitch;
		_musicSource.volume = vol;
		_musicSource.Play();
	}

	public void PlaySfx(AudioClip ac, float vol = 1f) {
		if (ac == null) {
			return;
		}

		var pitch = 1f;
		_sfxSource.pitch = pitch;
		_sfxSource.volume = vol;
		_sfxSource.PlayOneShot(ac);
	}
#endregion

}

