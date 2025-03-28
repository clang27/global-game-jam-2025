using UnityEngine;

public class LampBehavior : MonoBehaviour {

#region Dependencies
	[SerializeField] private Sprite litSprite, unlitSprite;
#endregion

#region Attributes
	public bool On { get; private set; }
#endregion

#region Components
	private SpriteRenderer _spriteRenderer;
	private GameObject _glowGameObject;
#endregion

#region Unity
    private void Awake() {
	    _spriteRenderer = GetComponent<SpriteRenderer>();
	    _glowGameObject = transform.GetChild(0).gameObject;
    }

    private void Start() {
        TurnOff();
    }

    private void OnTriggerEnter2D(Collider2D other) {
	    if (!On) {
		    TurnOn();    
	    }
    }
#endregion

#region Custom
	public void TurnOn() {
		On = true;
		
		_spriteRenderer.sprite = litSprite;
		_glowGameObject.SetActive(true);
	}

	public void TurnOff() {
		On = false;
		
		_spriteRenderer.sprite = unlitSprite;
		_glowGameObject.SetActive(false);
	}
#endregion

}

