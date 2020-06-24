using UnityEngine;

public class powerUp : MonoBehaviour
{
    [SerializeField]
    private float _powerUpSpeed = 3.0f;
    private int powerupID;
    [SerializeField]
    private GameObject _audioPrefab;
    [SerializeField]
    private float _rotateSpeed = 100f;
    private bool _summoned = false;
    private Vector2 _playerPos;
    [SerializeField]
    private bool _rotate;
    // Start is called before the first frame update



    // Update is called once per frame
    void Update()
    {
        if (_summoned == false)
        {
            transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.position = Vector3.MoveTowards(this.transform.position, _playerPos, _powerUpSpeed * Time.deltaTime);
        }
        
        if (_rotate == true) this.transform.Rotate(Vector3.forward, _rotateSpeed * Time.deltaTime, Space.Self);

        if (transform.position.y < -22.0f)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetPowerupID(int ID)
    {
        powerupID = ID;
    }

    public void CollectPowerupPressed(Vector2 pos)
    {
        _playerPos = pos;
        _summoned = true;
    }
    public void CollectPowerupNotPressed()
    {
        _playerPos = default;
        _summoned = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        collision.GetComponent<Player>().powerUp_TripleShot();
                        break;
                    case 1:
                        collision.GetComponent<Player>().powerUp_Speed();
                        break;
                    case 2:
                        collision.GetComponent<Player>().powerUp_Shield();
                        break;
                    case 3:
                        collision.GetComponent<Player>().powerUp_Ammo();
                        break;
                    case 4:
                        collision.GetComponent<Player>().powerUp_Health();
                        break;
                    case 5:
                        collision.GetComponent<Player>().powerUp_LightningShot();
                        break;
                    case 6:
                        collision.GetComponent<Player>().Neg_Powerup_Brain();
                        break;
                    case 7:
                        collision.GetComponent<Player>().powerUp_HomingShot();
                        break;
                    default:
                        Debug.Log("no powerup");
                        break;
                }
            }
            GameObject clone = Instantiate(_audioPrefab, transform.position, transform.rotation);
            AudioSource cloneAudio = clone.GetComponent<AudioSource>();
            Destroy(clone, cloneAudio.clip.length + 0.1f);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("EnemyLaser"))
        {
            Destroy(this.gameObject);
        }
    }
}
