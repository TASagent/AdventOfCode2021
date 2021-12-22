namespace AoCTools;

public readonly struct MatrixPoint3D
{
    //  Matrix:
    //  ( a, b, c )
    //  ( d, e, f )
    //  ( g, h, i )

    public readonly int a;
    public readonly int b;
    public readonly int c;
    public readonly int d;
    public readonly int e;
    public readonly int f;
    public readonly int g;
    public readonly int h;
    public readonly int i;

    public static readonly MatrixPoint3D Identity = new MatrixPoint3D(1, 0, 0, 0, 1, 0, 0, 0, 1);

    public MatrixPoint3D(
        int a, int b, int c,
        int d, int e, int f,
        int g, int h, int i)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
        this.e = e;
        this.f = f;
        this.g = g;
        this.h = h;
        this.i = i;
    }

    public MatrixPoint3D(int[] values)
    {
        if (values is null || values.Length != 9)
        {
            throw new ArgumentException("Bad int array", nameof(values));
        }

        a = values[0];
        b = values[1];
        c = values[2];
        d = values[3];
        e = values[4];
        f = values[5];
        g = values[6];
        h = values[7];
        i = values[8];
    }

    public static MatrixPoint3D RotateX = new MatrixPoint3D(1, 0, 0, 0, 0, -1, 0, 1, 0);
    public static MatrixPoint3D RotateY = new MatrixPoint3D(0, 0, 1, 0, 1, 0, -1, 0, 0);
    public static MatrixPoint3D RotateZ = new MatrixPoint3D(0, -1, 0, 1, 0, 0, 0, 0, 1);

    public override bool Equals(object obj)
    {
        if (obj is not MatrixPoint3D other)
        {
            return false;
        }

        return a == other.a && b == other.b && c == other.c &&
            d == other.d && e == other.e && f == other.f &&
            g == other.g && h == other.h && i == other.i;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            HashCode.Combine(a, b, c),
            HashCode.Combine(d, e, f),
            HashCode.Combine(g, h, i));

    public override string ToString() => $"[({a}, {b}, {c}), ({d}, {e}, {f}), ({g}, {h}, {i})]";

    public static bool operator ==(in MatrixPoint3D lhs, in MatrixPoint3D rhs) =>
        lhs.a == rhs.a && lhs.b == rhs.b && lhs.c == rhs.c &&
        lhs.d == rhs.d && lhs.e == rhs.e && lhs.f == rhs.f &&
        lhs.g == rhs.g && lhs.h == rhs.h && lhs.i == rhs.i;

    public static bool operator !=(in MatrixPoint3D lhs, in MatrixPoint3D rhs) =>
        lhs.a != rhs.a || lhs.b != rhs.b || lhs.c != rhs.c ||
        lhs.d != rhs.d || lhs.e != rhs.e || lhs.f != rhs.f ||
        lhs.g != rhs.g || lhs.h != rhs.h || lhs.i != rhs.i;


    //MatrixPoint3D * Point3D
    public static Point3D operator *(in MatrixPoint3D lhs, in Point3D rhs) =>
        new Point3D(
            x: lhs.a * rhs.x + lhs.b * rhs.y + lhs.c * rhs.z,
            y: lhs.d * rhs.x + lhs.e * rhs.y + lhs.f * rhs.z,
            z: lhs.g * rhs.x + lhs.h * rhs.y + lhs.i * rhs.z);

    //MatrixPoint3D * MatrixPoint3D
    public static MatrixPoint3D operator *(in MatrixPoint3D lhs, in MatrixPoint3D rhs) =>
        new MatrixPoint3D(
            a: lhs.a * rhs.a + lhs.b * rhs.d + lhs.c * rhs.g,
            b: lhs.a * rhs.b + lhs.b * rhs.e + lhs.c * rhs.h,
            c: lhs.a * rhs.c + lhs.b * rhs.f + lhs.c * rhs.i,

            d: lhs.d * rhs.a + lhs.e * rhs.d + lhs.f * rhs.g,
            e: lhs.d * rhs.b + lhs.e * rhs.e + lhs.f * rhs.h,
            f: lhs.d * rhs.c + lhs.e * rhs.f + lhs.f * rhs.i,

            g: lhs.g * rhs.a + lhs.h * rhs.d + lhs.i * rhs.g,
            h: lhs.g * rhs.b + lhs.h * rhs.e + lhs.i * rhs.h,
            i: lhs.g * rhs.c + lhs.h * rhs.f + lhs.i * rhs.i);


    public static MatrixPoint3D operator -(in MatrixPoint3D value) =>
        new MatrixPoint3D(
            a: -value.a,
            b: -value.b,
            c: -value.c,
            d: -value.d,
            e: -value.e,
            f: -value.f,
            g: -value.g,
            h: -value.h,
            i: -value.i);

    public static MatrixPoint3D operator +(in MatrixPoint3D lhs, in MatrixPoint3D rhs) =>
        new MatrixPoint3D(
            a: lhs.a + rhs.a,
            b: lhs.b + rhs.b,
            c: lhs.c + rhs.c,
            d: lhs.d + rhs.d,
            e: lhs.e + rhs.e,
            f: lhs.f + rhs.f,
            g: lhs.g + rhs.g,
            h: lhs.h + rhs.h,
            i: lhs.i + rhs.i);

    public static MatrixPoint3D operator -(in MatrixPoint3D lhs, in MatrixPoint3D rhs) =>
        new MatrixPoint3D(
            a: lhs.a - rhs.a,
            b: lhs.b - rhs.b,
            c: lhs.c - rhs.c,
            d: lhs.d - rhs.d,
            e: lhs.e - rhs.e,
            f: lhs.f - rhs.f,
            g: lhs.g - rhs.g,
            h: lhs.h - rhs.h,
            i: lhs.i - rhs.i);

    public static MatrixPoint3D operator *(in MatrixPoint3D lhs, int rhs) =>
        new MatrixPoint3D(
            a: lhs.a * rhs,
            b: lhs.b * rhs,
            c: lhs.c * rhs,
            d: lhs.d * rhs,
            e: lhs.e * rhs,
            f: lhs.f * rhs,
            g: lhs.g * rhs,
            h: lhs.h * rhs,
            i: lhs.i * rhs);

    public static MatrixPoint3D operator *(int lhs, in MatrixPoint3D rhs) =>
        new MatrixPoint3D(
            a: lhs * rhs.a,
            b: lhs * rhs.b,
            c: lhs * rhs.c,
            d: lhs * rhs.d,
            e: lhs * rhs.e,
            f: lhs * rhs.f,
            g: lhs * rhs.g,
            h: lhs * rhs.h,
            i: lhs * rhs.i);

    public void Deconstruct(
        out int a, out int b, out int c,
        out int d, out int e, out int f,
        out int g, out int h, out int i)
    {
        a = this.a;
        b = this.b;
        c = this.c;
        d = this.d;
        e = this.e;
        f = this.f;
        g = this.g;
        h = this.h;
        i = this.i;
    }
}
