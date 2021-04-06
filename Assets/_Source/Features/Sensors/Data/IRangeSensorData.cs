using _Source.Entities.Novatar;

namespace _Source.Features.Sensors.Data
{
    public interface IRangeSensorData
    {
        float GetVisualRangeSize(EntityState state);
    }
}