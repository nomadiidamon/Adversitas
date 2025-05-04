using UnityEngine;

public class Grid
{
	public int width;
	public int height;
	public float cellSize;
	public int[,] gridArray;


	public Grid(int _width, int _height, float _cellSize)
	{
		width = _width;
		height = _height;
		gridArray = new int[width, height];

		for (int x = 0; x < gridArray.GetLength(0); x++) {

			for (int y = 0; y < gridArray.GetLength(1); y++) {
		
				Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x,y + 1), Color.red, 100f);
				Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.red, 100f);
				
			}
		}
				Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
				Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);

	}

	private Vector3 GetWorldPosition(int x, int y)
	{
		return new Vector3(x, y) * cellSize;
	}

	public void SetValue(int x, int y, int targetValue)
	{
		if (x>=0 && y>=0 && x < width && y < height)
		{
			gridArray[x,y] = targetValue;

		}
	}




}
