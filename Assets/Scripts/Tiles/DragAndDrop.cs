using UnityEngine;
using System.Collections;
using PellyTools;
using Uween;

public class DragAndDrop : MonoBehaviour
{

	[Header("Restrictions")]
	public bool considerScale = true;
	public bool considerOtherObjects = true;

	[Space(5)]

	public Vector2 dragScale = new Vector2(0.25f, 1);
	public Vector4 currentPosition = new Vector4(1, 1, 1, 1);

	Vector2 gridOffset = Vector2.zero;
	Vector2 gridSize = Vector2.one;
	Vector3 screenPoint;

	Vector4 lastPos;
	Vector3 lastParentPos;

	Vector4 targetPos;

	public GameObject overlay;

	Editor editor;

	public Grid.BlockType type;

    void Awake()
	{
		editor = FindObjectOfType<Editor>();
		// Fix the position according to the scale of this object
		var newPos = transform.localPosition;
		newPos.x = (transform.localScale.x / 2f) - 0.5f;
		newPos.y = -((transform.localScale.y / 2f) - 0.5f);

		transform.localPosition = newPos;

		// Update Data
		// Actualizar datos
		UpdateGridData();
		UpdatePosition();

		// Save actual position
		lastParentPos = transform.parent.position;
		lastPos = currentPosition;

		// Add position
		AddPosition(lastPos);
	}

	// Get recent values of the Grid
	void UpdateGridData()
	{
		gridSize = FindObjectOfType<Grid>().gridSize;
		gridOffset = FindObjectOfType<Grid>().GetGridOffset();
	}

	void OnMouseDown()
	{
		// overlay.transform.localScale = new Vector2(1.1f, 1.1f);
		TweenSXYZ.Add(overlay, 0.04f, new Vector3(1.1f, 1.1f)).EaseOutCircular();
		overlay.GetComponent<SpriteRenderer>().enabled = true;

		// Remove the last position
		RemovePosition(lastPos);

		// Update data
		UpdateGridData();

		// Correct the position according to the scale of this object
		var newPos = transform.localPosition;
		newPos.x = (transform.localScale.x / 2f) - 0.5f;
		newPos.y = -((transform.localScale.y / 2f) - 0.5f);

		transform.localPosition = newPos;

		UpdatePosition();
	}

