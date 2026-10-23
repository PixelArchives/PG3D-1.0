using UnityEngine;

[RequireComponent(typeof(BotMovement))]
public class BotAI : MonoBehaviour
{
	private bool Agression;

	private bool deaded;

	private Transform Target;

	private Transform myTransform;

	private BotMovement _motor;

	private BotHealth _eh;

	private float attackTimer;

	public float coolDown = 2f;

	private BotTrigger _botTrigger;

	public Transform homePoint;

	private void Start()
	{
		Target = null;
		_motor = GetComponent<BotMovement>();
		_eh = GetComponent<BotHealth>();
		attackTimer = 0f;
		myTransform = base.transform;
		_botTrigger = GetComponent<BotTrigger>();
	}

	private void Update()
	{
		if (_eh.getIsLife())
		{
			if (attackTimer > 0f)
			{
				attackTimer -= Time.deltaTime;
			}
			if (attackTimer < 0f)
			{
				attackTimer = 0f;
			}
			if (Target != null && Agression && attackTimer == 0f)
			{
				SendMessage("Attack");
				attackTimer = coolDown;
			}
		}
		else if (!deaded)
		{
			SendMessage("Death");
			_botTrigger.shouldDetectPlayer = false;
			deaded = true;
		}
	}

	public void SetTarget(Transform _tgt, bool agression)
	{
		Agression = agression;
		Target = _tgt;
		_motor.SetTarget(Target, agression);
	}
}
