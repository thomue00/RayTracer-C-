using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    public abstract class Texture {

        public abstract Vec3 Value(float u, float v, Vec3 p);
    }


    //-----------------------------------------------------------------------------
    // Constant Texture
    //-----------------------------------------------------------------------------

    public class ConstantTexture : Texture {

        private Vec3 color;
        public Vec3 Color { get => color; }

        //-----------------------------------------------------------------------------

        public ConstantTexture(Vec3 color) {
            this.color = color;
        }

        //-----------------------------------------------------------------------------

        public override Vec3 Value(float u, float v, Vec3 p) {
            return color;
        }
    }

    //-----------------------------------------------------------------------------
    // Checker Texture
    //-----------------------------------------------------------------------------

    public class CheckerTexture : Texture {

        private Texture odd, even;

        //-----------------------------------------------------------------------------

        public CheckerTexture(Texture t0, Texture t1) {

            this.even = t0;
            this.odd = t1;
        }

        //-----------------------------------------------------------------------------

        public override Vec3 Value(float u, float v, Vec3 p) {

            float sines = (float)Math.Sin(1F * p.x) * (float)Math.Sin(1F * p.y) * (float)Math.Sin(1F * p.z);
            return sines < 0 ? odd.Value(u, v, p) : even.Value(u, v, p);
        }
    }


    //-----------------------------------------------------------------------------
    // Image Texture
    //-----------------------------------------------------------------------------

}
