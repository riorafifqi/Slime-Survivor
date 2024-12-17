using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkDetector : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    ChunkManager chunkManager;

    private void Start()
    {
        chunkManager = GetComponentInParent<ChunkManager>();

        StartCoroutine(DetectArea());
    }

    public IEnumerator DetectArea()
    {
        while (true)
        {
            Collider2D chunkCollider = Physics2D.OverlapCircle(this.transform.position, 0.5f, layerMask);
            GameObject chunkGameObject = chunkCollider != null? chunkCollider.gameObject : null;
            
            ChunkManager.OnDetectChunkE?.Invoke(chunkGameObject);

            if (chunkGameObject == null)
            {
                //Debug.Log("Missing chunk is in: " + GetChunkDirection(this.transform.localPosition));
                Vector2 chunkPosition = GetChunkDirection(this.transform.localPosition);

                GameObject newChunk = chunkManager.InstantiateChunk();
                newChunk.transform.localPosition = chunkManager.CurrentChunkPosition() + chunkManager.GetTilemapOffset() * chunkPosition;

                //Debug.Log("Distance: " + Vector2.Distance(newChunk.transform.position, chunkManager.transform.position), newChunk);

                //Debug.Log(chunkManager.CurrentChunkPosition());
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public Vector2 GetChunkDirection(Vector2 position)
    {
        Vector2 result = new(0, 0);

        if (position.x > 0) result.x = 1;
        else if (position.x < 0) result.x = -1;

        if (position.y > 0) result.y = 1;
        else if (position.y < 0) result.y = -1;

        return result;
    }
}
