
using UnityEngine;
using UnityEngine.Video;

public class BugSpawner : MonoBehaviour
{
    public GameObject bugPrefab;
    public bool allowBugVideos = true;

    [Header("Player")]
    public GameObject player;

    [Header("Optional Bug Video References")]
    public GameObject bugVideoCanvas;
    public VideoPlayer bugVideoPlayer;

    [Header("Spawn Area")]
    public Transform spawnCenter;
    public float spawnWidth = 4f;
    public float spawnLength = 6f;

    [Header("Amount")]
    public int minBugs = 6;
    public int maxBugs = 10;

    [Header("Size")]
    public float minScale = 0.07f;
    public float maxScale = 0.13f;

    void Start()
    {
        if (bugPrefab == null)
        {
            Debug.LogError("BugSpawner: Bug Prefab is not assigned!", this);
            return;
        }

        if (spawnCenter == null)
        {
            Debug.LogError("BugSpawner: Spawn Center is not assigned!", this);
            return;
        }

        if (allowBugVideos &&
            (bugVideoCanvas == null || bugVideoPlayer == null))
        {
            Debug.LogWarning(
                "Bug videos are enabled, but a video reference is missing.",
                this
            );
        }

        SpawnBugs();
    }

    void SpawnBugs()
    {
        int bugCount = Random.Range(minBugs, maxBugs + 1);

        for (int i = 0; i < bugCount; i++)
        {
            float randomX = Random.Range(
                -spawnWidth / 2f,
                spawnWidth / 2f
            );

            float randomZ = Random.Range(
                -spawnLength / 2f,
                spawnLength / 2f
            );

            Vector3 spawnPosition = new Vector3(
                spawnCenter.position.x + randomX,
                spawnCenter.position.y,
                spawnCenter.position.z + randomZ
            );

            GameObject bug = Instantiate(
                bugPrefab,
                spawnPosition,
                Quaternion.identity
            );

            float randomScale = Random.Range(minScale, maxScale);
            bug.transform.localScale = Vector3.one * randomScale;

            BugInteraction interaction =
                bug.GetComponent<BugInteraction>();

            if (interaction != null)
            {
                interaction.player = player;
                interaction.allowVideoInteraction = allowBugVideos;

                if (allowBugVideos)
                {
                    interaction.bugVideoCanvas = bugVideoCanvas;
                    interaction.bugVideoPlayer = bugVideoPlayer;
                }
                else
                {
                    interaction.bugVideoCanvas = null;
                    interaction.bugVideoPlayer = null;
                }
            }
        }
    }
}
