using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer),typeof(EdgeCollider2D))]
public class Water : MonoBehaviour {

#region Dependencies
	[Header("Mesh Generation")] 
	[Range(2, 500)] public int numOfXVertices = 70;
	public float width = 10f;
	public float height = 4f;
	public Material material;
	
	[Header("Gizmo")]
	public Color gizmoColor = Color.white;
#endregion

#region Components
	private Mesh _mesh;
	private MeshRenderer _meshRenderer;
	private MeshFilter _meshFilter;
	private Vector3[] _vertices;
	private int[] _topVerticesIndex;
	private EdgeCollider2D _collider;
#endregion

#region Data
	private const int NUM_OF_Y_VERTICES = 2;
#endregion

#region Unity
    private void Reset() {
	    _collider = GetComponent<EdgeCollider2D>();
	    _collider.isTrigger = true;
    }

    private void Start() {
        GenerateMesh();
    }
#endregion

#region Custom
	public void GenerateMesh() {
		_mesh = new Mesh();

		_vertices = new Vector3[numOfXVertices * NUM_OF_Y_VERTICES];
		_topVerticesIndex = new int [numOfXVertices];

		// Add Vertices
		for (var y = 0; y < NUM_OF_Y_VERTICES; y++) {
			for (var x = 0; x < numOfXVertices; x++) {
				var xPos = (x / (float)(numOfXVertices - 1)) * width - width / 2;
				var yPos = (y / (float)(NUM_OF_Y_VERTICES - 1)) * height - height / 2;
				_vertices[y * numOfXVertices + x] = new Vector3(xPos, yPos, 0f);

				if (y == NUM_OF_Y_VERTICES - 1) {
					_topVerticesIndex[x] = y * numOfXVertices + x;
				}
			}
		}

		// Create Triangles
		var triangles = new int[(numOfXVertices - 1) * (NUM_OF_Y_VERTICES - 1) * 6];
		var index = 0;

		for (var y = 0; y < NUM_OF_Y_VERTICES - 1; y++) {
			for (var x = 0; x < numOfXVertices - 1; x++) {
				var bottomLeft = y * numOfXVertices + x;
				var bottomRight = bottomLeft + 1;
				var topLeft = bottomLeft + numOfXVertices;
				var topRight = topLeft + 1;

				triangles[index++] = bottomLeft;
				triangles[index++] = topLeft;
				triangles[index++] = bottomRight;

				triangles[index++] = bottomRight;
				triangles[index++] = topLeft;
				triangles[index++] = topRight;
			}
		}
		
		// UVs
		var uvs = new Vector2[_vertices.Length];
		for (var i = 0; i < _vertices.Length; i++) {
			uvs[i] = new Vector2(
				(_vertices[i].x + width / 2) / width,
				(_vertices[i].y + height / 2) / height
			);
		}

		if (_meshRenderer == null) {
			_meshRenderer = GetComponent<MeshRenderer>();
		}
		
		if (_meshFilter == null) {
			_meshFilter = GetComponent<MeshFilter>();
		}

		_meshRenderer.material = material;
		_mesh.vertices = _vertices;
		_mesh.triangles = triangles;
		_mesh.uv = uvs;

		_mesh.RecalculateNormals();
		_mesh.RecalculateBounds();

		_meshFilter.mesh = _mesh;
	}

	public void ResetEdgeCollider() {
		_collider = GetComponent<EdgeCollider2D>();
		
		var newPoints = new Vector2[2];
		newPoints[0] = new Vector2(_vertices[_topVerticesIndex[0]].x, _vertices[_topVerticesIndex[0]].y);
		newPoints[1] = new Vector2(_vertices[_topVerticesIndex[^1]].x, _vertices[_topVerticesIndex[^1]].y);

		_collider.offset = Vector2.zero;
		_collider.points = newPoints;
	}
#endregion
}