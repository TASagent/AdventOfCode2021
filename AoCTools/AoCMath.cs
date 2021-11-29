namespace AoCTools;

public static class AoCMath
{
    public static Point4D Min(Point4D a, Point4D b) => new Point4D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y),
        z: Math.Min(a.z, b.z),
        w: Math.Min(a.w, b.w));

    public static Point4D Max(Point4D a, Point4D b) => new Point4D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y),
        z: Math.Max(a.z, b.z),
        w: Math.Max(a.w, b.w));

    public static Point3D Min(Point3D a, Point3D b) => new Point3D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y),
        z: Math.Min(a.z, b.z));

    public static Point3D Max(Point3D a, Point3D b) => new Point3D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y),
        z: Math.Max(a.z, b.z));

    public static Point2D Min(Point2D a, Point2D b) => new Point2D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y));

    public static Point2D Max(Point2D a, Point2D b) => new Point2D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y));

    public static LongPoint4D Min(LongPoint4D a, LongPoint4D b) => new LongPoint4D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y),
        z: Math.Min(a.z, b.z),
        w: Math.Min(a.w, b.w));

    public static LongPoint4D Max(LongPoint4D a, LongPoint4D b) => new LongPoint4D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y),
        z: Math.Max(a.z, b.z),
        w: Math.Max(a.w, b.w));

    public static LongPoint3D Min(LongPoint3D a, LongPoint3D b) => new LongPoint3D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y),
        z: Math.Min(a.z, b.z));

    public static LongPoint3D Max(LongPoint3D a, LongPoint3D b) => new LongPoint3D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y),
        z: Math.Max(a.z, b.z));

    public static LongPoint2D Min(LongPoint2D a, LongPoint2D b) => new LongPoint2D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y));

    public static LongPoint2D Max(LongPoint2D a, LongPoint2D b) => new LongPoint2D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y));


    public static Vector4D Min(Vector4D a, Vector4D b) => new Vector4D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y),
        z: Math.Min(a.z, b.z),
        w: Math.Min(a.w, b.w));

    public static Vector4D Max(Vector4D a, Vector4D b) => new Vector4D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y),
        z: Math.Max(a.z, b.z),
        w: Math.Max(a.w, b.w));

    public static Vector3D Min(Vector3D a, Vector3D b) => new Vector3D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y),
        z: Math.Min(a.z, b.z));

    public static Vector3D Max(Vector3D a, Vector3D b) => new Vector3D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y),
        z: Math.Max(a.z, b.z));

    public static Vector2D Min(Vector2D a, Vector2D b) => new Vector2D(
        x: Math.Min(a.x, b.x),
        y: Math.Min(a.y, b.y));

    public static Vector2D Max(Vector2D a, Vector2D b) => new Vector2D(
        x: Math.Max(a.x, b.x),
        y: Math.Max(a.y, b.y));
}
