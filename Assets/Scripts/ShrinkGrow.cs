using DG.Tweening;
using UnityEngine;

public class ShrinkGrow : MonoBehaviour {

#region Dependencies
	[SerializeField][Range(1f, 2f)] private float scale = 1.1f;
	[SerializeField][Range(0.1f, 2f)] private float speed = 1f;
	[SerializeField] private Ease easeType = Ease.InOutBounce;
#endregion

#region Components
	private RectTransform _transform;
#endregion
	
#region Unity
	private void Awake() {
		_transform = GetComponent<RectTransform>();
	}
	
	public void Go()  {
	    _transform.DOScale(_transform.localScale.x * scale, speed).SetEase(easeType).SetLoops(-1, LoopType.Yoyo);
    }
#endregion

}

