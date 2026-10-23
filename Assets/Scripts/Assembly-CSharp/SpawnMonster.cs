using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
	public bool shouldMove = true;

	public bool isCycle;

	public GameObject[] _targetCyclePoints;

	private int _targetIndex;

	public float _range = 10f;

	private float _minDist = 5f;

	private Vector2 _spawnPoint;

	public float _speed = 1f;

	private float _lastTimeGo = -1f;

	private float _timeForIdle = 2f;

	private Vector3 _targetPoint;

	private Rect _moveBounds;

	private float halbLength = 17f;

	private float _dst;

	public bool ShouldMove
	{
		get
		{
			return shouldMove;
		}
		set
		{
			if (shouldMove != value)
			{
				shouldMove = value;
				if (shouldMove)
				{
					ResetPars();
					SendMessage("Walk");
				}
			}
		}
	}

	private void Start()
	{
		if (!isCycle)
		{
			_spawnPoint = new Vector2(base.transform.position.x, base.transform.position.z);
		}
		ShouldMove = false;
		ShouldMove = true;
		_moveBounds = new Rect(0f - halbLength, 0f - halbLength, halbLength * 2f, halbLength * 2f);
	}

	private void Update()
	{
		if (shouldMove)
		{
			if (_lastTimeGo <= Time.time)
			{
				_targetPoint = new Vector3(0f - halbLength + Random.Range(0f, halbLength * 2f), base.transform.position.y, 0f - halbLength + Random.Range(0f, halbLength * 2f));
				_lastTimeGo = Time.time + Vector3.Distance(base.transform.position, _targetPoint) / _speed + _timeForIdle;
				float num = Vector3.Distance(base.transform.position, _targetPoint);
				base.transform.LookAt(_targetPoint);
			}
			if (_lastTimeGo - _timeForIdle > Time.time)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, _targetPoint, _speed * Time.deltaTime / Vector3.Distance(base.transform.position, _targetPoint));
			}
		}
	}

	private void ResetPars()
	{
		_targetIndex = 0;
		_lastTimeGo = -1f;
	}
}
