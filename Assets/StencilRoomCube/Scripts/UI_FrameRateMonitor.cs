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
using UnityEngine.UI;
using System.Collections;

public class UI_FrameRateMonitor : MonoBehaviour
{
	public  float       m_UpdateFrequency   = 1.0f;     // Frequency in seconds betwen visual updates

	private float       m_FramerateMin      = 65535.0f;
	private float       m_FramerateMax      = 0.0f;
	private float       m_FramerateAvg      = 0.0f;
	private float       m_Accumelated       = 0.0f;
	private int         m_AccumelatedCount  = 0;

	private float       m_NextTime          = 1;
	private string      m_DisplayFormatStr  = " Now: {0:F2}  Min: {1:F2}  Max: {2:F2}  Avg: {3:F2}  Delta: {4:F4} / {5:F2}   Time: {6:F2}";
	
	private Text		m_TextComponent;

	void Awake()
	{
		m_TextComponent = GetComponent<Text>();
	}

	void OnEnable()
	{

		Reset();
		StartCoroutine( FrameUpdate() );
	}

	void OnDisable()
	{
		StopAllCoroutines();
	}

	IEnumerator FrameUpdate()
	{
		// Skip a while to avoid delta spikes
		// yield return new WaitForSeconds(0.5f);
		yield return null;

		m_NextTime = Time.realtimeSinceStartup + m_UpdateFrequency;

		float timeDelta;

		while ( true )
		{
			timeDelta = Time.deltaTime;         //	Time.timeScale / Time.deltaTime;
			m_Accumelated += timeDelta;
			m_AccumelatedCount++;

			if ( timeDelta < m_FramerateMax ) m_FramerateMax = timeDelta;
			if ( timeDelta > m_FramerateMin ) m_FramerateMin = timeDelta;

			if ( Time.realtimeSinceStartup > m_NextTime )
			{
				// if(Input.GetKey(KeyCode.R)) Reset();

				m_FramerateAvg			= 1f / ( m_Accumelated / m_AccumelatedCount );

				m_TextComponent.text	= string.Format( m_DisplayFormatStr, 1f / timeDelta, 1f / m_FramerateMin, 1f / m_FramerateMax, m_FramerateAvg, Time.deltaTime, 1f / Time.deltaTime, Time.realtimeSinceStartup );

				m_Accumelated			= 0;
				m_AccumelatedCount		= 0;
				m_FramerateMin			= 0.0f;
				m_FramerateMax			= 65536.0f;

				// Skip 2 frames to avoid delta spikes
				yield return null;
				yield return null;
				m_NextTime = Time.realtimeSinceStartup + m_UpdateFrequency;
			}

			yield return null;
		}
	}

	void Reset()
	{
		m_FramerateMin = 0.0f;
		m_FramerateMax = 65536.0f;
		m_FramerateAvg = 0.0f;
		m_Accumelated = 0.0f;
		m_AccumelatedCount = 0;
		m_TextComponent.text = " Resetting fps ...";
		m_NextTime = Time.realtimeSinceStartup + m_UpdateFrequency;
	}
}
