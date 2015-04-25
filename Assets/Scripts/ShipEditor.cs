using AsteroidBelt.Assets.Scripts;
using AsteroidBelt.ShipComponents;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidBelt
{
	public class ShipEditor : Singleton<ShipEditor>
	{
		public GameObject CurrentPart;
		public GameObject EditorTileParent;
		public GameObject EditorTilePrefab;
		public GameObject errorLog;
		public int TilesPerSide;
		private GameObject[,] tiles;

		private enum SearchResult
		{
			Untouched = 0,
			Touched = 1,
			Invalid = 2
		}

		public List<ShipPart> GetShip()
		{
			List<ShipPart> shipPartList = new List<ShipPart>();
			GameObject[] editorTiles = GameObject.FindGameObjectsWithTag("EditorTile");
			foreach (GameObject editorTile in editorTiles)
			{
				ShipEditorTile tileComponent = editorTile.GetComponent<ShipEditorTile>();
				if (tileComponent.Part != null)
				{
					ShipEditorPart shipPartComponent = tileComponent.Part.GetComponent<ShipEditorPart>();

					Vector2 position = editorTile.GetComponent<RectTransform>().anchoredPosition;
					float recWidth = editorTile.GetComponent<RectTransform>().rect.width;
					position.x = position.x / recWidth;
					position.y = position.y / recWidth;

					ShipComponentType shipComponentType = shipPartComponent.ComponentType;
					ShipComponent.Direction direction = shipPartComponent.Direction;

					shipPartList.Add(new ShipPart(shipComponentType, position, direction));
				}
			}

			return shipPartList;
		}

		public void LevelButton()
		{
			if (ValidateConnections())
			{
				GameManager.Instance.SetShipToLoad(GetShip());
				Application.LoadLevel(1);
			}
		}

		public void Validate()
		{
			ValidateConnections();
		}

		public bool ValidateConnections()
		{
			errorLog.GetComponent<Text>().text = "";
			int totalNumberOfParts = 0;
			bool pass = true;
			int firstX = 0;
			int firstY = 0;

			for (int y = 0; y < TilesPerSide; ++y) // get total number of parts
			{
				for (int x = 0; x < TilesPerSide; ++x)
				{
					if (tiles[x, y].GetComponent<ShipEditorTile>().Part != null)
					{
						firstX = x;
						firstY = y;
						totalNumberOfParts++;
					}
				}
			}
			if (totalNumberOfParts == 1)
			{
				errorLog.GetComponent<Text>().text = "Your ship has " + totalNumberOfParts + " part.\n";
			}
			else if (totalNumberOfParts > 1)
			{
				errorLog.GetComponent<Text>().text = "Your ship has " + totalNumberOfParts + " parts.\n";
				SearchResult[,] searchResults = ValidationSearch(firstX, firstY, new SearchResult[TilesPerSide, TilesPerSide]);

				int totalTouchedComponents = 0;
				for (int y = 0; y < TilesPerSide; ++y) // get total number of parts
				{
					for (int x = 0; x < TilesPerSide; ++x)
					{
						if (searchResults[x, y] != SearchResult.Untouched)
						{
							totalTouchedComponents++;
						}
						if (searchResults[x, y] == SearchResult.Invalid)
						{
							pass = false;
						}
					}
				}
				if (totalTouchedComponents != totalNumberOfParts)
				{
					errorLog.GetComponent<Text>().text += "Only " + totalTouchedComponents + " parts are connected";
					pass = false;
				}
			}
			else
			{
				errorLog.GetComponent<Text>().text += "Your ship has no parts!";
				pass = false;
			}

			return pass;
		}

		private void Start()
		{
			tiles = new GameObject[TilesPerSide, TilesPerSide];

			//instantiate all the editor tiles
			for (int x = 0; x < TilesPerSide; ++x)
			{
				for (int y = 0; y < TilesPerSide; ++y)
				{
					GameObject newEditorTile = Instantiate(EditorTilePrefab);
					newEditorTile.transform.SetParent(EditorTileParent.transform);
					Rect editorRect = newEditorTile.GetComponent<RectTransform>().rect;
					newEditorTile.GetComponent<RectTransform>().anchoredPosition = new Vector2((x - TilesPerSide / 2) * editorRect.width, (y - TilesPerSide / 2) * editorRect.height);
					newEditorTile.tag = "EditorTile";
					tiles[x, y] = newEditorTile;
				}
			}
		}

		//private ShipComponent tileToShipComponent(ShipEditorTile tile)
		//{
		//	if (tile.Part == null) return null;
		//	return tile.Part.GetComponent<ShipEditorPart>().Part.GetComponent<ShipComponent>();
		//}

		private void Update()
		{
			if (CurrentPart != null)
			{
				CurrentPart.GetComponent<RectTransform>().position = Input.mousePosition;

				if (!Input.GetMouseButton(0))
				{
					Destroy(CurrentPart);
					CurrentPart = null;
				}
			}
		}

		private SearchResult[,] ValidationSearch(int currX, int currY, SearchResult[,] searchTable)
		{
			ShipEditorTile currentTile = tiles[currX, currY].GetComponent<ShipEditorTile>();
			ShipEditorPart currentComponent = currentTile.Part.GetComponent<ShipEditorPart>();
			searchTable[currX, currY] = SearchResult.Touched;
			int nextX = currX;
			int nextY = currY;

			for (int i = 0; i < 4; ++i)
			{
				int connDir = ((int)currentComponent.Direction + (4 - i)) % 4;
				ShipComponent.PossibleConnection possibleConnection = currentComponent.Part.GetComponent<ShipComponent>().possibleConnections[connDir];
				ShipEditorTile connectedTile;
				switch ((ShipComponent.Direction)i)
				{
					case ShipComponent.Direction.Left:
						if (currX - 1 >= 0)
						{
							nextX = currX - 1;
							nextY = currY;
						}
						else
						{
							continue;
						}
						break;

					case ShipComponent.Direction.Right:
						if (currX + 1 < TilesPerSide)
						{
							nextX = currX + 1;
							nextY = currY;
						}
						else
						{
							continue;
						}
						break;

					case ShipComponent.Direction.Up:
						if (currY + 1 < TilesPerSide)
						{
							nextX = currX;
							nextY = currY + 1;
						}
						else
						{
							continue;
						}
						break;

					case ShipComponent.Direction.Down:
						if (currY - 1 >= 0)
						{
							nextX = currX;
							nextY = currY - 1;
						}
						else
						{
							continue;
						}
						break;
				}

				connectedTile = tiles[nextX, nextY].GetComponent<ShipEditorTile>();
				if (connectedTile.Part != null) // if there is a component on the other side
				{
					ShipEditorPart connectedShipComponent = connectedTile.Part.GetComponent<ShipEditorPart>();
					int othersConnDir = ((int)connectedShipComponent.Direction + (4 - i) + 2) % 4;
					ShipComponent.PossibleConnection othersPossibleConnection = connectedShipComponent.Part.GetComponent<ShipComponent>().possibleConnections[othersConnDir]; // gettting out of bounds here
					if (possibleConnection == ShipComponent.PossibleConnection.MustBeEmpty)
					{
						searchTable[currX, currY] = SearchResult.Invalid;
						errorLog.GetComponent<Text>().text += "Error: a component is bordered on a side that needs to be open\n";
					}

					// if we can connect to the other component and we have not already touched it in our search, search it next
					if (othersPossibleConnection == ShipComponent.PossibleConnection.Yes && possibleConnection == ShipComponent.PossibleConnection.Yes && searchTable[nextX, nextY] == SearchResult.Untouched)
					{
						searchTable = ValidationSearch(nextX, nextY, searchTable);
					}
				}
			}
			return searchTable;
		}
	}
}