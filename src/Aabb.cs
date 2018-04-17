using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    public sealed class Aabb {

        //-----------------------------------------------------------------------------
        // Member
        //-----------------------------------------------------------------------------

        private Vec3 min, max;

        //-----------------------------------------------------------------------------
        // Properties
        //-----------------------------------------------------------------------------

        public Vec3 Min { get => min; set => min = value; }
        public Vec3 Max { get => max; set => max = value; }

        //-----------------------------------------------------------------------------
        // Functions
        //-----------------------------------------------------------------------------

        public static Aabb SurroundingBox(Aabb box0, Aabb box1) {

            Vec3 small = new Vec3(
                Math.Min(box0.Min.x, box1.Min.x),
                Math.Min(box0.Min.y, box1.Min.y),
                Math.Min(box0.Min.z, box1.Min.z));
            Vec3 big = new Vec3(
                Math.Max(box0.Max.x, box1.Max.x),
                Math.Max(box0.Max.y, box1.Max.y),
                Math.Max(box0.Max.z, box1.Max.z));

            return new Aabb(small, big);
        }

        //-----------------------------------------------------------------------------
        // Methods
        //-----------------------------------------------------------------------------

        public Aabb() { }

        //-----------------------------------------------------------------------------

        public Aabb(Vec3 a, Vec3 b) {

            this.min = a;
            this.Max = b;
        }

        //-----------------------------------------------------------------------------

        public bool Hit(Ray ray, float tMin, float tMax) {

            for (int i = 0; i < 3; i++) {

                float invD = 1.0f / ray.Direction[i];
                float t0 = (min[i] - ray.Origin[i]) * invD;
                float t1 = (max[i] - ray.Origin[i]) * invD;
                if (invD < 0.0) {

                    float tmp = t0;
                    t0 = t1;
                    t1 = tmp;

                    //Utils.Swap<float>(ref t0, ref t1);
                }
                tMin = t0 > tMin ? t0 : tMin;
                tMax = t1 < tMax ? t1 : tMax;
                if (tMax <= tMin) {
                    return false;
                }
            }

            return true;
        }

        //-----------------------------------------------------------------------------

        public float Area() {

            float a = max.x - min.x;
            float b = max.y - min.y;
            float c = max.z - min.z;
            return 2 * (a * b + b * c + c * a);
        }

        //-----------------------------------------------------------------------------

        public int LongestAxis() {

            float a = max.x - min.x;
            float b = max.y - min.y;
            float c = max.z - min.z;
            if (a > b && a > c)
                return 0;
            else if (b > c)
                return 1;
            else
                return 2;
        }

    }
}
