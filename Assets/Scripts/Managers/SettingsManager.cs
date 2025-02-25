using Managers;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private Slider masterVolumeSlider;
	[SerializeField] private float sliderQuotient = 10f;
#endregion

#region Attributes
	public static float MasterVolume { get; private set; }
	public static float MusicVolume { get; private set; }
	public static float SfxVolume { get; private set; }
#endregion

#region Unity
	private void Awake() {
		masterVolumeSlider.onValueChanged.AddListener(AdjustMasterVolume);
	}

	private void Start() {
		Load();
	}

#endregion

#region Custom
	private void AdjustMasterVolume(float f) {
		MasterVolume = f / sliderQuotient;
		
		AudioManager.Instance.AdjustMusicVolume(MasterVolume * MusicVolume);
		AudioManager.Instance.AdjustSfxVolume(MasterVolume * SfxVolume);
		PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
	}

	public void Open() {
		UiManager.Instance.ShowSettings(true, masterVolumeSlider);
		UiManager.Instance.ShowTitle(false);
	}

	public void CloseAndSave() {
		UiManager.Instance.ShowSettings(false, masterVolumeSlider);
		UiManager.Instance.ShowTitle(true);
		PlayerPrefs.Save();
	}

	private void Load() {
		MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
		MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
		SfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);
		
		masterVolumeSlider.value = MasterVolume * sliderQuotient;
	}
#endregion

}

