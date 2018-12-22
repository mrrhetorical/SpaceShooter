using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

	public static Player Singleton;

	[SerializeField] private bool _isForMenu = false;
	[SerializeField] private GameObject _bulletPrefab;
	[SerializeField] private float _shipSpeed = 2f;
	[SerializeField] private float _bulletSpeed = 5f;

	[Header("User Interface")]
	[SerializeField] private List<GameObject> _interfaceHearts = new List<GameObject>();
	[SerializeField] private Slider _heatSlider;
	[SerializeField] private Image _heatFillImage;

	[SerializeField] private int _health;
	[SerializeField] public bool Invincible;
	[SerializeField] public bool TakingDamage;

	public bool GameRunning = true;
	[SerializeField] public int PlayerScore = 0;
	[SerializeField] private Text _scoreText;

	private bool _canFire = true;
	private float _heat = 0f;
	public float FireRate = 0.2f;
	private bool _overHeated = false;
	public bool ApplyHeat = true;

	
	private void Start()
	{
		if (Singleton != null)
		{
			Destroy(this);
			return;
		}

		Singleton = this;

		Time.timeScale = 1f;
		
		if (_isForMenu)
			return;
		
		if (_bulletPrefab == null)
		{
			Debug.LogError("The bullet prefab is not set up!");
		}

		_health = 3;

		StartCoroutine(RunScore());
	}

	private void Update()
	{
		if (_isForMenu)
			return;
		
		if (transform.position.x > 8.8f)
		{
			var pos = transform.position;
			pos.x = -8.8f;
			transform.position = pos;
		} else if (transform.position.x < -8.8f)
		{
			var pos = transform.position;
			pos.x = 8.8f;
			transform.position = pos;
		}

		if (transform.position.y < -4.5f)
		{
			var pos = transform.position;
			pos.y = -4.5f;
			transform.position = pos;
		} else if (transform.position.y > 4.5f)
		{
			var pos = transform.position;
			pos.y = 4.5f;
			transform.position = pos;
		}

		var movement = Vector3.zero;
		movement.x = Input.GetAxis("Horizontal");
		movement.y = Input.GetAxis("Vertical");
		movement *= _shipSpeed;
		movement *= Time.deltaTime;

		transform.Translate(movement);
		
		if ((Input.GetButtonDown("Jump") || Input.GetButton("Jump")) && !_overHeated)
		{
			if (_canFire)
			{
				Shoot();
			}
		}
		else
		{
			if (_canFire)
			{
				if (_heat - (5f * Time.deltaTime) < 0f)
				{
					_heat = 0;
				}
				else
				{
					_heat -= 5f * Time.deltaTime;
				}

				if (_heat < 70f)
				{
					_overHeated = false;
				}
			}
		}

		UpdateHeat();
	}

	private void UpdateHeat()
	{
		var progress = _heat / 100f;
		_heatSlider.value = progress;

		if (_heat > 70f)
		{
			_heatFillImage.color = Color.red;
		} else if (_heat <= 70f && _heat > 50f)
		{
			_heatFillImage.color = new Color(255, 150, 0, 255); //orange
		} else if (_heat <= 50f && _heat > 30f)
		{
			_heatFillImage.color = Color.yellow;
		} else if (_heat <= 30f)
		{
			_heatFillImage.color = Color.grey;
		}
	}

	private void Shoot()
	{
		StartCoroutine(DoShootCooldown());
		if (ApplyHeat)
		{
			_heat += 1.5f;
			if (_heat >= 100f)
			{
				_overHeated = true;
			}
		}

		var bullet = Instantiate(_bulletPrefab);
		bullet.transform.position = transform.position;
		StartCoroutine(ApplyBulletForce(bullet));
		
	}

	private IEnumerator RunScore()
	{
		while (GameRunning)
		{
			yield return new WaitForSeconds(1.0f);
			if (!EnemySpawner.Singleton || !EnemySpawner.Singleton.BossSpawned)
			{
				PlayerScore++;
				_scoreText.text = PlayerScore + "";
			}
			
		}
	}

	private IEnumerator ApplyBulletForce(GameObject bullet)
	{
	
		while (bullet != null && bullet.transform.position.y < 10f)
		{
			bullet.transform.Translate(new Vector3(0, Time.deltaTime * _bulletSpeed, 0));
			yield return null;
		}

		if (bullet != null)
		{
			Destroy(bullet);
		}
	}

	private IEnumerator DoShootCooldown()
	{
		_canFire = false;
		yield return new WaitForSeconds(FireRate);
		_canFire = true;
	}

	public void Damage(int damage)
	{
		if (Invincible || TakingDamage)
			return;
		
		for (var i = _health; i > _health - damage; i--)
		{
			if (i > 0)
			{
				StartCoroutine(UpdateHealthDisplay(i));
			}
		}

		_health -= damage;

		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
			
			

		if (_health < 1)
		{
			GameRunning = false;
			MenuManager.Singleton.EnableGameOverMenu();
		}
	}

	public void AddHeart()
	{
		if (_health <= 3)
		{
			_health++;
			if (_health == 4)
			{
				GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 1, 1);
			}

			StartCoroutine(UpdateHealthDisplay(_health));
		}

	}

	private IEnumerator UpdateHealthDisplay(int heartToDamage)
	{
		TakingDamage = true;
		
		var heart = _interfaceHearts[heartToDamage - 1];
		var heartRenderer = heart.GetComponent<Image>();
		var playerRenderer = GetComponent<SpriteRenderer>();
		for (var i = 0; i < 3; i++)
		{
			heartRenderer.enabled = false;
			playerRenderer.enabled = false;
			yield return new WaitForSeconds(0.2f);
			heartRenderer.enabled = true;
			playerRenderer.enabled = true;
			yield return new WaitForSeconds(0.2f);
		}

		if (heartToDamage != _health)
		{
			heartRenderer.enabled = false;
		}

		TakingDamage = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("EnemyBullet"))
		{
			Destroy(other.gameObject);
			Damage(1);
		}

		if (other.CompareTag("BossBullet"))
		{
			Destroy(other.gameObject);
			Damage(2);
		}

		if (other.CompareTag("BossBomb"))
		{
			Damage(3);
		}
	}

	public Text GetScoreText()
	{
		return _scoreText;
	}

	public int GetHealth()
	{
		return _health;
	}
}
