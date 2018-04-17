using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {
    public class Box : Hitable {

        private Vec3 pMin, pMax;
        private HitableList list;

        //-----------------------------------------------------------------------------

        public Box(Vec3 pMin, Vec3 pMax, Material material) {

            this.pMin = pMin;
            this.pMax = pMax;
            Hitable[] tmp = new Hitable[6];
            tmp[0] = new RectXY(pMin.x, pMax.x, pMin.y, pMax.y, pMax.z, material);
            tmp[1] = new FlipNormals(new RectXY(pMin.x, pMax.x, pMin.y, pMax.y, pMin.z, material));
            tmp[2] = new RectXZ(pMin.x, pMax.x, pMin.z, pMax.z, pMax.y, material);
            tmp[3] = new FlipNormals(new RectXZ(pMin.x, pMax.x, pMin.z, pMax.z, pMin.y, material));
            tmp[4] = new RectYZ(pMin.y, pMax.y, pMin.z, pMax.z, pMax.x, material);
            tmp[5] = new FlipNormals(new RectYZ(pMin.y, pMax.y, pMin.z, pMax.z, pMin.x, material));
            this.list = new HitableList(tmp, 6);
        }

        //-----------------------------------------------------------------------------

        public override bool BoundingBox(float t0, float t1, ref Aabb box) {

            box = new Aabb(pMin, pMax);
            return true;
        }

        //-----------------------------------------------------------------------------

        public override bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec) {

            return this.list.Hit(r, tMin, tMax, ref rec);
        }
    }
}
