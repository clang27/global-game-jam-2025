using DG.Tweening;
using UnityEngine;

public class KeyPrompt : MonoBehaviour {

#region Dependencies
	[SerializeField] private Sprite keyboardSprite, controllerSprite;
#endregion

#region Components
	private Transform _transform;
	private SpriteRenderer _spriteRenderer;
	private float _startingScale;
#endregion
	
#region Unity
    private void Awake() {
	    _transform = transform;
	    _startingScale = _transform.localScale.x;
	    _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
	    _spriteRenderer.DOFade(0f, 0f);
    }
#endregion

#region Custom
	public void Bounce() {
		_transform.DOKill();
		_transform.DOScale(_startingScale, 0f);
		_transform.DOScale(_startingScale * 0.9f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic);
	}
	public void ChangeToXbox() {
		_spriteRenderer.sprite = controllerSprite;
	}
	
	public void ChangeToPc() {
		_spriteRenderer.sprite = keyboardSprite;
	}
	
	public void Show() {
		_spriteRenderer.DOKill();
		_spriteRenderer.DOFade(1f, 1f);
	}
#endregion

}

