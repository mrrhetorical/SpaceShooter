using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShootingEnemy : Enemy
{

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Vector2 _shootDelay;
    [SerializeField] private float _bulletSpeed = 2f;
    [SerializeField] private float _spinChance = 5f;
    private bool _alive = true;

    [SerializeField] private int _bulletCount = 0;
    
    public override void Start()
    {
        base.Start();
        StartCoroutine(ShootBullets());
        _spriteRenderer.color = Color.green;
    }

    public override void Update()
    {
        var chance = Random.Range(0, 100);
        if (_spinChance >= chance)
        {
            _direction *= -1;
        }

        base.Update();
    }

    private IEnumerator ShootBullets()
    {
        var first = Random.Range(_shootDelay.x, _shootDelay.y);
        yield return new WaitForSeconds(first);
        while (_alive)
        {
            var delay = Random.Range(_shootDelay.x, _shootDelay.y);
            var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity, null);
            StartCoroutine(ApplyBulletForce(bullet));
            _bulletCount++;
            yield return new WaitForSeconds(delay);
        }
    }
    
    private IEnumerator ApplyBulletForce(GameObject bullet)
    {
	
        while (bullet != null && bullet.transform.position.y > -8f)
        {
            bullet.transform.Translate(new Vector3(0, Time.deltaTime * _bulletSpeed * -1, 0));
            yield return null;
        }

        if (bullet != null)
        {
            Destroy(bullet);
        }


        _bulletCount--;
    }

    public virtual void BaseKill()
    {
        _alive = false;
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(KillLater());
    }

    public override void Kill()
    {
        BaseKill();
    }

    private IEnumerator KillLater()
    {
        while (_bulletCount > 0)
        {
            yield return new WaitForSeconds(1.0f);
        }
        
        Destroy(this);
    }
}