using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public Texture fon;

	public GUIStyle playStyle;

	public GUIStyle soundStyle;

	public GUIStyle bestScoreStyle;

	private GameCenterSingleton _gc;

	private void Start()
	{
		_gc = GameCenterSingleton.Instance;
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fon, ScaleMode.ScaleAndCrop);
		if (GUI.Button(new Rect((float)(Screen.width / 2) - playStyle.fixedWidth / 2f, (float)(Screen.height / 2) - playStyle.fixedHeight / 2f, playStyle.fixedWidth, playStyle.fixedHeight), string.Empty, playStyle))
		{
			Application.LoadLevel("LoadingNoWait");
		}
		float num = 15f;
		NewUtils.enableAudio = GUI.Toggle(new Rect(num, (float)Screen.height - num - soundStyle.fixedHeight, soundStyle.fixedWidth, soundStyle.fixedHeight), NewUtils.enableAudio, string.Empty, soundStyle);
		GUI.Box(new Rect((float)(Screen.width / 2) - bestScoreStyle.fixedWidth / 2f, (float)Screen.height * 0.85f - bestScoreStyle.fixedHeight / 2f, bestScoreStyle.fixedWidth, bestScoreStyle.fixedHeight), "BEST SCORE " + PlayerPrefs.GetInt(Defs.BestScoreSett, 0), bestScoreStyle);
	}
}
