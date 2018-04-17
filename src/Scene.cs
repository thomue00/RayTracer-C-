using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RayTracer.src {

    public class Scene {

        private int nx, ny, ns, max_depth;
        private Vec3 look_from, look_at;
        private float focus_dist, aperture, vfov;
        private Camera cam;
        private Vec3[] colors;
        private Hitable world;

        public Scene(int width, int height, int samples, Vec3 look_from, Vec3 look_at, Hitable _world, int _max_depth = 50, float _focus_dist = 10F, float _aperture = 0.0F, float _vfov = 40) {

            this.nx = width;
            this.ny = height;
            this.ns = samples;
            this.max_depth = _max_depth;
            this.look_from = look_from;
            this.look_at = look_at;
            this.focus_dist = _focus_dist;
            this.aperture = _aperture;
            this.vfov = _vfov;
            this.world = _world;
            float aspect = (float)nx / (float)ny;
            this.cam = new Camera(this.look_from, this.look_at, Vec3.Up, this.vfov, aspect, this.aperture, this.focus_dist, 0, 1);
            this.colors = new Vec3[nx * ny];
        }

        //-----------------------------------------------------------------------------

        public void RenderRowJob(int y, int width, int height, int samplesPerPixel) {

            //int bufferIdx = rowY * width * 4;

            float invWidth = 1f / width;
            float invHeight = 1f / height;

            for (int x = 0; x < width; x++) {

                Vec3 col = Vec3.Zero;
                for (int s = 0; s < samplesPerPixel; s++) {

                    float u = (x + MathUtil.RandomFloat01()) * invWidth;
                    float v = (y + MathUtil.RandomFloat01()) * invHeight;
                    Ray r = cam.GetRay(u, v);
                    col += this.Trace(r, 0);
                }
                col *= 1F / (float)ns;

                if (y % 20 == 0 || x % 20 == 0) {
                    col = Vec3.Zero;
                }

                Vec3 c = new Vec3((float)Math.Sqrt(col.x), (float)Math.Sqrt(col.y), (float)Math.Sqrt(col.z));
                c.Clamp01();
                this.colors[y * width + x] = c;
            }
        }

        //-----------------------------------------------------------------------------

        public BitmapSource Render(string name = "output") {

            float invWidth = 1f / nx;
            float invHeight = 1f / ny;

            Parallel.For(0, this.ny, y => {
                this.RenderRowJob(y, this.nx, this.ny, this.ns);
                //for (int x = 0; x < nx; x++) {

                //    Vec3 col = Vec3.Zero;
                //    for (int s = 0; s < ns; s++) {

                //        float u = (x + MathUtil.RandomFloat01()) * invWidth;
                //        float v = (y + MathUtil.RandomFloat01()) * invHeight;
                //        Ray r = cam.GetRay(u, v);
                //        Vec3 p = r.PointAtParameter(2F);
                //        col += this.Trace(r, 0);
                //    }

                //    col /= (float)ns;
                //    Vec3 c = new Vec3((float)Math.Sqrt(col.x), (float)Math.Sqrt(col.y), (float)Math.Sqrt(col.z));
                //    c.Clamp01();
                //    this.colors[x + y * nx] = c;
                //}
            });

            return this.Save(name);
        }

        //-----------------------------------------------------------------------------

        private BitmapSource Save(string name) {

            byte[] pixels = new byte[this.nx * this.ny * 3];

            for (int y = ny - 1; y >= 0; y--) {

                for (int x = 0; x < nx; x++) {

                    Vec3 col = this.colors[x + (ny - y - 1) * nx];
                    int ir = (int)(255.99 * col.x);
                    int ig = (int)(255.99 * col.y);
                    int ib = (int)(255.99 * col.z);
                    ir = ir > 255 ? 255 : ir < 0 ? 0 : ir;
                    ig = ig > 255 ? 255 : ig < 0 ? 0 : ig;
                    ib = ib > 255 ? 255 : ib < 0 ? 0 : ib;

                    pixels[((x + y * nx) * 3 + 0)] = Convert.ToByte(ir);
                    pixels[((x + y * nx) * 3 + 1)] = Convert.ToByte(ig);
                    pixels[((x + y * nx) * 3 + 2)] = Convert.ToByte(ib);
                }
            }


            int stride = nx * 3;
            BitmapSource bms = BitmapSource.Create(nx, ny, 96, 96, PixelFormats.Rgb24, null, pixels, stride);

            return bms;

        }

        //-----------------------------------------------------------------------------

        private Vec3 Trace(Ray r, int depth) {

            HitRecord rec = new HitRecord();
            if (world.Hit(r, 0.001F, float.MaxValue, ref rec)) {

                Ray scattered = new Ray();
                Vec3 attenuation = Vec3.Zero;
                Vec3 emitted = rec.mat.Emitted(rec.u, rec.v, rec.p);
                if (depth < max_depth && rec.mat.Scatter(r, rec, ref attenuation, ref scattered)) {
                    return emitted + attenuation * this.Trace(scattered, depth + 1);
                }
                else {
                    return emitted;
                }
            }
            else {

                // Sky
                Vec3 unitDir = r.Direction;
                float t = 0.5f * (unitDir.y + 1.0f);
                return ((1.0f - t) * Vec3.One + t * new Vec3(0.5f, 0.7f, 1.0f)) * 0.3f;
            }

        }

    }
}
