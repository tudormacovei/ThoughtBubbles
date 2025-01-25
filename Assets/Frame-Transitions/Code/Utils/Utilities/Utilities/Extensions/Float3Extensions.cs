using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace tdk.Utilities
{
    public static class Float3Extensions
    {
        /// <summary>
        /// Sets any x y z values of a Float3
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 With(this float3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new float3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }

        /// <summary>
        /// Adds to any x y z values of a Float3
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 Add(this float3 vector, float x = 0, float y = 0, float z = 0)
        {
            return new float3(vector.x + x, vector.y + y, vector.z + z);
        }

        /// <summary>
        /// Divides two Float3 objects component-wise.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 ComponentDivide(this float3 v0, float3 v1)
        {
            return new float3(
                v1.x != 0 ? v0.x / v1.x : v0.x,
                v1.y != 0 ? v0.y / v1.y : v0.y,
                v1.z != 0 ? v0.z / v1.z : v0.z);
        }

        /// <summary>
        /// Multiply two Float3 objects component-wise.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 Scale(this float3 a, float3 b)
        {
            return new float3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        /// <summary>
        /// Moves towards Float3 set distance over time.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 MoveTowards(float3 current, float3 target, float maxDistanceDelta)
        {
            float deltaX = target.x - current.x;
            float deltaY = target.y - current.y;
            float deltaZ = target.z - current.z;

            float sqdist = deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ;

            if (sqdist == 0 || sqdist <= maxDistanceDelta * maxDistanceDelta)
                return target;
            var dist = (float)math.sqrt(sqdist);

            return new float3(current.x + deltaX / dist * maxDistanceDelta,
                current.y + deltaY / dist * maxDistanceDelta,
                current.z + deltaZ / dist * maxDistanceDelta);
        }
    }
}
