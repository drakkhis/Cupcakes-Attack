using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossAttack : MonoBehaviour
{
    private Player _player;
    Vector3 _playerPos;
    [SerializeField]
    private float _attackSpeed = 10;
    [SerializeField]
    private float _rotateSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _playerPos = _player.transform.position;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

    }
        // Update is called once per frame
        void Update()
    {
        if (_playerPos != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, _playerPos, _attackSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _attackSpeed * Time.deltaTime);
        }

        if (transform.position == _playerPos) _playerPos = Vector3.zero;

        this.transform.Rotate(Vector3.forward, _rotateSpeed * Time.deltaTime, Space.Self);
        if (transform.position.y < -22.0f)
        {
            Destroy(this.gameObject);
        }
    }

    public void CollectPowerupPressed(Vector2 pos)
    {
        _playerPos = pos;
    }
}
