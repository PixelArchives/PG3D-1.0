using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(BoxCollider))]
public class BotMovement : MonoBehaviour
{
	public delegate void DelayedCallback();

	public AudioClip ZombieBiteAudio;

	public AudioClip ZombieVoice;

	public AudioClip ZombieDeadAudio;

	public float MaxDistance = 3f;

	public float RotationSpeed = 5f;

	public AnimationClip animIdle;

	public float animIdleSpeed = 1f;

	public AnimationClip animRun;

	public float animRunSpeed = 1f;

	public AnimationClip animZomb_run;

	public float animZomb_runSpeed = 1f;

	public AnimationClip animZomb_on;

	public AnimationClip animZomb_off;

	public AnimationClip animShut;

	public AnimationClip animDeath;

	private Transform target;

	public Transform _tors;

	private ZombieCreator _gameController;

	private bool Agression;

	private bool deaded;

	private BoxCollider cc;

	public float moveSpeed;

	private Player_move_c healthDown;

	private GameObject player;

	public float CurLifeTime;

	public float LifeTime;

	private void Start()
	{
		healthDown = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		CurLifeTime = LifeTime;
		player = GameObject.FindGameObjectWithTag("Player");
		_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ZombieCreator>();
		_gameController.NumOfLiveZombies++;
		cc = base.gameObject.GetComponent<BoxCollider>();
		target = null;
		base.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		if (!base.gameObject.CompareTag("ZombPolz"))
		{
			base.GetComponent<Animation>()[animShut.name].layer = 1;
			base.GetComponent<Animation>()[animShut.name].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()[animDeath.name].layer = 2;
			base.GetComponent<Animation>()[animDeath.name].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()[animZomb_on.name].layer = 3;
			base.GetComponent<Animation>()[animZomb_on.name].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()[animZomb_off.name].layer = 4;
			base.GetComponent<Animation>()[animZomb_off.name].wrapMode = WrapMode.Once;
		}
		base.GetComponent<Animation>().Stop();
		base.GetComponent<Animation>()[animIdle.name].speed = animIdleSpeed;
		base.GetComponent<Animation>()[animRun.name].speed = animRunSpeed;
		base.GetComponent<Animation>().CrossFade(animIdle.name);
		Walk();
	}

	private void Update()
	{
		if (!deaded)
		{
			if (!(target != null))
			{
				return;
			}
			float num = Vector3.Distance(target.position, base.transform.position);
			if (num >= MaxDistance)
			{
				Vector3 vector = new Vector3(target.position.x, base.transform.position.y, target.position.z);
				base.transform.position = Vector3.Lerp(base.transform.position, vector, moveSpeed * Time.deltaTime / num);
				base.transform.LookAt(vector);
				CurLifeTime = LifeTime;
				return;
			}
			CurLifeTime -= Time.deltaTime;
			if (CurLifeTime <= 0f)
			{
				healthDown.curHealthProp -= 1;
				CurLifeTime = LifeTime;
				base.GetComponent<AudioSource>().PlayOneShot(ZombieBiteAudio);
			}
		}
		else
		{
			float num2 = 7f;
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - num2 * Time.deltaTime, base.transform.position.z);
		}
	}

	public void SetTarget(Transform _target, bool agression)
	{
		Agression = agression;
		if ((bool)_target && target != _target)
		{
			base.GetComponent<AudioSource>().PlayOneShot(ZombieVoice);
			if (!base.gameObject.CompareTag("ZombPolz"))
			{
				base.GetComponent<Animation>().CrossFade(animZomb_on.name);
				base.GetComponent<Animation>().CrossFade(animZomb_run.name);
			}
		}
		else if (!_target && target != _target && !base.gameObject.CompareTag("ZombPolz"))
		{
			base.GetComponent<Animation>().CrossFade(animZomb_off.name);
			Walk();
		}
		target = _target;
		SpawnMonster component = GetComponent<SpawnMonster>();
		component.ShouldMove = _target == null;
	}

	private void Run()
	{
	}

	private void Stop()
	{
	}

	private void Attack()
	{
		if (!base.gameObject.CompareTag("ZombPolz"))
		{
			base.GetComponent<Animation>()[animShut.name].AddMixingTransform(_tors);
			base.GetComponent<Animation>().CrossFade(animShut.name);
		}
	}

	private void Death()
	{
		base.GetComponent<AudioSource>().PlayOneShot(ZombieDeadAudio);
		CodeAfterDelay(ZombieDeadAudio.length, DestroySelf);
		cc.enabled = false;
		deaded = true;
		if (!base.gameObject.CompareTag("ZombPolz"))
		{
			base.GetComponent<Animation>().Stop();
			base.GetComponent<Animation>().Play(animDeath.name);
		}
		SpawnMonster component = GetComponent<SpawnMonster>();
		component.ShouldMove = false;
		_gameController.NumOfDeadZombies++;
		healthDown.score += 50;
	}

	private void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}

	private void Walk()
	{
		base.GetComponent<Animation>().Stop();
		if (!base.gameObject.CompareTag("ZombPolz"))
		{
			base.GetComponent<Animation>().CrossFadeQueued(animZomb_off.name);
			base.GetComponent<Animation>().CrossFadeQueued(animRun.name);
		}
		else
		{
			base.GetComponent<Animation>().CrossFadeQueued(animZomb_run.name);
		}
	}

	private void FixedUpdate()
	{
		if ((bool)base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().velocity = Vector3.zero;
			base.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
	}

	public void CodeAfterDelay(float delay, DelayedCallback callback)
	{
		StartCoroutine(___DelayedCallback(delay, callback));
	}

	private IEnumerator ___DelayedCallback(float time, DelayedCallback callback)
	{
		yield return new WaitForSeconds(time);
		callback();
	}
}
