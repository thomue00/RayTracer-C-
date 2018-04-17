using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    public class ConstantMedium : Hitable {

        private Hitable boundary;
        private float density;
        private Material phase_function;

        //-----------------------------------------------------------------------------

        public ConstantMedium(Hitable boundary, float density, Texture texture) {

            this.boundary = boundary;
            this.density = density;
            this.phase_function = new Isotropic(texture);
        }

        //-----------------------------------------------------------------------------

        public override bool BoundingBox(float t0, float t1, ref Aabb box) {

            return boundary.BoundingBox(t0, t1, ref box);
        }

        //-----------------------------------------------------------------------------

        public override bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec) {

            bool db = (MathUtil.RandomFloat01() < 0.00001);
            db = false;
            HitRecord rec1 = new HitRecord();
            HitRecord rec2 = new HitRecord();
            if (boundary.Hit(r, -float.MaxValue, float.MaxValue, ref rec1)) {

                if (boundary.Hit(r, rec1.t + 0.0001F, float.MaxValue, ref rec2)) {

                    if (db) {
                        throw new Exception ("\nt0 t1 " +  rec1.t + " " + rec2.t);
                    }
                    if (rec1.t < tMin) {
                        rec1.t = tMin;
                    }
                    if (rec2.t > tMax) {
                        rec2.t = tMax;
                    }
                    if (rec1.t >= rec2.t) {
                        return false;
                    }
                    if (rec1.t < 0) {
                        rec1.t = 0;
                    }
                    float distance_inside_boundary = (rec2.t - rec1.t) * r.Direction.magnitude;
                    float hit_distance = -(1 / density) * (float)Math.Log(MathUtil.RandomFloat01());
                    if (hit_distance < distance_inside_boundary) {

                        if (db) {
                            throw new Exception("hit_distance = " + hit_distance);
                        }
                        rec.t = rec1.t + hit_distance / r.Direction.magnitude;
                        if (db) {
                            throw new Exception("rec.t = " + rec.t);
                        }
                        rec.p = r.PointAtParameter(rec.t);
                        if (db) {
                            throw new Exception("rec.p = " + rec.p);
                        }
                        rec.normal = new Vec3(1, 0, 0);  // arbitrary
                        rec.mat = phase_function;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
