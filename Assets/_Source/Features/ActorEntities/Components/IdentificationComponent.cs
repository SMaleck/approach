using System.Net.Mime;
using _Source.Debug;
using _Source.Features.Actors.DataComponents;
using UnityEditor;

namespace _Source.Features.ActorEntities.Components
{
    public class IdentificationComponent : AbstractMonoComponent
    {
        private EntityTypeDataComponent _entityTypeDataComponent;

        protected override void OnSetup()
        {
            _entityTypeDataComponent = Actor.Get<EntityTypeDataComponent>();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (UnityEngine.Application.IsPlaying(this))
            {
                OnDrawGizmosPlayMode();
            }
        }

        private void OnDrawGizmosPlayMode()
        {
            Handles.Label(
                transform.position + DebugConstants.IdLabelOffset,
                _entityTypeDataComponent.Id,
                EditorStyles.boldLabel);
        }
#endif
    }
}
