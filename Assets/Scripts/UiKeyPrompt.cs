using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UiKeyPrompt : MonoBehaviour {

#region Dependencies
	[SerializeField] private Sprite keyboardSprite, controllerSprite;
#endregion

#region Components
	private Transform _transform;
	private Image _image;
#endregion
	
#region Unity
    private void Awake() {
	    _transform = transform;
	    _image = GetComponent<Image>();
    }
#endregion

#region Custom
	public void ChangeToXbox() {
		if (_image) {
			_image.sprite = controllerSprite;	
		}
	}
	
	public void ChangeToPc() {
		if (_image) {
			_image.sprite = keyboardSprite;	
		}
	}
	
	public void Hide() {
		_image.DOFade(0f, 0f);
	}
	
	public void Show() {
		_image.DOFade(1f, 1f);
	}
#endregion

}

