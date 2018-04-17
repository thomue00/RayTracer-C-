using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {
    public class MathUtil {

        public static float PI => 3.1415926f;
        private static uint s_RndState = 1;

        //-----------------------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint XorShift32() {
            uint x = s_RndState;
            x ^= x << 13;
            x ^= x >> 17;
            x ^= x << 15;
            s_RndState = x;
            return x;
        }

        //-----------------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RandomFloat01() {
            return (XorShift32() & 0xFFFFFF) / 16777216.0f;
        }

        //-----------------------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 RandomInUnitDisk() {

            Vec3 p;
            do {
                p = 2.0f * new Vec3(RandomFloat01(), RandomFloat01(), 0) - new Vec3(1, 1, 0);
            } while (p.sqrMagnitude >= 1.0);
            return p;
        }

        //-----------------------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vec3 RandomInUnitSphere() {

            Vec3 p;
            do {
                p = 2.0f * new Vec3(RandomFloat01(), RandomFloat01(), RandomFloat01()) - new Vec3(1, 1, 1);
            } while (p.sqrMagnitude >= 1.0);
            return p;
        }
    }
}
