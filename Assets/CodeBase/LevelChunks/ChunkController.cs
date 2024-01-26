using EventBusSystem;
using Events;
using UnityEngine;

namespace CodeBase.Chunks
{
    public class ChunkController : MonoBehaviour, IGameLoopStartedHandler, IGameLoopFinishedHandler
    {
        public Transform ChunkVisualTransform => _chunkVisualTransform;

        [SerializeField]
        private Transform _chunkVisualTransform;

        private IStaticDataService _staticDataService;

        private float chunkUnitySizeZ;
        private bool _canMove;

        public void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;

            var levelData = staticDataService.ForLevel();
            chunkUnitySizeZ = levelData.ChunkRows * levelData.LinesSpacingZ;
        }

        private void Awake() => 
            EventBus.Subscribe(this);

        private void OnDestroy() => 
            EventBus.Unsubscribe(this);

        public void OnGameLoopStated() => 
            _canMove = true;

        public void OnGameLoopFinished() => 
            _canMove = false;

        private void Update()
        {
            if (_canMove)
            {
                var speed = _staticDataService.ForLevel().ScrollSpeed;
                transform.position += Vector3.back * speed * Time.deltaTime;
            }
        }

        private void LateUpdate()
        {
            var endChunkPoint = transform.position + Vector3.forward * chunkUnitySizeZ;
            
            //as start point on Vector3.Zero
            if (endChunkPoint.z <= 0)
                EventBus.RaiseEvent<IChunkReadyForUnloadHandler>(h => h.OnChunkReadyForUnload(endChunkPoint.z));
        }
    }
}
