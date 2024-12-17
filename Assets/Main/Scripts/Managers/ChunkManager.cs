using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] Transform mapGrid;
    [SerializeField] Vector2 tileOffset;

    Transform currentChunkPosition;
    List<GameObject> activeChunks;

    [SerializeField] LayerMask chunkMask;

    public static Action<GameObject> OnDetectChunkE;

    private void Awake()
    {
        activeChunks = new List<GameObject>();
    }

    private void OnEnable()
    {
        OnDetectChunkE += OnDetectChunkHandler;
    }

    private void OnDisable()
    {
        OnDetectChunkE -= OnDetectChunkHandler;
    }

    public void OnDetectChunkHandler(GameObject chunk = null)
    {
        if (chunk != null && !activeChunks.Contains(chunk))
        {
            activeChunks.Add(chunk);
        }
    }

    public GameObject InstantiateChunk()
    {
        GameObject _chunk = Instantiate(chunkPrefab, mapGrid);

        //CheckUnusedChunks();

        return _chunk;
    }

    public Vector2 CurrentChunkPosition()
    {
        Collider2D chunkCollider = Physics2D.OverlapCircle(this.transform.position, 0.1f, chunkMask);

        return chunkCollider.transform.position;
    }

    public Vector2 GetTilemapOffset()
    {
        return tileOffset;
    }

    public void CheckUnusedChunks()
    {
        foreach (GameObject chunk in activeChunks)
        {
            if (Vector2.Distance(this.transform.position, chunk.transform.position) >= 16.5f)
            {
                activeChunks.Remove(chunk);
                Destroy(chunk);
            }
        }
    }
}
