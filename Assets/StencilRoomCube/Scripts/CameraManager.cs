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

using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour 
{	
	public	Transform		_lookatTarget;
	public	Transform		_cameraToControl;
		
	public	float			_radius		= 256.0f;
	public	float			_minRadius	= 10.0f;
	public	float			_maxRadius	= 32.0f;
	
	public	float			_elevation 		=  0.0f;
	public	float			_minElevation 	=  0.0f;
	public	float			_maxElevation 	= 30.0f;	
	
	public	float			xRotSpeed		= 1.0f;
	public	float			yRotSpeed		= 1.0f;
	public	float			translateSpeed = 0.75f;

	public	float			wheelSpeed	= 1.0f;
	
	public	int				yMinLimit	= -20;
	public	int				yMaxLimit	=  80;
		
	public	float 			_rotX 		= -50.0f;
	public	float		 	_rotY 		= 20.8f; 
	
	
	void Start()
	{
		RefreshCamera();
	}

	void Update()
	{
		UpdateCamera();
	}


	public void UpdateCamera()
	{
		bool orbitFlag  = Input.GetMouseButton(0);
		bool radiusFlag = Input.GetMouseButton(1) || ( orbitFlag && (Input.GetKey(KeyCode.LeftShift)   || Input.GetKey(KeyCode.RightShift) ) );
		bool panFlag    = Input.GetMouseButton(2) || ( orbitFlag && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ));

		UpdateCameraControls( orbitFlag, radiusFlag, panFlag, ( Input.GetAxis( "Mouse ScrollWheel" ) != 0 ) );
	}

																																																																								
	public void UpdateCameraControls (bool orbitFlag, bool radiusFlag, bool panFlag, bool wheelFlag) 
	{
		if ( radiusFlag ) 
		{				
			AdjustCameraRadius( Input.GetAxis("Mouse Y") * xRotSpeed * Time.smoothDeltaTime * 80f);	
		}
		else if ( wheelFlag )
		{
			AdjustCameraRadius( Input.GetAxis("Mouse ScrollWheel") * wheelSpeed );	
		}
		else if ( panFlag )
		{
			Vector3	tmp = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
			tmp = _cameraToControl.TransformDirection(tmp);	
			// tmp.y = 0;
			tmp.Normalize();			
				
			_lookatTarget.Translate(tmp * translateSpeed ); 
		}				
		else if (orbitFlag )
		{				
			_rotX += Input.GetAxis("Mouse X") * xRotSpeed; 
			_rotY -= Input.GetAxis("Mouse Y") * yRotSpeed;
					
			// Wrap around
			if (_rotX < -360) _rotX += 360;
			if (_rotX >  360) _rotX -= 360;
		
			_rotY = wrapAngle(_rotY, yMinLimit, yMaxLimit);					
		}								
				
		if (orbitFlag || panFlag || radiusFlag || wheelFlag) RefreshCamera();	
	}	
	

		
	public void RefreshCamera()
	{		
		Vector3 	dist 		= new Vector3(0.0f, 0.0f, -_radius);		
		Quaternion 	rotation 	= Quaternion.Euler(_rotY, _rotX, 0.0f);	
		Vector3 	position 	= rotation * dist + _lookatTarget.position;
		
		_cameraToControl.localRotation = rotation;
		_cameraToControl.localPosition = position;
	}
	
	
	public void Camera_OrbitMovement(bool useAxisX)
	{
		if(useAxisX)
		{
			_rotX += Input.GetAxis("Mouse X") * xRotSpeed; 
		
			// Wrap around
			if (_rotX < -360) _rotX += 360;
			if (_rotX >  360) _rotX -= 360;
		}
		
		_rotY -= Input.GetAxis("Mouse Y") * yRotSpeed;
		_rotY  = wrapAngle(_rotY, yMinLimit, yMaxLimit);
		
		RefreshCamera();	
	}
		
	void AdjustCameraRadius( float amount)
	{		
		_radius =  Mathf.Clamp (_radius-amount, _minRadius, _maxRadius);					
	}	
			
	public void SetCameraRadiusByValue(float offset)
	{
		_radius =  Mathf.Clamp (offset, _minRadius, _maxRadius);		
		RefreshCamera();
	}	
	
	public void SetCameraRadiusByRatio(float ratio)
	{
		_radius = _minRadius + ( _maxRadius - _minRadius ) * ratio;
		
		RefreshCamera();
	}
	
	public float GetCameraRadiusByRatio()
	{
		return (_radius-_minRadius)/( _maxRadius - _minRadius );
	}
	
	float wrapAngle ( float angle, float min, float max)
	{
		// Wrap around
		if (angle < -360) angle += 360;
		if (angle > 360)  angle -= 360;
		
		return Mathf.Clamp (angle, min, max);
	}
}
