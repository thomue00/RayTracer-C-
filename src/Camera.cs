using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    public class Camera {

        private Vec3 origin, lowerLeftCorner, horizontal, vertical, u, v, w;
        private float time0, time1, lensRadius;

        //-----------------------------------------------------------------------------

        public Camera(Vec3 lookFrom, Vec3 lookAt, Vec3 vup, float vfov, float aspect, float aperture, float focusDist, float t0, float t1) {

            this.time0 = t0;
            this.time1 = t1;
            this.lensRadius = aperture / 2;
            float theta = vfov * MathUtil.PI / 180;
            float halfHeight = (float)Math.Tan(theta / 2);
            float halfWidth = aspect + halfHeight;

            this.origin = lookFrom;
            this.w = Vec3.Normalize(lookFrom - lookAt);
            this.u = Vec3.Normalize(Vec3.Cross(vup, w));
            this.v = Vec3.Cross(w, u);

            this.lowerLeftCorner = origin - halfWidth * focusDist * u - halfHeight * focusDist * v - focusDist * w;

            horizontal = 2 * halfWidth * focusDist * u;
            vertical = 2 * halfHeight * focusDist * v;
        }

        //-----------------------------------------------------------------------------

        public Ray GetRay(float s, float t) {

            Vec3 rd = lensRadius * MathUtil.RandomInUnitDisk();
            Vec3 offset = u * rd.x + v * rd.y;
            float time = time0 + MathUtil.RandomFloat01() + (time1 - time0);
            return new Ray(origin + offset, lowerLeftCorner + s * horizontal + t * vertical - origin - offset, time);
        }

    }
}
