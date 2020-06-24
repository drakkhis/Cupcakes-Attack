using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 10.0f;
    [SerializeField]
    private bool _homing = false;
    GameObject _lockedOn;
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = default;
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime, Space.Self);
    }
    // Update is called once per frame
    void Update()
    {
        if (_homing == false)
        {
            transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            
            if (_lockedOn == null)
            {
                transform.rotation = default;
                transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime, Space.Self);
                _lockedOn = FindClosestEnemy();
            }
            else
            {
                Vector3 target = new Vector3(_lockedOn.transform.position.x, _lockedOn.transform.position.y, 0);
                Vector3 myPosition = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                RotateTowardsTarget();
                transform.position = Vector3.MoveTowards(myPosition, target, _laserSpeed * Time.deltaTime);
            }

        }


        if (transform.position.y > 10.0f)
        {
            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void RotateTowardsTarget()
    {
        float rotationSpeed = 10f;
        float offset = -90f;
        Vector3 direction = _lockedOn.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] _enemies;
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject _enemy in _enemies)
        {
            Vector3 diff = _enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = _enemy;
                distance = curDistance;
            }
        }
        return closest;
    }
}
