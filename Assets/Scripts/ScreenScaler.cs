using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenScaler : MonoBehaviour
{
	public CanvasScaler scaler;

	private void Start()
	{
		SetScale(Screen.width, Screen.height);
	}

	public void SetScale(int width, int height)
	{
		if(height / (float)width >= 16f / 9f)
			scaler.matchWidthOrHeight = 0f;
		else
			scaler.matchWidthOrHeight = 1f;
	}
}
