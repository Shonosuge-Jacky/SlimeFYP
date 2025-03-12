using Unity.Entities;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    public class Baker : Baker<SpawnerAuthoring> {
        public override void Bake(SpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SpawnerConfig{
                SlimePrefab = GetEntity(GameDataCenter._SlimePrefabECS, TransformUsageFlags.Dynamic),
                EmptyPrefab = GetEntity(GameDataCenter._EmptyPrefabECS, TransformUsageFlags.None),
                Amount = GameDataCenter._SlimeAmount,
                MinX = GameDataCenter._FloorSetting.MinX,
                MinY = GameDataCenter._FloorSetting.MinY,
                MaxX = GameDataCenter._FloorSetting.MaxX,
                MaxY = GameDataCenter._FloorSetting.MaxY,
            });
        }
    };
}


public struct SpawnerConfig : IComponentData{
    public Entity SlimePrefab;
    public Entity EmptyPrefab;
    public int Amount;
    public int MinX;
    public int MinY;
    public int MaxX;
    public int MaxY;
}
