using UnityEngine;

public class crossHair : MonoBehaviour
{
	public Texture2D crossHairTexture;

	private Rect crossHairPosition;

	private void Start()
	{
		crossHairPosition = new Rect((Screen.width - crossHairTexture.width) / 2, (Screen.height - crossHairTexture.height) / 2, crossHairTexture.width, crossHairTexture.height);
	}

	private void OnGUI()
	{
		GUI.DrawTexture(crossHairPosition, crossHairTexture);
	}
}
