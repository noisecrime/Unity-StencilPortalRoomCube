// In order to further optimise the scene it is possible to detect when a portal view into any side of the cube is visible to the main camera.
// If it is not visble, then simply disable the portal room that side of the cube reveals.
// This greatly reduces the number of gameObjects being rendered and can result in a sizable boost to performance, but is dependant upon what 
// is in view, how many PortalRooms are in view etc, since Unity will be culling entire rooms if they are not in the viewport anyway.

// One side-effect though is that the Interactive cloths don't always activate for some reason, just one of those weird physics edge-cases
// that need to be resolved, most likely due to having 'pause when not visible' being selected.

// Place component on Portal stencil planes and then set associated portal sub group reference.

using UnityEngine;

public class PortalContentCulling : MonoBehaviour 
{
	public GameObject		_portalGroupForRoom;

	void Start() { } // added so component can be enabled/disabled.

	void OnDisable()
	{
		_portalGroupForRoom.SetActive(true);	
	}

	void Update () 
	{
		// Use the stencilQuad to determine if portal is visible.
		Vector3 viewDir = transform.position - Camera.main.transform.position;

		float visible = Vector3.Dot(transform.forward, viewDir);

		if(_portalGroupForRoom.activeInHierarchy == (visible > 0) ) return;

		_portalGroupForRoom.SetActive(visible > 0);	

	//	Debug.Log ("Portal Active: " + _portalGroupForRoom.name + "  " + (visible > 0) );
	}	
}
