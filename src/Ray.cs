using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    public class Ray {

        //-----------------------------------------------------------------------------
        // Member
        //-----------------------------------------------------------------------------

        private Vec3 a, b;
        private float time;

        //-----------------------------------------------------------------------------
        // Properties
        //-----------------------------------------------------------------------------

        public Vec3 Origin {  get { return this.a; } }

        //-----------------------------------------------------------------------------

        public Vec3 Direction { get { return this.b; } }

        //-----------------------------------------------------------------------------

        public float Time { get { return this.time; } }

        //-----------------------------------------------------------------------------

        public Vec3 PointAtParameter(float t) {
            return this.a + this.time * this.b;
        }

        //-----------------------------------------------------------------------------
        // Methods
        //-----------------------------------------------------------------------------

        public Ray() { }

        //-----------------------------------------------------------------------------

        public Ray(Vec3 a, Vec3 b, float time = 0.0F) {

            this.a = a;
            this.b = b;
            this.time = time;
        }

    }
}
