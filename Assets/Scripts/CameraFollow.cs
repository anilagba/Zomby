using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float moveSpeed;
    [SerializeField] float offsetX;
    [SerializeField] float offsetZ;

    Vector2 velocity = Vector2.zero;


    private void LateUpdate()
    {
        Vector2 playerPos = new Vector2(player.position.x + offsetX, player.position.z + offsetZ);
        Vector2 cameraPos = new Vector2(transform.position.x, transform.position.z);

        Vector2 pos = Vector2.SmoothDamp(cameraPos, playerPos, ref velocity, moveSpeed);

        Vector3 posToV3 = new Vector3(pos.x, transform.position.y, pos.y);

        transform.position = posToV3;
    }
}
