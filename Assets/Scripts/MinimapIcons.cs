using UnityEngine;

public class MinimapIcons : MonoBehaviour
{
    public Transform target;
    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, transform.position.y);
        transform.rotation = Quaternion.Euler(0f, target.eulerAngles.y, 0);
    }
}
