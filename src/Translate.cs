using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    public class Translate : Hitable{

        private Hitable hitable;
        private Vec3 offset;

        //-----------------------------------------------------------------------------

        public Translate(Hitable hitable, Vec3 offset) {

            this.hitable = hitable;
            this.offset = offset;
        }

        //-----------------------------------------------------------------------------

        public override bool BoundingBox(float t0, float t1, ref Aabb box) {

            if (this.hitable.BoundingBox(t0, t1, ref box)) {

                box = new Aabb(box.Min + offset, box.Max + offset);
                return true;
            }
            else {
                return false;
            }
        }

        //-----------------------------------------------------------------------------

        public override bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec) {

            Ray moved_ray = new Ray(r.Origin -offset, r.Direction, r.Time);
            if (this.hitable.Hit(moved_ray, tMin, tMax, ref rec)) {

                rec.p += offset;
                return true;
            }
            else {
                return false;
            }
        }
    }
}
