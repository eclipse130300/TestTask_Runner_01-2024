using CodeBase.Services;
using EventBusSystem;
using Events;
using UnityEngine;

namespace CodeBase.Chunks
{
    /// <summary>
    /// Moves each level chunk to the end of the level
    /// Reports with event when it's done
    /// </summary>
    public class ChunkController : MonoBehaviour
    {
        public Transform ChunkVisualTransform => _chunkVisualTransform;

        [SerializeField]
        private Transform _chunkVisualTransform;

        private float _chunkUnitySizeZ;

        public void Construct(IStaticDataService staticDataService)
        {
            var levelData = staticDataService.ForGame();
            _chunkUnitySizeZ = levelData.ChunkRows * levelData.LinesSpacingZ;
        }

        private void LateUpdate()
        {
            var endChunkPoint = transform.position + Vector3.forward * _chunkUnitySizeZ;
            
            //as a start point is on Vector3.Zero
            if (endChunkPoint.z <= 0)
                EventBus.RaiseEvent<IChunkReadyForUnloadHandler>(h => h.OnChunkReadyForUnload(endChunkPoint.z));
        }
    }
}
