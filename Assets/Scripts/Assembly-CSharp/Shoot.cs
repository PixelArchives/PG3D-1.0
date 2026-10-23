using UnityEngine;

public class Shoot : MonoBehaviour
{
	public float Range = 1000f;

	public Transform _transform;

	public GameObject bullet;

	private GameObject _bulletSpawnPoint;

	private void Start()
	{
		_bulletSpawnPoint = GameObject.Find("BulletSpawnPoint");
	}

	public void shootS()
	{
		GameObject gameObject = (GameObject)Object.Instantiate(bullet, _bulletSpawnPoint.transform.position, Quaternion.LookRotation(_transform.TransformDirection(Vector3.forward)));
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 100f, -5) && (hitInfo.collider.gameObject.CompareTag("Enemy") || hitInfo.collider.gameObject.CompareTag("ZombPolz")))
		{
			BotHealth component = hitInfo.collider.gameObject.GetComponent<BotHealth>();
			component.adjustHealth(-50, _transform);
		}
	}
}
