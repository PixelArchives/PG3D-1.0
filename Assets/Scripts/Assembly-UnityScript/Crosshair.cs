using System;
using UnityEngine;

[Serializable]
public class Crosshair : MonoBehaviour
{
	public Texture2D crosshairTexture;

	public Rect position;

	public virtual void Start()
	{
		position = new Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height - crosshairTexture.height) / 2, crosshairTexture.width, crosshairTexture.height);
	}

	public virtual void OnGUI()
	{
		GUI.DrawTexture(position, crosshairTexture);
	}

	public virtual void Main()
	{
	}
}
