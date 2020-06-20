using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _enemylaserSpeed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemylaserSpeed * Time.deltaTime);
        if (transform.position.y < -22.0f)
        {
            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
