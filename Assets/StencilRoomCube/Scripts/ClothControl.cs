// Notes:
// Simply toggling the cloth gameObject on/off will cuase the cloth to behave eradically.
// To avoid this when enabled the cloth component is disabled then re-enabled.
// This resets its state, but results in greater stablility. 

using UnityEngine;

public class ClothControl : MonoBehaviour
{			
	void OnEnable()
	{
		GetComponent<Cloth>().enabled = false;
		GetComponent<Cloth>().enabled = true;
	}

	void OnDisable()
	{
		GetComponent<Cloth>().enabled = false;
	}
}
