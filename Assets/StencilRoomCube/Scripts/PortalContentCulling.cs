// MIT License
//
// Copyright (c) 2017 Noisecrime
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

/// <remarks>
/// In order to further optimise the scene it is possible to detect when a portal view into any side of the cube is visible to the main camera.
/// If it is not visble, then simply disable the portal room that side of the cube reveals.
/// This greatly reduces the number of gameObjects being rendered and can result in a sizable boost to performance, but is dependant upon what 
/// is in view, how many PortalRooms are in view etc, since Unity will be culling entire rooms if they are not in the viewport anyway.
///
/// One side-effect though is that the Interactive cloths don't always activate for some reason, just one of those weird physics edge-cases
/// that need to be resolved, most likely due to having 'pause when not visible' being selected.
///
/// Place component on Portal stencil planes and then set associated portal sub group reference.
/// </remarks>

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
