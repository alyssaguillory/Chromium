using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPropSpawn : MonoBehaviour
{
    //layer to last spawned object position dictionary
    static readonly Dictionary<int, Vector2> lastPositions = new Dictionary<int, Vector2>();

    public int layer = 0;
    public float minSpawnDistance = 2.0f;
    public float spawnProbability = 0.5f;

    void Start()
    {
        bool activated = false;
        var position = transform.position;

        if ((lastPositions.TryGetValue(layer, out var lastPos) &&
             (lastPos - (Vector2) position).magnitude > minSpawnDistance) || !lastPositions.ContainsKey(layer))
        {
            float random = Random.Range(0.0f, 1.0f);

            if (random < spawnProbability)
            {
                Debug.Log((lastPos - (Vector2) position).magnitude);

                lastPositions[layer] = position;
                gameObject.SetActive(true);
                activated = true;
            }
        }

        if (!activated)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}