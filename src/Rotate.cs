using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    public class RotateY : Hitable {

        private Hitable hitable;
        private float sinTheta;
        private float cosTheta;
        private bool hasBox;
        private Aabb bBox;

        //-----------------------------------------------------------------------------

        public RotateY(Hitable hitable, float angle) {

            this.hitable = hitable;
            float radians = (MathUtil.PI / 180F) * angle;
            sinTheta = (float)Math.Sin(radians);
            cosTheta = (float)Math.Cos(radians);
            hasBox = this.hitable.BoundingBox(0, 1, ref bBox);
            Vec3 min = Vec3.PositiveInfinity;
            Vec3 max = Vec3.NegativeInfinity;
            for (int i = 0; i < 2; i++) {

                for (int j = 0; j < 2; j++) {

                    for (int k = 0; k < 2; k++) {

                        float x = i * bBox.Max.x + (1 - i) * bBox.Min.x;
                        float y = j * bBox.Max.y + (1 - j) * bBox.Min.y;
                        float z = k * bBox.Max.z + (1 - k) * bBox.Min.z;
                        float newx = cosTheta * x + sinTheta * z;
                        float newz = -sinTheta * x + cosTheta * z;
                        Vec3 tester = new Vec3(newx, y, newz);
                        for (int c = 0; c < 3; c++) {

                            if (tester[c] > max[c]) {
                                max[c] = tester[c];
                            }
                            if (tester[c] < min[c]) {
                                min[c] = tester[c];
                            }
                        }
                    }
                }
            }
            bBox = new Aabb(min, max);
        }

        //-----------------------------------------------------------------------------

        public override bool BoundingBox(float t0, float t1, ref Aabb box) {

            box = bBox;
            return this.hasBox;
        }

        //-----------------------------------------------------------------------------

        public override bool Hit(Ray r, float tMin, float tMax, ref HitRecord rec) {

            Vec3 origin = r.Origin;
            Vec3 direction = r.Direction;
            origin[0] = cosTheta * r.Origin[0] - sinTheta * r.Origin[2];
            origin[2] = sinTheta * r.Origin[0] + cosTheta * r.Origin[2];
            direction[0] = cosTheta * r.Direction[0] - sinTheta * r.Direction[2];
            direction[2] = sinTheta * r.Direction[0] + cosTheta * r.Direction[2];
            Ray rotated_r = new Ray(origin, direction, r.Time);
            if (hitable.Hit(rotated_r, tMin, tMax, ref rec)) {

                Vec3 p = rec.p;
                Vec3 normal = rec.normal;
                p[0] = cosTheta * rec.p[0] + sinTheta * rec.p[2];
                p[2] = -sinTheta * rec.p[0] + cosTheta * rec.p[2];
                normal[0] = cosTheta * rec.normal[0] + sinTheta * rec.normal[2];
                normal[2] = -sinTheta * rec.normal[0] + cosTheta * rec.normal[2];
                rec.p = p;
                rec.normal = normal;
                return true;
            }

            return false;
        }
    }
}
