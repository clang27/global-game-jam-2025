using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor {
	[CustomEditor(typeof(Water))]
	public class WaterEditor : UnityEditor.Editor {
		private Water _water;

		private void OnEnable() {
			_water = (Water) target;
		}

		public override VisualElement CreateInspectorGUI() {
			var root = new VisualElement();
			InspectorElement.FillDefaultInspector(root, serializedObject, this);

			root.Add(new VisualElement { style = { height = 10 } });
			var generateMeshButton = new Button(() => _water.GenerateMesh()) {
				text = "Generate Mesh"
			};
			root.Add(generateMeshButton);
			var placeEdgeColliderButton = new Button(() => _water.ResetEdgeCollider()) {
				text = "Place Edge Collider"
			};
			root.Add(placeEdgeColliderButton);

			return root;
		}

		private void ChangeDimensions(ref float width, ref float height, float calculatedWidthMax, float calculatedHeightMax) {
			width = Mathf.Max(0.1f, calculatedWidthMax);
			height = Mathf.Max(0.1f, calculatedHeightMax);
		}

		private void OnSceneGUI() {
			Handles.color = _water.gizmoColor;

			var center = _water.transform.position;
			var size = new Vector3(_water.width, _water.height, 0.1f);
			Handles.DrawWireCube(center, size);

			var handleSize = HandleUtility.GetHandleSize(center) * 0.1f;
			var snap = Vector3.one * 0.1f;
		
			var bottomLeft = center + new Vector3(-_water.width / 2, -_water.height / 2, 0f);
			var bottomRight = center + new Vector3(_water.width / 2, -_water.height / 2, 0f);
			var topLeft = center + new Vector3(-_water.width / 2, _water.height / 2, 0f);
			var topRight = center + new Vector3(_water.width / 2, _water.height / 2, 0f);
		
			EditorGUI.BeginChangeCheck();
			var newBottomLeft = Handles.FreeMoveHandle(bottomLeft, handleSize, snap, Handles.CubeHandleCap);
			if (EditorGUI.EndChangeCheck()) {
				ChangeDimensions(
					ref _water.width, ref _water.height, bottomRight.x - newBottomLeft.x, topRight.y - newBottomLeft.y);
				_water.transform.position +=
					new Vector3((newBottomLeft.x - bottomLeft.x) / 2, (newBottomLeft.y - bottomLeft.y) / 2, 0f);
			}
		
			EditorGUI.BeginChangeCheck();
			var newBottomRight = Handles.FreeMoveHandle(bottomRight, handleSize, snap, Handles.CubeHandleCap);
			if (EditorGUI.EndChangeCheck()) {
				ChangeDimensions(
					ref _water.width, ref _water.height, newBottomRight.x - bottomLeft.x, topRight.y - newBottomRight.y);
				_water.transform.position +=
					new Vector3((newBottomRight.x - bottomRight.x) / 2, (newBottomRight.y - bottomRight.y) / 2, 0f);
			}
		
			EditorGUI.BeginChangeCheck();
			var newTopLeft = Handles.FreeMoveHandle(topLeft, handleSize, snap, Handles.CubeHandleCap);
			if (EditorGUI.EndChangeCheck()) {
				ChangeDimensions(
					ref _water.width, ref _water.height, topRight.x - newTopLeft.x, newTopLeft.y - bottomLeft.y);
				_water.transform.position +=
					new Vector3((newTopLeft.x - topLeft.x) / 2, (newTopLeft.y - topLeft.y) / 2, 0f);
			}
		
			EditorGUI.BeginChangeCheck();
			var newTopRight = Handles.FreeMoveHandle(topRight, handleSize, snap, Handles.CubeHandleCap);
			if (EditorGUI.EndChangeCheck()) {
				ChangeDimensions(
					ref _water.width, ref _water.height, newTopRight.x - topLeft.x, newTopRight.y - bottomRight.y);
				_water.transform.position +=
					new Vector3((newTopRight.x - topRight.x) / 2, (newTopRight.y - topRight.y) / 2, 0f);
			}

			if (GUI.changed) {
				_water.GenerateMesh();
			}
		}
	}
}