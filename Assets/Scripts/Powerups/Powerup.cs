using System;
using System.Collections;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private bool _pickedUp = false;
    [SerializeField] private float _despawnTime = 10f;

    private void Start()
    {
        StartCoroutine(DespawnObject());
    }

    private IEnumerator DespawnObject()
    {
        yield return new WaitForSeconds(_despawnTime - 2f);

        var t = (_despawnTime - 8f) / 6f;

        for (var i = 0; i < 3; i++)
        {
            if (!_pickedUp)
            {
                GetComponent<Renderer>().enabled = false;
                yield return new WaitForSeconds(t);
            }

            if (!_pickedUp)
            {
                GetComponent<Renderer>().enabled = true;
                yield return new WaitForSeconds(t);
            }
        }

        if (!_pickedUp)
        {
            Destroy(gameObject);
        }

    }

    private void HandlePickup()
    {
        _pickedUp = true;
        PerformPowerAction();
    }

    public virtual void PerformPowerAction()
    {
        Destroy(gameObject);   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePickup();
        }
    }
}