using RayTracer.src;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RayTracer {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {

            InitializeComponent();
            this.Title = "Raytracer 0.1a";
        }

        //-----------------------------------------------------------------------------

        private void Btn_Press_Raytrace(object sender, RoutedEventArgs e) {

            Console.WriteLine("Width: " + this.RenderOutput.Width);
            Console.WriteLine("Height: " + this.RenderOutput.Height);
            //Scene main = new Scene(
            //    (int)this.RenderOutput.Width,
            //    (int)this.RenderOutput.Height,
            //    10,
            //    new Vec3(0, 2, -15),
            //    new Vec3(0, 1, 0),
            //    Test01(),
            //    _vfov: 40);

            Scene main = new Scene(
                (int)this.RenderOutput.Width,
                (int)this.RenderOutput.Height,
                10,
                new Vec3(278, 278, -800),
                new Vec3(278, 278, 0),
                Cornell(),
                _vfov: 40);
            this.RenderOutput.Source = main.Render();
        }

        //-----------------------------------------------------------------------------

        private void Btn_Press_SaveToDisc(object sender, RoutedEventArgs e) {

            if (this.RenderOutput.Source != null) {

                Console.WriteLine("Saving Image");
                this.SaveToFile("OutputTest.png", this.RenderOutput.Source as BitmapSource);
            }
            else {
                Console.WriteLine("No Image to Save");
            }
        }

        //-----------------------------------------------------------------------------

        private Hitable Test01() {

            Texture checker = new CheckerTexture(
                new ConstantTexture(new Vec3(0.8F, 0F, 0F)),
                new ConstantTexture(new Vec3(0.9F, 0.9F, 0.9F)));

            Texture blue = new ConstantTexture(Vec3.Forward);
            Texture red = new ConstantTexture(Vec3.Right);
            List<Hitable> list = new List<Hitable>();
            list.Add(new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertian(red)));
            list.Add(new Sphere(new Vec3(2, 2, 0), 1, new Metal(Vec3.Up, 1)));
            list.Add(new Sphere(new Vec3(-2, 2, 0), 1, new Lambertian(blue)));
            //list.Add(new RectXY(1, 2, 0, 1, 0, new Lambertian(blue)));
            return new HitableList(list.ToArray(), list.Count);
            //return new BVHNode(list, 2, 0, 1);
        }


        //-----------------------------------------------------------------------------

        private Hitable Cornell() {

            List<Hitable> list = new List<Hitable>();
            Material red = new Lambertian(new ConstantTexture(new Vec3(0.65F, 0.05F, 0.05F)));
            Material white = new Lambertian(new ConstantTexture(new Vec3(0.73F, 0.73F, 0.73F)));
            Material green = new Lambertian(new ConstantTexture(new Vec3(0.12F, 0.45F, 0.15F)));
            Material light = new DiffuseLight(new ConstantTexture(new Vec3(15, 15, 15)));

            list.Add(new FlipNormals(new RectYZ(0, 555, 0, 555, 555, green)));
            list.Add(new RectYZ(0, 555, 0, 555, 0, red));
            list.Add(new RectXZ(213, 343, 227, 332, 554, light));
            list.Add(new FlipNormals(new RectXZ(0, 555, 0, 555, 555, white)));
            list.Add(new RectXZ(0, 555, 0, 555, 0, white));
            list.Add(new FlipNormals(new RectXY(0, 555, 0, 555, 555, white)));
            list.Add(new Translate(new RotateY(new Box(new Vec3(0, 0, 0), new Vec3(165, 165, 165), white), -18), new Vec3(130, 0, 65)));
            list.Add(new Translate(new RotateY(new Box(new Vec3(0, 0, 0), new Vec3(165, 330, 165), white), 15), new Vec3(265, 0, 295)));
            return new HitableList(list.ToArray(), list.Count);
        }


        //-----------------------------------------------------------------------------

        private void SaveToFile(string filePath, BitmapSource imgSrc) {

            using (var fs = new FileStream(filePath, FileMode.Create)) {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(imgSrc));
                encoder.Save(fs);
            }
        }

    }


}
