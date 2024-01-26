﻿using EventBusSystem;
using Events;
using UnityEngine;

namespace CodeBase.Chunks
{
    public class ChunkController : MonoBehaviour
    {
        public Transform ChunkVisualTransform => _chunkVisualTransform;

        [SerializeField]
        private Transform _chunkVisualTransform;

        private float chunkUnitySizeZ;

        public void Construct(IStaticDataService staticDataService)
        {
            var levelData = staticDataService.ForLevel();
            chunkUnitySizeZ = levelData.ChunkRows * levelData.LinesSpacingZ;
        }

        private void LateUpdate()
        {
            var endChunkPoint = transform.position + Vector3.forward * chunkUnitySizeZ;
            
            //as a start point is on Vector3.Zero
            if (endChunkPoint.z <= 0)
                EventBus.RaiseEvent<IChunkReadyForUnloadHandler>(h => h.OnChunkReadyForUnload(endChunkPoint.z));
        }
    }
}
