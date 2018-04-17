using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    public class RectXY : Hitable {

        private Material mat;
        private float x0, x1, y0, y1, k;

        public RectXY(float x0, float x1, float y0, float y1, float k, Material mat) {

            this.mat = mat;
            this.x0 = x0;
            this.x1 = x1;
            this.y0 = y0;
            this.y1 = y1;
            this.k = k;
        }

        public override bool BoundingBox(float t0, float t1, ref Aabb box) {

            box = new Aabb(new Vec3(x0, y0, k - 0.001F), new Vec3(x1, y1, k + 0.001F));
            return true;
        }

        public override bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec) {

            float t = (k - r.Origin.z) / r.Direction.z;
            if (t < tMin || t > tMax)
                return false;
            float x = r.Origin.x + t * r.Direction.x;
            float y = r.Origin.y + t * r.Direction.y;
            if (x < x0 || x > x1 || y < y0 || y > y1)
                return false;
            rec.u = (x - x0) / (x1 - x0);
            rec.v = (y - y0) / (y1 - y0);
            rec.t = t;
            rec.mat = mat;
            rec.p = r.PointAtParameter(t);
            rec.normal = new Vec3(0, 0, 1);
            return true;
        }
    }

    //-----------------------------------------------------------------------------

    public class RectXZ : Hitable {

        private Material mat;
        private float x0, x1, z0, z1, k;

        public RectXZ(float x0, float x1, float z0, float z1, float k, Material mat) {

            this.mat = mat;
            this.x0 = x0;
            this.x1 = x1;
            this.z0 = z0;
            this.z1 = z1;
            this.k = k;
        }

        public override bool BoundingBox(float t0, float t1, ref Aabb box) {

            box = new Aabb(new Vec3(x0, k - 0.0001F, z0), new Vec3(x1, k + 0.0001F, z1));
            return true;
        }

        public override bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec) {

            float t = (k - r.Origin.y) / r.Direction.y;
            if (t < tMin || t > tMax)
                return false;
            float x = r.Origin.x + t * r.Direction.x;
            float z = r.Origin.z + t * r.Direction.z;
            if (x < x0 || x > x1 || z < z0 || z > z1)
                return false;
            rec.u = (x - x0) / (x1 - x0);
            rec.v = (z - z0) / (z1 - z0);
            rec.t = t;
            rec.mat = mat;
            rec.p = r.PointAtParameter(t);
            rec.normal = new Vec3(0, 1, 0);
            return true;
        }
    }

    //-----------------------------------------------------------------------------

    public class RectYZ : Hitable {

        private Material mat;
        private float z0, z1, y0, y1, k;

        public RectYZ(float z0, float z1, float y0, float y1, float k, Material mat) {

            this.mat = mat;
            this.z0 = z0;
            this.z1 = z1;
            this.y0 = y0;
            this.y1 = y1;
            this.k = k;
        }

        public override bool BoundingBox(float t0, float t1, ref Aabb box) {

            box = new Aabb(new Vec3(k - 0.0001F, y0, z0), new Vec3(k + 0.0001F, y1, z1));
            return true;
        }

        public override bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec) {

            float t = (k - r.Origin.x) / r.Direction.x;
            if (t < tMin || t > tMax)
                return false;
            float y = r.Origin.y + t * r.Direction.y;
            float z = r.Origin.z + t * r.Direction.z;
            if (y < y0 || y > y1 || z < z0 || z > z1)
                return false;
            rec.u = (y - y0) / (y1 - y0);
            rec.v = (z - z0) / (z1 - z0);
            rec.t = t;
            rec.mat = mat;
            rec.p = r.PointAtParameter(t);
            rec.normal = new Vec3(1, 0, 0);
            return true;
        }
    }
}
