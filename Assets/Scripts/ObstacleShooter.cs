using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleShooter : MonoBehaviour
{
    public GameObject bullet;
    public float shootingTime = .5f;
    private void OnEnable()
    {
        StartCoroutine(StartShooting());
    }
    private IEnumerator StartShooting()
    {
        StartCoroutine(Rotating());
        while(true)
        {
            Vector2 center = gameObject.transform.position;
            float rotationAngle = gameObject.transform.rotation.eulerAngles.z;
            var result = GetSpriteCorners(gameObject.GetComponent<SpriteRenderer>());
            for (int i = 0; i < 4; i++)
            {
                var tempBullet = Instantiate(bullet, (Vector2)result[i], Quaternion.identity);
                //tempBullet.transform.position += (Vector3)direction;
                var script = tempBullet.GetComponent<Bullet>();
                var direction2 = (Vector3)result[i] - transform.position;
                tempBullet.transform.position += direction2 / 2;
                script.GetDirection(direction2);
            }
            yield return new WaitForSeconds(shootingTime);
        }
    }
    private IEnumerator Rotating()
    {
        while(true)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f,0f, gameObject.transform.rotation.eulerAngles.z + 5f);
            yield return new WaitForSeconds(0.1f);
        }
    }
    public static Vector3[] GetSpriteCorners(SpriteRenderer renderer)
    {
        Vector3 topRight = renderer.transform.TransformPoint(renderer.sprite.bounds.max);
        Vector3 topLeft = renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.max.x, renderer.sprite.bounds.min.y, 0));
        Vector3 botLeft = renderer.transform.TransformPoint(renderer.sprite.bounds.min);
        Vector3 botRight = renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.min.x, renderer.sprite.bounds.max.y, 0));
        return new Vector3[] { topRight, topLeft, botLeft, botRight };
    }
}
