using Managers;
using TMPro;
using UnityEngine;

public class RetrieveDollar : MonoBehaviour {
    
#region Components
    private TextMeshProUGUI _textMesh;
#endregion
	
#region Unity
    private void Awake() {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }
    
    public void Go() {
        _textMesh.text = CoinManager.Instance ? $"${CoinManager.Instance.Dollars}" : "$0";
    }
#endregion

}