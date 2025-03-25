using UnityEngine;

public class RandomBed : MonoBehaviour {

#region Dependencies
	[SerializeField] private Sprite[] bedSprites;
	[SerializeField] private SpriteRenderer[] bedSpriteRenderers;
#endregion

#region Unity
    private void Start() {
        foreach (var r in bedSpriteRenderers) {
            r.sprite = bedSprites[Random.Range(0, bedSprites.Length)];
        }
    }
#endregion
}

