using System.Collections;
using UnityEngine;

public class Player_move_c : MonoBehaviour
{
	public GUISkin MySkin;

	public Texture2D AimTexture;

	public int AimTextureWidth = 50;

	public int AimTextureHeight = 50;

	public Transform GunFlash;

	public Transform bullet;

	public int BulletForce = 5000;

	public int MaxAmmoCount = 10;

	public int CurAmmoCount = 10;

	public int InventoryAmmoCount = 120;

	private GameObject _leftJoystick;

	private int _curHealth;

	public int MaxHealth;

	private int DifferenceAmmo;

	public int AmmoBoxWidth = 100;

	public int AmmoBoxHeight = 100;

	public int AmmoBoxOffset = 10;

	public int ScoreBoxWidth = 100;

	public int ScoreBoxHeight = 100;

	public int ScoreBoxOffset = 10;

	public AudioClip Fire;

	public AudioClip Reload;

	public AudioClip Empty;

	private float GunFlashLifetime;

	public GUIStyle ScoreBox;

	public GUIStyle AmmoBox;

	public GUIStyle HealthBox;

	public GUIStyle pauseStyle;

	public GUIStyle pauseFonStyle;

	public GUIStyle resumeStyle;

	public GUIStyle menuStyle;

	public GUIStyle soundStyle;

	private GameObject _fpc;

	public Texture HeartTexture;

	public GameObject damage;

	private bool damageShown;

	public Texture pauseFon;

	private Pauser _pauser;

	public Texture pauseTitle;

	public int score;

	public int CurHealth
	{
		get
		{
			return _curHealth;
		}
		set
		{
			_curHealth = value;
		}
	}

	public int curHealthProp
	{
		get
		{
			return CurHealth;
		}
		set
		{
			if (CurHealth > value && !damageShown)
			{
				StartCoroutine(FlashWhenHit());
			}
			CurHealth = value;
		}
	}

	private void Start()
	{
		NewInput.mouseLocked = true;
		base.GetComponent<Animation>()["Reload"].layer = 1;
		base.GetComponent<Animation>().Stop();
		CurAmmoCount = MaxAmmoCount;
		GunFlash.gameObject.active = false;
		CurHealth = MaxHealth;
		_fpc = GameObject.FindGameObjectWithTag("FirstPersonControl");
		Color color = damage.GetComponent<GUITexture>().color;
		color.a = 0f;
		damage.GetComponent<GUITexture>().color = color;
		_pauser = GameObject.FindGameObjectWithTag("GameController").GetComponent<Pauser>();
		_leftJoystick = GameObject.Find("LeftTouchPad");
	}

	private void ReloadPressed()
	{
		DifferenceAmmo = MaxAmmoCount - CurAmmoCount;
		if (InventoryAmmoCount > 0 && CurAmmoCount != MaxAmmoCount)
		{
			base.GetComponent<Animation>().Stop("Empty");
			base.GetComponent<Animation>().CrossFade("Shoot");
			base.GetComponent<Animation>().Play("Reload");
			if (InventoryAmmoCount >= DifferenceAmmo)
			{
				CurAmmoCount += DifferenceAmmo;
				InventoryAmmoCount -= DifferenceAmmo;
			}
			else
			{
				CurAmmoCount += InventoryAmmoCount;
				InventoryAmmoCount = 0;
			}
			base.GetComponent<AudioSource>().PlayOneShot(Reload);
			_leftJoystick.SendMessage("HasAmmo");
		}
	}

	private void ShotPressed()
	{
		if (base.GetComponent<Animation>().IsPlaying("Reload"))
		{
			return;
		}
		base.GetComponent<Animation>().Stop();
		if (CurAmmoCount > 0)
		{
			base.GetComponent<Animation>().Play("Shoot");
			_fpc.SendMessage("shootS");
			CurAmmoCount--;
			if (CurAmmoCount == 0)
			{
				_leftJoystick.SendMessage("NoAmmo");
			}
			base.GetComponent<AudioSource>().PlayOneShot(Fire);
			GunFlash.gameObject.active = true;
			GunFlashLifetime = 0.1f;
		}
		else
		{
			base.GetComponent<Animation>().Play("Empty");
			base.GetComponent<AudioSource>().PlayOneShot(Empty);
		}
	}

	private IEnumerator Fade(float start, float end, float length, GameObject currentObject)
	{
		for (float i = 0f; i < 1f; i += Time.deltaTime / length)
		{
			Color rgba = currentObject.GetComponent<GUITexture>().color;
			rgba.a = Mathf.Lerp(start, end, i);
			currentObject.GetComponent<GUITexture>().color = rgba;
			yield return 0;
			Color rgba_ = currentObject.GetComponent<GUITexture>().color;
			rgba_.a = end;
			currentObject.GetComponent<GUITexture>().color = rgba_;
		}
	}

