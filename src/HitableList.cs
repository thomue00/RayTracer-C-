using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    class HitableList : Hitable {

        private Hitable[] list;
        private int listSize;

        //-----------------------------------------------------------------------------

        public HitableList(Hitable[] list, int listSize) {

            this.list = list;
            this.listSize = listSize;
        }

        //-----------------------------------------------------------------------------

        public override bool BoundingBox(float t0, float t1, ref Aabb box) {

            if (listSize < 1) {

                box = new Aabb();
                return false;
            }
            Aabb temp_box = new Aabb();
            bool first_true = list[0].BoundingBox(t0, t1, ref temp_box);
            if (!first_true) {

                box = new Aabb();
                return false;
            }
            else {
                box = temp_box;
            }

            for (int i = 1; i < listSize; i++) {

                if (list[i].BoundingBox(t0, t1, ref temp_box)) {
                    box = Aabb.SurroundingBox(box, temp_box);
                }
                else {
                    return false;
                }
            }
            return true;
        }

        //-----------------------------------------------------------------------------

        public override bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec) {

            HitRecord temp_rec = new HitRecord();
            bool hit_anything = false;
            float closest_so_far = tMax;
            for (int i = 0; i < listSize; i++) {

                if (list[i].Hit(r, tMin, closest_so_far, ref temp_rec)) {

                    hit_anything = true;
                    closest_so_far = temp_rec.t;
                    rec = temp_rec;
                }
            }
            return hit_anything;
        }
    }
}