    private void OnMouseOver()
    {
		overlay.GetComponent<SpriteRenderer>().enabled = true;
		Windows.ChangeCursor(Windows.WindowsCursor.Hand);
	}
    private void OnMouseExit()
    {
		overlay.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Al estar presionando el clic sobre este objeto
	void OnMouseDrag()
	{
		Windows.ChangeCursor(Windows.WindowsCursor.Hand);
		// Get World Point using the Mouse Position
		screenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		screenPoint.z = 0;

		// Fix the requested position if the number of the cells is even
		if (gridSize.x % 2 == 0)
		{
			screenPoint.x -= 0.5f;
		}
		if (gridSize.y % 2 == 0)
		{
			screenPoint.y -= 0.5f;
		}

		// Change GameObject position
		// transform.parent.position = SnapToGrid(screenPoint);
		TweenXY.Add(this.gameObject.transform.parent.gameObject, 0.03f, SnapToGrid(screenPoint));
	}

	public void UpdateAll()
	{
		UpdatePosition();
		AddPosition(lastPos);
	}

	// Al soltar el clic sobre este objeto
	void OnMouseUp()
	{
		TweenSXYZ.Add(overlay, 0.04f, new Vector3(1.0f, 1.0f)).EaseInCircular();

		overlay.GetComponent<SpriteRenderer>().enabled = false;

		UpdateGridData();

		// Save Target Pos
		targetPos.x = transform.parent.position.x + (gridSize.x * 0.5f) + 0.5f;
		targetPos.y = (transform.parent.position.x + (gridSize.x * 0.5f) + 0.5f) + transform.localScale.x - 1;

		targetPos.z = -(transform.parent.position.y - (gridSize.y * 0.5f) - 0.5f);
		targetPos.w = -(transform.parent.position.y - (gridSize.y * 0.5f) - 0.5f) + transform.localScale.y - 1;

		// Debug.Log ("Target Position: " + targetPos);

		// Check if it is occupying the position of another object
		if (considerOtherObjects)
		{

			// If it is not busy
			if (!IsOccupied())
			{
				// The last saved position is removed
				RemovePosition(lastPos);

				// And the new position is added
				UpdatePosition();
				AddPosition(targetPos);

			}
			else
			{ 
				// If busy, add the saved position again
				AddPosition(lastPos);
			}
		}
		else
		{
			UpdatePosition();
			AddPosition(targetPos);
		}
	}

	// Agregar posicion
	void AddPosition(Vector4 pos)
	{
		var grid = FindObjectOfType<Grid>();

		var gridList = new Grid.GridSpace();
		gridList.xPos = pos.x;
		gridList.yPos = pos.y;
		gridList.type = type;

		if (!grid.occupiedPositions.Contains(pos))
		{
			grid.occupiedPositions.Add(pos);
			grid.gridList.Add(gridList);
			// Debug.Log ("Added: " + pos);
		}
	}

	// Eliminar posicion
	void RemovePosition(Vector4 pos)
	{
		var grid = FindObjectOfType<Grid>();

		var gridList = new Grid.GridSpace();
		gridList.xPos = pos.x;
		gridList.yPos = pos.y;
		gridList.type = type;

		if (grid.occupiedPositions.Contains(pos))
		{
			grid.occupiedPositions.Remove(pos);
			grid.gridList.Remove(gridList);
		}
	}

	// Check if the target position is occupied
	bool IsOccupied()
	{
		var occupied = FindObjectOfType<Grid>().occupiedPositions;
		foreach (Vector4 pos in occupied)
		{
			if (((targetPos.x >= pos.x && targetPos.x <= pos.y) || (targetPos.y >= pos.x && targetPos.y <= pos.y) || (pos.y >= targetPos.x && pos.y <= targetPos.y))
				&& ((targetPos.z >= pos.z && targetPos.z <= pos.w) || (targetPos.w >= pos.z && targetPos.w <= pos.w) || (pos.w >= targetPos.z && pos.w <= targetPos.w)))
			{

				transform.parent.position = lastParentPos;
				currentPosition = lastPos;
				// Debug.Log ("Wanted: " + wantedPos); 
				// Debug.Log ("Occupied: " + pos);
				// Debug.Log ("Can't move here");
				return true;
			}
		}
		return false;
	}

	// Update object position variable
	void UpdatePosition()
	{
		currentPosition.x = transform.parent.position.x + (gridSize.x * 0.5f) + 0.5f;
		currentPosition.y = (transform.parent.position.x + (gridSize.x * 0.5f) + 0.5f) + transform.localScale.x - 1;

		currentPosition.z = -(transform.parent.position.y - (gridSize.y * 0.5f) - 0.5f);
		currentPosition.w = -(transform.parent.position.y - (gridSize.y * 0.5f) - 0.5f) + transform.localScale.y - 1;

		// Save actual position
		// Guardar posicion actual
		lastParentPos = transform.parent.position;
		lastPos = currentPosition;
	}

	// Fix the GameObject position if the Grid Transform has changed
	public void FixPosition(Vector3 newPos)
	{
		newPos.z = 0;
		transform.parent.position = transform.parent.position + newPos;

		UpdateGridData();
		UpdatePosition();
	}



	// Function that allows you to move an object according to the Grid
	Vector3 SnapToGrid(Vector3 dragPos)
	{
		// If X is even, fix the target position
		if (gridSize.x % 2 == 0)
		{
			dragPos.x = (Mathf.Round(dragPos.x / dragScale.x) * dragScale.x) + 0.5f;
		}
		else
		{
			dragPos.x = (Mathf.Round(dragPos.x / dragScale.x) * dragScale.x);
		}

		// If Y is even, fix the target position
		if (gridSize.y % 2 == 0)
		{
			dragPos.y = (Mathf.Round(dragPos.y / dragScale.y) * dragScale.y) + 0.5f;
		}
		else
		{
			dragPos.y = (Mathf.Round(dragPos.y / dragScale.y) * dragScale.y);
		}

		#region Restrictions

		// Restrict exit from grid
		// Restringir que se pueda salir de la cuadricula
		var maxXPos = ((gridSize.x - 1) * 0.5f) + gridOffset.x;
		var maxYPos = ((gridSize.y - 1) * 0.5f) + gridOffset.y;

		// Considering GameObject Scale
		// Considerando la escala del objeto
		if (considerScale)
		{

			if (dragPos.x > maxXPos - transform.localScale.x + 1)
			{
				dragPos.x = maxXPos - transform.localScale.x + 1;
			}

			if (dragPos.x < -maxXPos + gridOffset.x + gridOffset.x)
			{
				dragPos.x = -maxXPos + gridOffset.x + gridOffset.x;
			}

			if (dragPos.y > maxYPos)
			{
				dragPos.y = maxYPos;
			}

			if (dragPos.y < (-maxYPos + gridOffset.y + gridOffset.y) + transform.localScale.y - 1)
			{
				dragPos.y = -maxYPos + gridOffset.y + gridOffset.y + transform.localScale.y - 1;
			}
		}

		else
		{

			if (dragPos.x > maxXPos)
			{
				dragPos.x = maxXPos;
			}

			if (dragPos.x < -maxXPos + gridOffset.x + gridOffset.x)
			{
				dragPos.x = -maxXPos + gridOffset.x + gridOffset.x;
			}

			if (dragPos.y > maxYPos)
			{
				dragPos.y = maxYPos;
			}

			if (dragPos.y < -maxYPos + gridOffset.y + gridOffset.y)
			{
				dragPos.y = -maxYPos + gridOffset.y + gridOffset.y;
			}
		}

		#endregion

		return dragPos;
	}
}



