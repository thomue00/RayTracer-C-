using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    public class BVHNode : Hitable {

        private Hitable left;
        private Hitable right;
        private Aabb box;

        //-----------------------------------------------------------------------------

        public BVHNode(Hitable[] l, int n, float time0, float time1) {

            Aabb[] boxes = new Aabb[n];
            float[] left_area = new float[n];
            float[] right_area = new float[n];
            Aabb main_box = new Aabb();
            l[0].BoundingBox(time0, time1, ref main_box);
            for (int i = 1; i < n; i++) {

                Aabb new_box = new Aabb();
                l[i].BoundingBox(time0, time1, ref new_box);
                main_box = Aabb.SurroundingBox(new_box, main_box);
            }
            int axis = main_box.LongestAxis();
            if (axis == 0) {
                Array.Sort(l, new BoxComparerX());
            }
            else if (axis == 1) {
                Array.Sort(l, new BoxComparerY());
            }
            else {
                Array.Sort(l, new BoxComparerZ());
            }
            for (int i = 0; i < n; i++) {
                bool dummy = l[i].BoundingBox(time0, time1, ref boxes[i]);
            }
            left_area[0] = boxes[0].Area();
            Aabb left_box = boxes[0];
            for (int i = 1; i < n - 1; i++) {

                left_box = Aabb.SurroundingBox(left_box, boxes[i]);
                left_area[i] = left_box.Area();
            }
            right_area[n - 1] = boxes[n - 1].Area();
            Aabb right_box = boxes[n - 1];
            for (int i = n - 2; i > 0; i--) {

                right_box = Aabb.SurroundingBox(right_box, boxes[i]);
                right_area[i] = right_box.Area();
            }
            float min_SAH = float.MaxValue;
            int min_SAH_idx;
            for (int i = 0; i < n - 1; i++) {

                float SAH = i * left_area[i] + (n - i - 1) * right_area[i + 1];
                if (SAH < min_SAH) {

                    min_SAH_idx = i;
                    min_SAH = SAH;
                }
            }

            if (n == 1) {
                left = right = l[0];
            }
            else if (n == 2) {
                left = l[0];
                right = l[1];
            }
            else {

                Hitable[] leftSlice;
                Hitable[] rightSlice;
                Utils.ArraySplit<Hitable>(l, n / 2, out leftSlice, out rightSlice);

                left = new BVHNode(leftSlice, leftSlice.Length, time0, time1);
                right = new BVHNode(rightSlice, rightSlice.Length, time0, time1);

                //left = new BVHNode(l, n / 2, time0, time1);
                //Hitable[] part = new Hitable[n - n / 2];
                //Array.Copy(l, n / 2, part, 0, n - n / 2);
                //right = new BVHNode(part, n - n / 2, time0, time1);
            }

            box = main_box;
        }

        //-----------------------------------------------------------------------------

        public override bool BoundingBox(float t0, float t1, ref Aabb box) {

            this.box = box;
            return true;
        }

        //-----------------------------------------------------------------------------

        public override bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec) {

            if (box.Hit(r, tMin, tMax)) {

                HitRecord left_rec = new HitRecord();
                HitRecord right_rec = new HitRecord();
                bool hit_left = left.Hit(r, tMin, tMax, ref left_rec);
                bool hit_right = right.Hit(r, tMin, tMax, ref right_rec);
                if (hit_left && hit_right) {

                    if (left_rec.t < right_rec.t) {
                        rec = left_rec;
                    }
                    else {
                        rec = right_rec;
                    }
                    return true;
                }
                else if (hit_left) {

                    rec = left_rec;
                    return true;
                }
                else if (hit_right) {

                    rec = right_rec;
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }

        //-----------------------------------------------------------------------------
        // Comparer
        //-----------------------------------------------------------------------------

        public class BoxComparerX : IComparer<Hitable> {

            public int Compare(Hitable x, Hitable y) {
                throw new NotImplementedException();
            }
        }

        //-----------------------------------------------------------------------------

        public class BoxComparerY : IComparer<Hitable> {

            public int Compare(Hitable x, Hitable y) {
                throw new NotImplementedException();
            }
        }

        //-----------------------------------------------------------------------------

        public class BoxComparerZ : IComparer<Hitable> {

            public int Compare(Hitable x, Hitable y) {
                throw new NotImplementedException();
            }
        }

    }
}
