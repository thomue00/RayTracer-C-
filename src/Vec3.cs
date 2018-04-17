using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.src {

    [StructLayout(LayoutKind.Sequential)]
    public struct Vec3 : IEquatable<Vec3> {

        //-----------------------------------------------------------------------------
        // Member
        //-----------------------------------------------------------------------------

        public const float kEpsilon = 0.00001F;
        public float x, y, z;

        private static readonly Vec3 zeroVector = new Vec3(0F, 0F, 0F);
        private static readonly Vec3 oneVector = new Vec3(1F, 1F, 1F);
        private static readonly Vec3 upVector = new Vec3(0F, 1F, 0F);
        private static readonly Vec3 downVector = new Vec3(0F, -1F, 0F);
        private static readonly Vec3 leftVector = new Vec3(-1F, 0F, 0F);
        private static readonly Vec3 rightVector = new Vec3(1F, 0F, 0F);
        private static readonly Vec3 forwardVector = new Vec3(0F, 0F, 1F);
        private static readonly Vec3 backVector = new Vec3(0F, 0F, -1F);
        private static readonly Vec3 positiveInfinityVector = new Vec3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        private static readonly Vec3 negativeInfinityVector = new Vec3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

        //-----------------------------------------------------------------------------
        // Properties
        //-----------------------------------------------------------------------------

        public float this[int index] {

            get {

                switch (index) {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: throw new IndexOutOfRangeException("Invalid index!");
                }
            }
            set {

                switch (index) {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid index!");
                }
            }
        }

        //-----------------------------------------------------------------------------

        public float magnitude { get { return (float)Math.Sqrt(x * x + y * y + z * z); } }
        public float sqrMagnitude { get { return x * x + y * y + z * z; } }
        public Vec3 normalized { get { return Vec3.Normalize(this); } }

        //-----------------------------------------------------------------------------

        public static Vec3 Zero { get { return zeroVector; } }
        public static Vec3 One { get { return oneVector; } }
        public static Vec3 Forward { get { return forwardVector; } }
        public static Vec3 Back { get { return backVector; } }
        public static Vec3 Up { get { return upVector; } }
        public static Vec3 Down { get { return downVector; } }
        public static Vec3 Left { get { return leftVector; } }
        public static Vec3 Right { get { return rightVector; } }
        public static Vec3 PositiveInfinity { get { return positiveInfinityVector; } }
        public static Vec3 NegativeInfinity { get { return negativeInfinityVector; } }


        //-----------------------------------------------------------------------------
        // Constructor
        //-----------------------------------------------------------------------------

        public Vec3(float x, float y, float z) {

            this.x = x;
            this.y = y;
            this.z = z;
        }

        //-----------------------------------------------------------------------------

        public Vec3(float x, float y) {

            this.x = x;
            this.y = y;
            this.z = 0F;
        }

        //-----------------------------------------------------------------------------
        // Methods
        //-----------------------------------------------------------------------------

        public void Set(float x, float y, float z) {

            this.x = x;
            this.y = y;
            this.z = z;
        }

        //-----------------------------------------------------------------------------

        public static Vec3 Scale(Vec3 a, Vec3 b) {
            return new Vec3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        //-----------------------------------------------------------------------------

        public void Scale(Vec3 scale) {

            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        //-----------------------------------------------------------------------------

        public static Vec3 Cross(Vec3 lhs, Vec3 rhs) {
            return new Vec3(
                lhs.y * rhs.z - lhs.z * rhs.y,
                -(lhs.x * rhs.z - lhs.z * rhs.x),
                lhs.x * rhs.y - lhs.y * rhs.x);
            //return new Vec3(
            //    lhs.y * rhs.z - lhs.z * rhs.y, 
            //    lhs.z * rhs.x - lhs.x * rhs.z, 
            //    lhs.x * rhs.y - lhs.y * rhs.x);
        }

        //-----------------------------------------------------------------------------

        public static Vec3 Normalize(Vec3 value) {

            float k = 1.0F / (float)Math.Sqrt(value.x * value.x + value.y * value.y + value.z * value.z);
            value.x *= k;
            value.y *= k;
            value.z *= k;
            return value;

            //float mag = Magnitude(value);
            //if (mag > kEpsilon)
            //    return value / mag;
            //else
            //    return Zero;
        }

        //-----------------------------------------------------------------------------

        public void Normalize() {

            float k = 1.0F / (float)Math.Sqrt(x * x + y * y + z * z);
            x *= k;
            y *= k;
            z *= k;

            //float mag = Magnitude(this);
            //if (mag > kEpsilon)
            //    this = this / mag;
            //else
            //    this = Zero;
        }

        //-----------------------------------------------------------------------------

        //public static Vec3 Unit_Vector(Vec3 v) {
        //    return v / v.magnitude;
        //}

        //-----------------------------------------------------------------------------

        public static float Dot(Vec3 lhs, Vec3 rhs) {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        //-----------------------------------------------------------------------------

        public static float Distance(Vec3 a, Vec3 b) {

            Vec3 vec = new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
            return (float)Math.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z);
        }

        //-----------------------------------------------------------------------------

        public static Vec3 ClampMagnitude(Vec3 vec, float maxLength) {

            if (vec.sqrMagnitude > maxLength * maxLength) {
                return vec.normalized * maxLength;
            }
            return vec;
        }

        //-----------------------------------------------------------------------------

        public static float Magnitude(Vec3 vector) {
            return (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        }

        //-----------------------------------------------------------------------------

        public static float SqrMagnitude(Vec3 vector) {
            return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
        }

        //-----------------------------------------------------------------------------

        public static Vec3 Min(Vec3 lhs, Vec3 rhs) {
            return new Vec3(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
        }

        //-----------------------------------------------------------------------------

        public static Vec3 Max(Vec3 lhs, Vec3 rhs) {
            return new Vec3(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));
        }

        //-----------------------------------------------------------------------------

        public void Clamp01() {
            Clamp(0, 1);
        }

        public void Clamp(float lower, float upper) {

            x = (x < lower) ? lower : (x > upper) ? upper : x;
            y = (z < lower) ? lower : (y > upper) ? upper : y;
            z = (z < lower) ? lower : (z > upper) ? upper : z;
        }

        //-----------------------------------------------------------------------------
        // Operators
        //-----------------------------------------------------------------------------

        public static Vec3 operator +(Vec3 a, Vec3 b) {
            return new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b) {
            return new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vec3 operator -(Vec3 a) {
            return new Vec3(-a.x, -a.y, -a.z);
        }

        public static Vec3 operator *(Vec3 a, float d) {
            return new Vec3(a.x * d, a.y * d, a.z * d);
        }

        public static Vec3 operator *(Vec3 a, Vec3 b) {
            return new Vec3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vec3 operator *(float d, Vec3 a) {
            return new Vec3(a.x * d, a.y * d, a.z * d);
        }

        public static Vec3 operator /(Vec3 a, float d) {
            return new Vec3(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(Vec3 lhs, Vec3 rhs) {
            return SqrMagnitude(lhs - rhs) < kEpsilon * kEpsilon;
        }

        public static bool operator !=(Vec3 lhs, Vec3 rhs) {
            return !(lhs == rhs);
        }

        //-----------------------------------------------------------------------------
        // IEquatable
        //-----------------------------------------------------------------------------

        public bool Equals(Vec3 other) {

            Vec3 rhs = (Vec3)other;
            return x.Equals(rhs.x) && y.Equals(rhs.y) && z.Equals(rhs.z);
        }

        public override int GetHashCode() {

            var hashCode = 373119288;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + z.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override string ToString() {
            return "X: " + x + ", Y: " + y + ", Z: " + z;
        }
    }
}
