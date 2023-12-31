using OpenCvSharp;

namespace DistanceTransform;

public static class DistanceTransformer
{
    public record DistanceLabel
    {
        public int Y { get; }
        public int X { get; }
        public int Distance { get; }

        public DistanceLabel(int y, int x, int distance)
        {
            Y = y;
            X = x;
            Distance = distance;
        }
    }

    public static List<DistanceLabel> GetDistanceLabel(Mat binaryImage)
    {
        // 距離変換を適用する
        var dist = binaryImage.DistanceTransform(DistanceTypes.C, DistanceTransformMasks.Mask3);
        dist.MinMaxLoc(out double _, out var maxVal);

        // 出力画像の準備
        var output = new List<DistanceLabel>();
        // 距離値ごとに色を変えて描画する（パラレル処理）
        Parallel.For(0, (int) maxVal + 1, distance =>
        {
            for (var y = 0; y < dist.Rows; y++)
            for (var x = 0; x < dist.Cols; x++)
            {
                if ((int) dist.At<float>(y, x) != distance) continue;
                output.Add(new DistanceLabel(y, x, distance));
            }
        });

        return output;
    }

    public static void ShowDistanceColor(Mat binaryImage)
    {
        // 距離変換を適用する
        var dist = binaryImage.DistanceTransform(DistanceTypes.C, DistanceTransformMasks.Mask3);
        dist.MinMaxLoc(out double _, out var maxVal);

        // 出力画像の準備
        var output = binaryImage.CvtColor(ColorConversionCodes.GRAY2BGR);
        var random = new Random();

        // 距離値ごとに色を変えて描画する（パラレル処理）
        Parallel.For(0, (int) maxVal + 1, distance =>
        {
            var color = new Vec3b(
                (byte) random.Next(256),
                (byte) random.Next(256),
                (byte) random.Next(256));

            for (var y = 0; y < dist.Rows; y++)
            for (var x = 0; x < dist.Cols; x++)
            {
                if ((int) dist.At<float>(y, x) != distance) continue;
                output.Set(y, x, color);
            }
        });

        // 結果を表示する
        Cv2.ImShow("Color Coded Distance Map", output);
        Cv2.WaitKey(0);
    }
}