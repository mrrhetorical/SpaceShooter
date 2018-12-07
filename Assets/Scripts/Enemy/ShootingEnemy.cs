using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShootingEnemy : Enemy
{

    [SerializeField] protected GameObject _bulletPrefab;
    [SerializeField] protected Vector2 _shootDelay;
    [SerializeField] protected float _bulletSpeed = 2f;
    [SerializeField] private float _spinChance = 5f;
    protected bool _alive = true;

    [SerializeField] protected int _bulletCount = 0;
    
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

    protected virtual IEnumerator ShootBullets()
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
    
    protected IEnumerator ApplyBulletForce(GameObject bullet)
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