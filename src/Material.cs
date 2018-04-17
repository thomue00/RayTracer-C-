using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    public abstract class Material {

        public abstract bool Scatter(Ray rayIn, HitRecord rec, ref Vec3 attenuattion, ref Ray scattered);

        public virtual Vec3 Emitted(float u, float v, Vec3 p) {
            return Vec3.Zero;
        }
    }

    //-----------------------------------------------------------------------------
    // Lambertian
    //-----------------------------------------------------------------------------

    public class Lambertian : Material {

        Texture albedo;

        //-----------------------------------------------------------------------------

        public Lambertian(Texture albedo) {
            this.albedo = albedo;
        }

        //-----------------------------------------------------------------------------

        public override bool Scatter(Ray rayIn, HitRecord rec, ref Vec3 attenuattion, ref Ray scattered) {

            Vec3 target = rec.p + rec.normal + MathUtil.RandomInUnitSphere();
            scattered = new Ray(rec.p, target - rec.p, rayIn.Time);
            attenuattion = this.albedo.Value(rec.u, rec.v, rec.p);
            return true;
        }
    }

    //-----------------------------------------------------------------------------
    // Metal
    //-----------------------------------------------------------------------------

    public class Metal : Material {

        Vec3 albedo;
        float fuzz;

        public Metal(Vec3 albedo, float fuzz) {

            this.albedo = albedo;
            this.fuzz = fuzz < 1 ? fuzz : 1;
        }

        //-----------------------------------------------------------------------------

        public override bool Scatter(Ray rayIn, HitRecord rec, ref Vec3 attenuattion, ref Ray scattered) {

            Vec3 reflected = Utils.Reflect(rayIn.Direction.normalized, rec.normal);
            scattered = new Ray(rec.p, reflected + fuzz * MathUtil.RandomInUnitSphere(), rayIn.Time);
            attenuattion = albedo;
            return Vec3.Dot(scattered.Direction, rec.normal) > 0;
        }
    }

    //-----------------------------------------------------------------------------
    // Dielectrict
    //-----------------------------------------------------------------------------

    public class Dielectric : Material {

        private float refIdx;

        //-----------------------------------------------------------------------------

        public Dielectric(float refIdx) {
            this.refIdx = refIdx;
        }

        //-----------------------------------------------------------------------------

        public override bool Scatter(Ray rayIn, HitRecord rec, ref Vec3 attenuattion, ref Ray scattered) {

            Vec3 outward_normal;
            Vec3 reflected = Utils.Reflect(rayIn.Direction, rec.normal);
            float ni_over_nt;
            attenuattion = Vec3.One;
            Vec3 refracted;
            float reflect_prob;
            float cosine;
            if (Vec3.Dot(rayIn.Direction, rec.normal) > 0) {

                outward_normal = -rec.normal;
                ni_over_nt = this.refIdx;
                cosine = Vec3.Dot(rayIn.Direction, rec.normal) / rayIn.Direction.magnitude;
                cosine = (float)Math.Sqrt(1.0F - this.refIdx * this.refIdx * (1.0 - cosine * cosine));
            }
            else {

                outward_normal = rec.normal;
                ni_over_nt = 1.0F / this.refIdx;
                cosine = -Vec3.Dot(rayIn.Direction, rec.normal) / rayIn.Direction.magnitude;
            }
            if (Utils.Refract(rayIn.Direction, outward_normal, ni_over_nt, out refracted)) {
                reflect_prob = Utils.Schlick(cosine, this.refIdx);
            }
            else {
                reflect_prob = 1.0F;
            }
            if (MathUtil.RandomFloat01() > reflect_prob) {
                scattered = new Ray(rec.p, reflected, rayIn.Time);
            }
            else {
                scattered = new Ray(rec.p, refracted, rayIn.Time);
            }
            return true;
        }
    }


    //-----------------------------------------------------------------------------
    // Diffuse Light
    //-----------------------------------------------------------------------------

    public class DiffuseLight : Material {

        private Texture emit;

        //-----------------------------------------------------------------------------

        public DiffuseLight(Texture emit) {
            this.emit = emit;
        }

        //-----------------------------------------------------------------------------

        public override Vec3 Emitted(float u, float v, Vec3 p) {
            return emit.Value(u, v, p);
        }

        //-----------------------------------------------------------------------------

        public override bool Scatter(Ray rayIn, HitRecord rec, ref Vec3 attenuattion, ref Ray scattered) {

            return false;
        }
    }

    //-----------------------------------------------------------------------------
    // Isotropic
    //-----------------------------------------------------------------------------

    public class Isotropic : Material {

        private Texture albedo;

        //-----------------------------------------------------------------------------

        public Isotropic(Texture albedo) {
            this.albedo = albedo;
        }

        //-----------------------------------------------------------------------------

        public override bool Scatter(Ray rayIn, HitRecord rec, ref Vec3 attenuattion, ref Ray scattered) {

            scattered = new Ray(rec.p, MathUtil.RandomInUnitSphere());
            attenuattion = albedo.Value(rec.u, rec.v, rec.p);
            return true;
        }
    }

}
