using System;
using DG.Tweening;
using Enums;
using Managers;
using UnityEngine;

public class WinScreen : MonoBehaviour {
	
#region Unity
    public void Awake() {
	    var x = GameObject.FindGameObjectWithTag("Win");
	    x.GetComponent<CanvasGroup>().alpha = 1f;
	    
	    UiManager.Instance.ShowHud(false);
	    PlayerManager.Controller.Enabled = false;
	    GameManager.Instance.GameState = GameState.Win;
	    
	    foreach (var retrieve in FindObjectsByType<RetrieveDollar>(FindObjectsSortMode.None)) {
		    retrieve.Go();
	    }

	    DOVirtual.DelayedCall(0.5f, () => {
		    foreach (var popOut in FindObjectsByType<PopOut>(FindObjectsSortMode.None)) {
			    popOut.Go();
		    }

		    foreach (var shrinkGrow in FindObjectsByType<ShrinkGrow>(FindObjectsSortMode.None)) {
			    shrinkGrow.Go();
		    }
	    });
    }

    public void OnDestroy() {
	    var x = GameObject.FindGameObjectWithTag("Win");
	    x.GetComponent<CanvasGroup>().alpha = 0f;
    }
#endregion

}

