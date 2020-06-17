using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
    [SerializeField]
    private float _powerUpSpeed = 3.0f;
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private GameObject _audioPrefab;
    // Start is called before the first frame update



    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);

        if (transform.position.y < -5.5f)
        {
            Destroy(this.gameObject);
        }
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
