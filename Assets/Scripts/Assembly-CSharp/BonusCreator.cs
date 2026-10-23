using System.Collections;
using UnityEngine;

public class BonusCreator : MonoBehaviour
{
	public GameObject[] bonusPrefabs;

	public float creationInterval = 10f;

	private ArrayList bonuses = new ArrayList();

	private void Start()
	{
		StartCoroutine(AddBonus());
	}

	private void Update()
	{
	}

	private IEnumerator AddBonus()
	{
		while (true)
		{
			yield return new WaitForSeconds(creationInterval);
			Vector3 pos = new Vector3(0f - Defs.HalfLength + Random.Range(0f, 2f * Defs.HalfLength), 0.24f, 0f - Defs.HalfLength + Random.Range(0f, 2f * Defs.HalfLength));
			int type = Random.Range(0, 11);
			Object newBonus = (GameObject)Object.Instantiate(bonusPrefabs[(type >= 9) ? 1u : 0u], pos, Quaternion.identity);
			bonuses.Add(newBonus);
			if (bonuses.Count > 5)
			{
				Object.Destroy((GameObject)bonuses[0]);
				bonuses.RemoveAt(0);
			}
		}
	}
}
