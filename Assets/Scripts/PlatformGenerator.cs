using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject normalWedgePrefab;
    public GameObject deadlyWedgePrefab;

    [Range(3, 12)] public int wedgeCount = 6; 
    public float yStep = -1f; 
    public int totalLevels = 10; 

    [HideInInspector] public List<GameObject> platforms = new List<GameObject>();


    public void GeneratePlatforms()
    {
        platforms.Clear();

        for (int level = 0; level < totalLevels; level++)
        {
            GameObject platform = SpawnPlatform(level);
            platforms.Add(platform);
        }
    }

    private GameObject SpawnPlatform(int level)
    {
        float y = level * yStep;

        GameObject platformParent = new GameObject("Platform_" + level);
        platformParent.transform.SetParent(transform);
        platformParent.transform.localPosition = new Vector3(0, y, 0);
        platformParent.transform.localRotation = Quaternion.identity;


        BoxCollider scoreTrigger = platformParent.AddComponent<BoxCollider>();
        scoreTrigger.isTrigger = true;
        scoreTrigger.size = new Vector3(5f, 0.01f, 5f);
        scoreTrigger.center = Vector3.zero;
        platformParent.tag = "Platform";

        int gapIndex = Random.Range(0, wedgeCount);
        int deadlyIndex = (gapIndex + Random.Range(1, wedgeCount)) % wedgeCount;

        for (int i = 0; i < wedgeCount; i++)
        {
            if (i == gapIndex) continue;
            else if (i == deadlyIndex)
                SpawnWedge(deadlyWedgePrefab, platformParent.transform, i);
            else
                SpawnWedge(normalWedgePrefab, platformParent.transform, i);
        }

        PlatformTrigger trigger = platformParent.AddComponent<PlatformTrigger>();
        trigger.platform = platformParent;

        return platformParent;
    }

    private void SpawnWedge(GameObject prefab, Transform parent, int index)
    {
        GameObject wedge = Instantiate(prefab, parent);
        wedge.transform.localPosition = Vector3.zero;
        wedge.transform.localRotation = Quaternion.Euler(0, index * (360f / wedgeCount), 0);
    }

    public float GetCylinderHeight()
    {
        return totalLevels * Mathf.Abs(yStep);
    }
}
