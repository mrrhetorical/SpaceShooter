using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossEnemy : ShootingEnemy
{

    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private GameObject _bossBulletPrefab;
    [SerializeField] private float _bombSpeed = 2f;
    
    public override void Start()
    {
        base.Start();
        _moveDown = false;
        _spriteRenderer.color = Color.cyan;
    }

    public override void Update()
    {
        var movement = Vector3.zero;
        movement.x = _speed.x;
        movement.x *= Time.deltaTime;
        
        if (transform.position.x > Player.Singleton.transform.position.x)
        {
            movement.x *= -1f;
        } else if (Mathf.Abs(transform.position.x - Player.Singleton.transform.position.x) <= 0.1)
        {
            movement.x = 0;
        }

        transform.Translate(movement);

//        base.Update();
    }

    public override void BaseKill()
    {

        if (EnemySpawner.Singleton)
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            var found = false;

            foreach (var enemy in enemies)
            {
                if (enemy == gameObject)
                    continue;

                if (enemy.GetComponent<BossEnemy>())
                {
                    found = true;
                    break;
                }
            }


            EnemySpawner.Singleton.BossSpawned = found;
        }

        base.BaseKill();
    }
    
    protected override IEnumerator ShootBullets()
    {
        var first = Random.Range(_shootDelay.x, _shootDelay.y);
        yield return new WaitForSeconds(first);
        while (_alive)
        {
            var distance = Mathf.Abs(transform.position.x - Player.Singleton.transform.position.x);
            var delay = Random.Range(_shootDelay.x, _shootDelay.y); 
            
            if (distance <= 7.5f)
            {
                var rand = Mathf.RoundToInt(Random.Range(0f, 2f));

                if (rand == 0)
                {
                    var bomb = Instantiate(_bombPrefab, transform.position, Quaternion.identity, null);
                    StartCoroutine(ApplyBombForce(bomb));
                }
                else if (rand == 1)
                {
                    var bullet = Instantiate(_bossBulletPrefab, transform.position, Quaternion.identity, null);
                    StartCoroutine(ApplyBulletForce(bullet, _bulletSpeed));
                    _bulletCount++;
                }
                else if (rand == 2)
                {
                    _bulletCount += 3;
                    var b1 = Instantiate(_bulletPrefab, transform.position, Quaternion.identity, null);
                    var b2 = Instantiate(b1);
                    var b3 = Instantiate(b2);

                    b2.transform.rotation = Quaternion.Euler(0, 0, 45);
                    b3.transform.rotation = Quaternion.Euler(0, 0, -45);

                    StartCoroutine(ApplyBulletForce(b1, _bulletSpeed * 1.5f));
                    StartCoroutine(ApplyBulletForce(b2, _bulletSpeed * 1.5f));
                    StartCoroutine(ApplyBulletForce(b3, _bulletSpeed * 1.5f));
                }
            }

            yield return new WaitForSeconds(delay);
        }
    }

    [Obsolete("No need for this method with ApplyBulletForce(bullet) existing.")]
    private IEnumerator ApplyTriShotForce(GameObject b1, GameObject b2, GameObject b3)
    {
        _bulletCount += 3;
        while ((b1 != null && b1.transform.position.y > -6f) || (b2 != null && b2.transform.position.y > -6f) || (b3 != null && b3.transform.position.y > -6f))
        {
            var move = Vector3.zero;
            move.y = _bulletSpeed * -1.5f;
            move *= Time.deltaTime;

            if (b1 != null)
            {
                b1.transform.Translate(move);
            }

            if (b2 != null)
            {
                b2.transform.Translate(move);
            }

            if (b3 != null)
            {
                b3.transform.Translate(move);
            }

            yield return null;
        }

        if (b1 != null)
        {
            Destroy(b1);
        }

        if (b2 != null)
        {
            Destroy(b2);
        }

        if (b3 != null)
        {
            Destroy(b3);
        }

        _bulletCount -= 3;
    }

    private IEnumerator ApplyBombForce(GameObject bomb)
    {
        _bulletCount++;
        var pos = bomb.transform.position;

        var traveled = 0f;

        var distance = Random.Range(2.5f, 6f);

        while (traveled < distance)
        {
            var move = Vector3.zero;
            move.y = _bombSpeed * -1f;
            move *= Time.deltaTime;
            bomb.transform.Translate(move);
            traveled += Vector3.Distance(bomb.transform.position, pos);
            pos = bomb.transform.position;
            yield return null;
        }

        bomb.GetComponent<SpriteRenderer>().color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        var scale = bomb.transform.localScale;
        scale *= 6;
        bomb.transform.localScale = scale;
        bomb.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0, 1);
        yield return new WaitForSeconds(0.2f);
        scale /= 3;
        bomb.transform.localScale = scale;
        bomb.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(0.1f);
        Destroy(bomb);
        _bulletCount--;
    }

}