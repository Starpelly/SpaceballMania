using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
	public enum BlockType
	{
		Baseball = 0,
		Riceball = 1,
		Highball = 2,
		Highriceball = 3,
		Costume1 = 4,
		Costume2 = 5,
		Costume3 = 6,
		Camera = 7
	}

	public Vector2 gridSize = new Vector2(1, 1);

	public Vector2 gridOffset;
	Vector3 lastGridSize;
	Vector3 lastPosition;

	public List<Vector4> occupiedPositions;

	[System.Serializable]
	public class GridSpace : IEquatable<GridSpace>
	{
		public float xPos;
		public float yPos;
		public BlockType type;
		public float length { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			GridSpace objAsPart = obj as GridSpace;
			if (objAsPart == null) return false;
			else return Equals(objAsPart);
		}
		public bool Equals(GridSpace other)
		{
			if (other == null) return false;
			return (this.xPos.Equals(other.xPos));
		}
		public override int GetHashCode()
		{
			return -1;
		}
	}

	public List<GridSpace> gridList;

	void Awake()
	{
		UpdateScale();
		FixPositions();
	}

	void Update()
	{
		// If the transform has changed
		if (transform.hasChanged)
		{
			// Update the grid texture size and fix positions of the draggable gameobjects
			transform.hasChanged = false;
			// occupiedPositions.Clear();
			GetComponent<Renderer>().material.mainTextureScale = gridSize;
			// FixPositions();
			// gridSize = transform.localScale;
		}

		// If grid size has changed
		if ((gridSize.x != lastGridSize.x) || (gridSize.y != lastGridSize.y))
		{
			// Actualizamos la escala del objeto
			lastGridSize = gridSize;
			UpdateScale();

		}
	}

	int runOnce;

	// Update transform and texture size
	void UpdateScale()
	{
		Resize(new Vector3(gridSize.x, gridSize.y), Vector3.right);
		transform.localScale = new Vector3(gridSize.x, gridSize.y, 1);
		transform.position += new Vector3(2000, 0);
		GetComponent<Renderer>().material.mainTextureScale = gridSize;
	}

	public void Resize(Vector3 amount, Vector3 direction)
    {
		transform.localPosition = new Vector3(direction.x * amount.x - 11f, direction.y * amount.y) / 2;
		// transform.localScale = new Vector3(direction.x * amount.x, direction.y * amount.y);
    }

	// Fix draggable gameobjects positions
	void FixPositions()
	{
		var objs = FindObjectsOfType<DragAndDrop>();
		var diff = transform.localPosition - lastPosition;
		foreach (DragAndDrop i in objs)
		{
			i.FixPosition(diff);
			i.UpdateAll();
		}
		lastPosition = transform.localPosition;
	}

	// Obtener el Offset de la cuadricula
	public Vector2 GetGridOffset()
	{
		gridOffset.x = transform.localPosition.x;
		gridOffset.y = transform.localPosition.y;
		return gridOffset;
	}
}
