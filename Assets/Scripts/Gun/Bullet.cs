using UnityEngine;
using UnityEngine.SceneManagement;



public class Bullet : MonoBehaviour
{
     private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
