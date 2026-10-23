using UnityEngine;

public class BotTrigger : MonoBehaviour
{
	public float detectRadius = 17f;

	public Transform _parentBot;

	public bool shouldDetectPlayer = true;

	private bool _entered;

	private BotAI _eai;

	private GameObject _player;

	private void Start()
	{
		_eai = _parentBot.GetComponent<BotAI>();
		_player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (shouldDetectPlayer)
		{
			if (!_entered && Vector3.Distance(base.transform.position, _player.transform.position) <= detectRadius)
			{
				_eai.SetTarget(_player.transform, true);
				_entered = true;
			}
			else if (_entered && Vector3.Distance(base.transform.position, _player.transform.position) > detectRadius)
			{
				_eai.SetTarget(null, false);
				_entered = false;
			}
		}
	}
}
