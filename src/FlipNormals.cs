using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    public class FlipNormals : Hitable {

        private Hitable hitable;

        //-----------------------------------------------------------------------------

        public FlipNormals(Hitable hitable) {
            this.hitable = hitable;
        }

        //-----------------------------------------------------------------------------

        public override bool BoundingBox(float t0, float t1, ref Aabb box) {
            return this.hitable.BoundingBox(t0, t1, ref box);
        }

        //-----------------------------------------------------------------------------

        public override bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec) {

            if (this.hitable.Hit(r, tMin, tMax, ref rec)) {

                rec.normal = -rec.normal;
                return true;
            }
            return false;
        }
    }
}
