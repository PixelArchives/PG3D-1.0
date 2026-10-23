using UnityEngine;

public class LoadLevel : MonoBehaviour
{
	public Texture fon;

	private void Start()
	{
		Application.LoadLevel("Level2");
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fon, ScaleMode.ScaleAndCrop);
	}

	private void Update()
	{
	}
}
