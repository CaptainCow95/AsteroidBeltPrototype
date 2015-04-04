using UnityEngine;
using System.Collections;

public class ShipComponent : MonoBehaviour {

    public Ship parentShip;
	// Use this for initialization
	void Start () {
        gameObject.transform.SetParent(parentShip.gameObject.transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
