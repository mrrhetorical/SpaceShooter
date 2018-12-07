using System;
using System.Collections;
using System.Security.Policy;
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

    public override void BaseKill()
    {
        EnemySpawner.Singleton.BossSpawned = false;
        base.BaseKill();
    }
    
    protected override IEnumerator ShootBullets()
    {
        var first = Random.Range(_shootDelay.x, _shootDelay.y);
        yield return new WaitForSeconds(first);
        while (_alive)
        {
            var delay = Random.Range(_shootDelay.x, _shootDelay.y);

            var rand = Mathf.RoundToInt(Random.Range(0f, 2f));
            
            if (rand == 0)
            {
                var bomb = Instantiate(_bombPrefab, transform.position, Quaternion.identity, null);
                StartCoroutine(ApplyBombForce(bomb));
            } else if (rand == 1)
            {
                var bullet = Instantiate(_bossBulletPrefab, transform.position, Quaternion.identity, null);
                StartCoroutine(ApplyBulletForce(bullet));

            } else if (rand == 2)
            {
                var b1 = Instantiate(_bulletPrefab, transform.position, Quaternion.identity, null);
                var b2 = Instantiate(b1);
                var b3 = Instantiate(b2);

                b2.transform.rotation = Quaternion.Euler(0, 0, 45);
                b3.transform.rotation = Quaternion.Euler(0, 0, -45);

                StartCoroutine(ApplyTriShotForce(b1, b2, b3));
            }

            _bulletCount++;
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator ApplyTriShotForce(GameObject b1, GameObject b2, GameObject b3)
    {
        while ((b1 != null && b1.transform.position.y > -6f) || (b2 != null && b2.transform.position.y > -6f) || (b3 != null && b3.transform.position.y > -6f))
        {
            var move = Vector3.zero;
            move.y = _bulletSpeed * -1;
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
    }

    private IEnumerator ApplyBombForce(GameObject bomb)
    {
        var pos = bomb.transform.position;

        var traveled = 0f;

        while (traveled < 4f)
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