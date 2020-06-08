using FractalImageCoder.Entities;
using System;

namespace FractalImageCoder
{
    public static class RangeDomainRelation
    {
        private static int s_bits = 5;
        private static int o_bits = 7;
        private static int GREY_LEVELS = 255;
        private static double max_scale = 1.0;

        public static void ComputeRangeDomainRelationParameters(Block range, Block domain, int rdsum, out int scale, out int offset, out double error)
        {
            var sum1 = range.Size * range.Size;
            var rsum = range.Sum;
            var rsum2 = range.SumOfSquares;
            var dsum = domain.Sum;
            var dsum2 = domain.SumOfSquares;

            double alpha;
            double beta;

            /* compute the determinant */
            var det = sum1 * dsum2 - dsum * dsum;

            /* compute the scale */
            if (det == 0.0)
                alpha = 0.0;
            else
                alpha = (sum1 * rdsum - rsum * dsum) / det;

            /* Convert alpha to an integer */
            scale = (int)(0.5 + (alpha + max_scale) / (2.0 * max_scale) * (1 << s_bits));
            if (scale < 0) scale = 0;
            if (scale >= (1 << s_bits)) scale = (1 << s_bits) - 1;

            /* Now recompute alpha back */
            alpha = scale / (1 << s_bits) * (2.0 * max_scale) - max_scale;

            /* compute the offset */
            beta = (rsum - alpha * dsum) / sum1;

            /* Convert beta to an integer */
            /* we use the sign information of alpha to pack efficiently */
            if (alpha > 0.0)
                beta += alpha * GREY_LEVELS;
            offset = (int)(0.5 + beta / ((1.0 + Math.Abs(alpha)) * GREY_LEVELS) * ((1 << o_bits) - 1));
            if (offset < 0) offset = 0;
            if (offset >= (1 << o_bits)) offset = (1 << o_bits) - 1;

            /* Recompute beta from the integer */
            beta = offset / ((1 << o_bits) - 1) * ((1.0 + Math.Abs(alpha)) * GREY_LEVELS);
            if (alpha > 0.0)
                beta -= alpha * GREY_LEVELS;

            /* Compute the sqerr based on the quantized alpha and beta! */
            error = rsum2 + alpha * (alpha * dsum2 - 2.0 * rdsum + 2.0 * beta * dsum) + beta * (beta * sum1 - 2.0 * rsum);
        }
    }
}
