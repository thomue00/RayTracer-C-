using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {
    

    public struct HitRecord {

        public float t, u, v;
        public Vec3 p, normal;
        public Material mat;
    }

    public abstract class Hitable {

        public abstract bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec);
        public abstract bool BoundingBox(float t0, float t1, ref Aabb box);

    }
}
