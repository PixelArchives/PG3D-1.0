using UnityEngine;

public class Switcher : MonoBehaviour
{
	public Texture fon;

	private void Start()
	{
		Invoke("LoadMenu", 2.5f);
	}

	private void OnGUI()
	{
		Rect position = new Rect(0f, 0f, Screen.width, Screen.height);
		GUI.DrawTexture(position, fon, ScaleMode.ScaleAndCrop);
	}

	private void LoadMenu()
	{
		Application.LoadLevel("Restart");
	}
}
