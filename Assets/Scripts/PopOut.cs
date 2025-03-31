using DG.Tweening;
using UnityEngine;

public class PopOut : MonoBehaviour {

#region Dependencies
	[SerializeField][Range(0.1f, 2f)] private float speed = 1f;
	[SerializeField] private float endPosition;
	[SerializeField] private Ease easeType = Ease.InOutSine;
#endregion

#region Components
	private RectTransform _transform;
#endregion
	
#region Unity
	private void Awake() {
		_transform = GetComponent<RectTransform>();
	}
    public void Go() {
	    _transform.DOAnchorPos(new Vector2(_transform.anchoredPosition.x, endPosition), speed).SetEase(easeType);
    }
#endregion

}

