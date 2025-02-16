using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers {
	public class BubbleManager : MonoBehaviour, IManager {

		#region Dependencies
		// [SerializeField] private GameObject[] Entries;
		#endregion

		#region Attributes
		public static BubbleManager Instance { get; private set; }
		private List<SmallBubbleBehavior> SmallBubbles { get; set; } = new();
		private List<LargeBubbleBehavior> LargeBubbles { get; set; } = new();
	
		public SmallBubbleBehavior SmallBubblePlayerIsOn { get; set; }
		public LargeBubbleBehavior LargeBubblePlayerIsOn { get; set; }
		
		private string LargeBubbleName { get; set; }
		
		public bool PlayerIsOnBubble => SmallBubblePlayerIsOn || LargeBubblePlayerIsOn;
		public Vector2 BubbleVelocity => LargeBubblePlayerIsOn ? LargeBubblePlayerIsOn.Velocity : Vector2.zero;
		#endregion
		

		#region Data
		// private Coroutine _marchOverCoroutine;
		#endregion

		#region Unity
		private void Awake() {
			Instance = this;
		}

		private void OnEnable() {
			foreach (var b in SmallBubbles) {
				b.enabled = true;
			}
		}
    
		private void OnDisable() {
			foreach (var b in SmallBubbles) {
				b.enabled = false;
			}
		}

		#endregion

		#region Custom
		public void Init() {
			enabled = false;
		}
		
		public void PreBubbleRide(string sceneName) {
			LargeBubbleName = LargeBubblePlayerIsOn.name;
			LargeBubblePlayerIsOn.StartMovingSlowly(false);

			PlayerManager.Instance.enabled = false;
			PlayerManager.Instance.GoToCenterOfBubble(LargeBubblePlayerIsOn);
		}

		public void PostSceneLoad(string sceneName) {
			SmallBubbles = FindObjectsByType<SmallBubbleBehavior>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
			LargeBubbles = FindObjectsByType<LargeBubbleBehavior>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
			
			var bubbleOn = LargeBubbles.Find(bubble => bubble.name.Equals(LargeBubbleName));

			if (bubbleOn) {
				bubbleOn.StartMovingInstantly(true);
				PlayerManager.Instance.HopLargeBubbles(bubbleOn);
				GameManager.Instance.EndLargeBubbleTransition();
			}
		}

		public void PostBubbleRide(string sceneName) {
			PlayerManager.Instance.enabled = true;
		}
	
		#endregion

	}
}

