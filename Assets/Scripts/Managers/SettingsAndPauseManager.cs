using Enums;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class SettingsAndPauseManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private Slider masterVolumeSlider;
	[SerializeField] private GameObject resumeButton, settingsButton;
	[SerializeField] private GameObject acceptButton;
	[SerializeField] private GameObject quitButton;
	[SerializeField] private float sliderQuotient = 10f;
#endregion

#region Attributes
	public static SettingsAndPauseManager Instance { get; private set; }
	public static float MasterVolume { get; private set; }
	public static float MusicVolume { get; private set; }
	public static float SfxVolume { get; private set; }
#endregion

#region Unity
	private void Awake() {
		Instance = this;
		
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

	public void OpenSettings() {
		if (GameManager.Instance.GameState == GameState.Start) {
			UiManager.Instance.ShowTitle(false);
		}
		
		UiManager.Instance.ShowPauseAndSettings(true);
		UiManager.Instance.ShowPauseGroup(resumeButton, false);
		UiManager.Instance.ShowSettingsGroup(masterVolumeSlider.gameObject, true);
		
		settingsButton.SetActive(false);
		acceptButton.SetActive(true);
		quitButton.SetActive(false);
		resumeButton.SetActive(false);
	}

	public void OpenPause() {
		UiManager.Instance.ShowPauseAndSettings(true);
		UiManager.Instance.ShowPauseGroup(resumeButton, true);
		UiManager.Instance.ShowSettingsGroup(masterVolumeSlider.gameObject, false);
		
		settingsButton.SetActive(true);
		acceptButton.SetActive(false);
		quitButton.SetActive(true);
		resumeButton.SetActive(true);
	}

	public void ClosePauseAndSettings() {
		UiManager.Instance.ShowPauseAndSettings(false);
	}

	public void AcceptSettings() {
		PlayerPrefs.Save();
		
		if (GameManager.Instance.GameState == GameState.Start) {
			ClosePauseAndSettings();
			UiManager.Instance.ShowTitle(true);
		} else {
			OpenPause();
		}
	}

	public void ResetSave() {
		UiManager.Instance.ShowPauseAndSettings(false);
		
		SaveManager.Instance.ClearSave();
		SaveManager.Instance.Load();
		GameManager.Instance.ResetGame();
	}

	private void Load() {
		MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
		MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
		SfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);
		
		masterVolumeSlider.value = MasterVolume * sliderQuotient;
	}
#endregion

}

