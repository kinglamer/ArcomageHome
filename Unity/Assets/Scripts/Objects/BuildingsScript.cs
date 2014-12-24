using UnityEngine;
using System.Collections;

public class BuildingsScript : MonoBehaviour {

	public float MaxHeight;
	public float MaxYCoord;
	public float Height;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position =new Vector3(transform.position.x, (-12 + (MaxYCoord / MaxHeight) * Height),transform.position.z);
	}
}
