using System.Collections;
using UnityEngine;

public class ZombieCreator : MonoBehaviour
{
	public Transform[] zombiePrefabs;

	private int _numOfLiveZombies;

	private int _numOfDeadZombies;

	private int _numOfDeadZombsSinceLastFast;

	public float curInterval = 10f;

	public int NumOfLiveZombies
	{
		get
		{
			return _numOfLiveZombies;
		}
		set
		{
			_numOfLiveZombies = value;
		}
	}

	public int NumOfDeadZombies
	{
		get
		{
			return _numOfDeadZombies;
		}
		set
		{
			int num = value - _numOfDeadZombies;
			_numOfDeadZombies = value;
			NumOfLiveZombies -= num;
			_numOfDeadZombsSinceLastFast += num;
			if (_numOfDeadZombsSinceLastFast == 12)
			{
				if (curInterval > 5f)
				{
					curInterval -= 5f;
				}
				_numOfDeadZombsSinceLastFast = 0;
			}
		}
	}

	private void Start()
	{
		StartCoroutine(AddZombies());
	}

	private void Update()
	{
	}

	private IEnumerator AddZombies()
	{
		int numOfZombsToAdd = 4;
		int maxNumOfZombs = 50;
		float halfLLength = 17f;
		float radius = 2.5f;
		float[] ys = new float[2] { 1f, 0.5f };
		while (true)
		{
			ArrayList points = new ArrayList
			{
				new Vector3(0f - halfLLength + Random.Range(0f, radius), 1f, 0f - radius + Random.Range(0f, 2f * radius)),
				new Vector3(0f - radius + Random.Range(0f, 2f * radius), 1f, halfLLength - Random.Range(0f, radius)),
				new Vector3(halfLLength - Random.Range(0f, radius), 1f, 0f - radius + Random.Range(0f, 2f * radius)),
				new Vector3(0f - radius + Random.Range(0f, 2f * radius), 1f, 0f - halfLLength + Random.Range(0f, radius))
			};
			if (NumOfLiveZombies < maxNumOfZombs - numOfZombsToAdd)
			{
				for (int i = 0; i < numOfZombsToAdd; i++)
				{
					int typeOfZomb = Random.Range(0, 2);
					float y = ys[typeOfZomb];
					Object.Instantiate(zombiePrefabs[typeOfZomb], new Vector3(((Vector3)points[i])[0], y, ((Vector3)points[i])[2]), Quaternion.identity);
				}
			}
			yield return new WaitForSeconds(curInterval);
		}
	}
}
