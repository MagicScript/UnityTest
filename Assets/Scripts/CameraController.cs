using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject selector;

	private bool panning_ = false;
	private Vector3 lastMouseDragPosition;

	// Use this for initialization
	void Start () {
		selector.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0))
		{
			World world = WorldController.Current.World;
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 position = new Vector3(mouseRay.origin.x, mouseRay.origin.y, 0.0f);
			if(WorldController.Current.IsInWorld(position))
			{
				Tile cell = world.GetClosestTile(position);

				Army army = world.GetArmyAt(cell.X, cell.Y);
				City city = world.GetCityAt(cell.X, cell.Y);
				if(army != null)
				{
					selector.transform.position = WorldController.Current.GetPositionForCell(city.X, city.Y);
					selector.SetActive(true);

					UIController.Current.Select(army);
				}
				else if(city != null)
				{
					selector.transform.position = WorldController.Current.GetPositionForCell(city.X, city.Y);
					selector.SetActive(true);

					UIController.Current.Select(city);
				}
				else
				{
					selector.SetActive(false);
					UIController.Current.SelectNothing();
				}
			}
			else
			{
				selector.SetActive(false);
				UIController.Current.SelectNothing();
			}
		}
	
		//Right mouse button pans.
		if (Input.GetMouseButtonDown (1))
		{
			panning_ = true;
			lastMouseDragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			lastMouseDragPosition.z = 0.0f;
		}

		if (Input.GetMouseButton (1))
		{
			if(panning_)
			{
				Vector3 currMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				currMousePosition.z = 0.0f;

				Camera.main.transform.Translate(lastMouseDragPosition - currMousePosition);

				//The camera has moved so we need to re-query the last position or else you'll get gitters.
				lastMouseDragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				lastMouseDragPosition.z = 0.0f;
			}
		}

		if (Input.GetMouseButtonUp (1))
		{
			panning_ = false;
		}

		//Mouse scroll wheel zooms.
		if (Input.mouseScrollDelta.y != 0)
		{
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize * (1.0f - Input.mouseScrollDelta.y * 0.10f), 2.0f, 10.0f);
		}

	}
}
