using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Settings")]
    [SerializeField] private Vector2 _horizontalSpeed;
    [SerializeField] private Vector2 _verticalSpeed;

    [SerializeField] private Vector2 _speed;
    
    protected int _direction = 1;
    [SerializeField] protected int _health = 2;
    private bool _damageImmune = false;

    [SerializeField] private float _minTimeBetweenChanges = 0.4f;
    
    protected bool _moveDown = true;

    private int lastMultiplier = 0;
    
    protected SpriteRenderer _spriteRenderer;

    private float _lastChange = 0f;
    
    public virtual void Start()
    {
        var hSpeed = Random.Range(_horizontalSpeed.x, _horizontalSpeed.y);
        var vSpeed = Random.Range(_verticalSpeed.x, _verticalSpeed.y);

        _speed.x = hSpeed;
        _speed.y = vSpeed;
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Update()
    {

        if (8.5f > transform.position.x && -8.5f < transform.position.x)
        {
            var move = Vector3.zero;
            move.x = _speed.x;
            move.x *= _direction;

            var downMultiplier = -1;

            if (!_moveDown && _minTimeBetweenChanges >= _lastChange)
            {
                var rand = Random.Range(0f, 1.0f);
                if (rand < 0.7f)
                {
                    downMultiplier = 1;
                }

                if (lastMultiplier != downMultiplier)
                {
                    _lastChange = 0f;
                }
            }

            move.y = _speed.y * downMultiplier;
            move *= Time.deltaTime;
            transform.Translate(move);

            if (transform.position.y > 4.23f && IsEndless())
            {
                var pos = transform.position;
                pos.y = 4.23f;
                transform.position = pos;
            }
        }
        else
        {
            _direction *= -1;
            var pos = transform.position;
            if (transform.position.x > 8.5f)
            {
                pos.x = 8.45f;
            } else if (transform.position.x < -8.5f)
            {
                pos.x = -8.45f;
            }

            transform.position = pos;
        }

        _lastChange += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (!_damageImmune)
            {
                Destroy(other.gameObject);
                StartCoroutine(TakeDamage());
            }
        }

        if (other.CompareTag("Player") || other.CompareTag("EndZone"))
        {
            Player.Singleton.Damage(1);
            if (other.CompareTag("EndZone"))
            {
                Destroy(gameObject);
            }
            else
            {
                if (!Player.Singleton.TakingDamage && Player.Singleton.Invincible)
                {
                    StartCoroutine(TakeDamage());
                }
            }
        }
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    private IEnumerator TakeDamage()
    {

        _health--;
        if (_health < 1)
        {
            Kill();
            yield break;
        }

        _damageImmune = true;
        
        for (var i = 0; i < 3; i++)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        
        _damageImmune = false;
    }

    private static bool IsEndless()
    {
        return SceneManager.GetActiveScene().name.ToUpper() == "ENDLESS";
    }
}