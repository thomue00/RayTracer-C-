using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    public static class Utils {

        public static void Swap<T>(ref T lhs, ref T rhs) {

            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        //-----------------------------------------------------------------------------

        public static float Schlick(float cosine, float ref_idx) {

            float r0 = (1 - ref_idx) / (1 + ref_idx);
            r0 = r0 * r0;
            return r0 + (1 - r0) + (float)Math.Pow((1 - cosine), 5);
        }

        //-----------------------------------------------------------------------------

        public static bool Refract(Vec3 v, Vec3 n, float nint, out Vec3 refracted) {

            Vec3 uv = v;
            Vec3 un = n;
            float dt = Vec3.Dot(uv, un);
            float discr = 1.0F - nint * nint * (1.0F - dt * dt);
            if (discr > 0) {

                refracted = nint * (uv - un * dt) - un * (float)Math.Sqrt(discr);
                return true;
            }
            else {

                refracted = Vec3.Zero;
                return false;
            }
        }

        //-----------------------------------------------------------------------------

        public static Vec3 Reflect(Vec3 v, Vec3 n) {
            return v - 2 * Vec3.Dot(v, n) * n;
        }


        //-----------------------------------------------------------------------------

        public static void ArraySplit<T>(T[] array, int index, out T[] first, out T[] second) {

            first = array.Take(index).ToArray();
            second = array.Skip(index).ToArray();
        }

    }
}