	private IEnumerator FlashWhenHit()
	{
		damageShown = true;
		Color rgba = damage.GetComponent<GUITexture>().color;
		rgba.a = 0f;
		damage.GetComponent<GUITexture>().color = rgba;
		float danageTime = 0.15f;
		yield return StartCoroutine(Fade(0f, 1f, danageTime, damage));
		yield return new WaitForSeconds(0.01f);
		yield return StartCoroutine(Fade(1f, 0f, danageTime, damage));
		damageShown = false;
	}

	private void Update()
	{
		if (NewInput.pause) SetPause();

		if (!_pauser.paused)
		{
			if (NewInput.mouseLocked && NewInput.Shooting()) ShotPressed();
			if (NewInput.reload) ReloadPressed();
		}

		if (GunFlashLifetime > 0f)
		{
			GunFlashLifetime -= Time.deltaTime;
		}
		if (GunFlashLifetime <= 0f)
		{
			GunFlash.gameObject.active = false;
		}
		if (CurHealth <= 0)
		{
			if (score > PlayerPrefs.GetInt(Defs.BestScoreSett, 0))
			{
				PlayerPrefs.SetInt(Defs.BestScoreSett, score);
				PlayerPrefs.Save();
			}
			GameCenterSingleton.Instance.ReportScore(score);
			Application.LoadLevel("GameOver");
		}
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(Screen.width / 2 - AimTextureWidth / 2, Screen.height / 2 - AimTextureHeight / 2, AimTextureWidth, AimTextureHeight), AimTexture);
		GUI.skin = MySkin;
		Rect position = new Rect((float)Screen.width - AmmoBox.fixedWidth, 0f, 1f, 50f);
		GUI.Box(position, CurAmmoCount + "/" + InventoryAmmoCount, AmmoBox);
		Rect position2 = new Rect(0f, 0f, 1f, 50f);
		GUI.Box(position2, "SCORE " + score, ScoreBox);
		float num = ((1 == 0) ? ScoreBox.fixedWidth : (ScoreBox.fixedWidth + (position.x - ScoreBox.fixedWidth) / 2f - HealthBox.fixedWidth / 2f));
		GUI.Box(new Rect(num, 0f, 1f, 1f), string.Empty, HealthBox);
		float num2 = 45.4f - (float)HeartTexture.width;
		float top = HealthBox.fixedHeight / 2f - (float)HeartTexture.height / 2f;
		for (int i = 0; i < CurHealth; i++)
		{
			GUI.DrawTexture(new Rect(num + num2 * 0.5f + (float)i * ((float)HeartTexture.width + num2), top, HeartTexture.width, HeartTexture.height), HeartTexture);
		}
		if (NewInput.isMobile && GUI.Button(new Rect((float)Screen.width / 2f - (float)pauseStyle.normal.background.width / 2f, HealthBox.fixedHeight, pauseStyle.normal.background.width, pauseStyle.fixedHeight), string.Empty, pauseStyle))
		{
			SetPause();
		}
		if (_pauser.paused)
		{
			Rect position3 = new Rect(0f, 0f, Screen.width, Screen.height);
			GUI.DrawTexture(position3, pauseFon, ScaleMode.ScaleAndCrop);
			float num3 = 15f;
			Vector2 vector = new Vector2(176f, 150f - num3);
			GUI.DrawTexture(new Rect((float)(Screen.width / 2) + vector.x - (float)(pauseTitle.width / 2), (float)(Screen.height / 2) - vector.y - num3 - (float)pauseTitle.height, pauseTitle.width, pauseTitle.height), pauseTitle);
			if (GUI.Button(new Rect((float)(Screen.width / 2) + vector.x - resumeStyle.fixedWidth / 2f, (float)Screen.height / 2f - vector.y, resumeStyle.fixedWidth, resumeStyle.fixedHeight), string.Empty, resumeStyle))
			{
				SetPause();
			}
			if (GUI.Button(new Rect((float)(Screen.width / 2) + vector.x - menuStyle.fixedWidth / 2f, (float)Screen.height / 2f - vector.y + resumeStyle.fixedHeight + num3, menuStyle.fixedWidth, menuStyle.fixedHeight), string.Empty, menuStyle))
			{
				Time.timeScale = 1f;
				Application.LoadLevel("Restart");
			}
			float num4 = 15f;
			NewUtils.enableAudio = GUI.Toggle(new Rect(num4, (float)Screen.height - num4 - soundStyle.fixedHeight, soundStyle.fixedWidth, soundStyle.fixedHeight), NewUtils.enableAudio, string.Empty, soundStyle);
		}
	}

	private void SetPause()
	{
		_pauser.paused = !_pauser.paused;
		if (_pauser.paused)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}

		NewInput.mouseLocked = !_pauser.paused;
	}

	private void OnDestroy()
	{
		NewInput.mouseLocked = false;
	}
}
