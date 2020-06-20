using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update



    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime, Space.World);
        if (powerupID == 3) this.transform.Rotate(Vector3.forward, _rotateSpeed * Time.deltaTime, Space.Self);

        if (transform.position.y < -22.0f)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetPowerupID(int ID)
    {
        powerupID = ID;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                switch(powerupID)
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
    }
}
