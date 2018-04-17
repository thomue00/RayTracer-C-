using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    public class Sphere : Hitable {

        public Vec3 center;
        private float radius;
        private Material material;

        //-----------------------------------------------------------------------------

        public Sphere(Vec3 center, float radius, Material material) {

            this.center = center;
            this.radius = radius;
            this.material = material;
        }

        //-----------------------------------------------------------------------------

        public override bool BoundingBox(float t0, float t1, ref Aabb box) {

            box = new Aabb(
                center - new Vec3(radius, radius, radius), 
                center + new Vec3(radius, radius, radius));
            return true;
        }

        //-----------------------------------------------------------------------------

        public override bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec) {

            Vec3 oc = r.Origin - this.center;
            float a = Vec3.Dot(r.Direction, r.Direction);
            float b = Vec3.Dot(oc, r.Direction);
            float c = Vec3.Dot(oc, oc) - radius * radius;
            float discriminant = b * b - a * c;
            if (discriminant > 0) {

                float temp = (-b - (float)Math.Sqrt(discriminant)) / a;
                if (temp < tMax && temp > tMin) {

                    rec.t = temp;
                    rec.p = r.PointAtParameter(rec.t);
                    this.GetSphereUv((rec.p - center) / radius, ref rec.u, ref rec.v);
                    rec.normal = (rec.p - center) / radius;
                    rec.mat = this.material;
                    return true;
                }
                temp = (-b + (float)Math.Sqrt(discriminant)) / a;
                if (temp < tMax && temp > tMin) {

                    rec.t = temp;
                    rec.p = r.PointAtParameter(rec.t);
                    this.GetSphereUv((rec.p - center) / radius, ref rec.u, ref rec.v);
                    rec.normal = (rec.p - center) / radius;
                    rec.mat = this.material;
                    return true;
                }
            }
            return false;

        }

        private void GetSphereUv(Vec3 p, ref float u, ref float v) {

            float phi = (float)Math.Atan2(p.z, p.x);
            float theta = (float)Math.Asin(p.y);
            u = 1 - (phi + MathUtil.PI) / (2 * MathUtil.PI);
            v = (theta + MathUtil.PI / 2) / MathUtil.PI;
        }
    }
}
