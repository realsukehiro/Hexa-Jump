using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [HideInInspector] public GameObject platform;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            GameManager gm = FindObjectOfType<GameManager>();

            if (platform.CompareTag("Deadly"))
            {
                Debug.Log("Ball hit deadly wedge!");
                gm.OnDeadlyHit();
            }
            else
            {
                Debug.Log("Ball crossed!");
                gm.OnPlatformCrossed(platform);
            }
        }
    }
}
