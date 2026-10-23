using System.Collections;
using UnityEngine;

public class BotHealth : MonoBehaviour
{
	public AudioClip zombie_hurt;

	public string myName = "Bot";

	public int myHealth = 100;

	private bool IsLife = true;

	public Texture hitTexture;

	private BotAI ai;

	private Player_move_c healthDown;

	private void Start()
	{
		if (myHealth < 10)
		{
			myHealth = 10;
		}
		ai = GetComponent<BotAI>();
		healthDown = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
	}

	private IEnumerator Flash()
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		if (renderers.Length != 0)
		{
			Texture oldTexture = renderers[0].material.mainTexture;
			Renderer[] array = renderers;
			foreach (Renderer rnd in array)
			{
				rnd.material.mainTexture = hitTexture;
			}
			yield return new WaitForSeconds(0.125f);
			Renderer[] array2 = renderers;
			foreach (Renderer rnd2 in array2)
			{
				rnd2.material.mainTexture = oldTexture;
			}
		}
	}

	public void adjustHealth(int _health, Transform target)
	{
		if (_health < 0)
		{
			StartCoroutine(Flash());
		}
		myHealth += _health;
		if (myHealth < 0)
		{
			myHealth = 0;
		}
		if (myHealth == 0)
		{
			IsLife = false;
		}
		else
		{
			healthDown.score += 5;
		}
		base.GetComponent<AudioSource>().PlayOneShot(zombie_hurt);
		ai.SetTarget(target, true);
	}

	public bool getIsLife()
	{
		return IsLife;
	}

	public int getHealth()
	{
		return myHealth;
	}
}
