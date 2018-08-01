/****************************************************************
 * 文件名称：PerspectiveWarper.cs
 * 实现功能：透视变换类
 * 创建时间：2018-08-02
 * 作    者：陈祎
 * 说    明：
 * 修改时间：
 * 修 改 人：
 * 修改说明：
 * **************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransForm //ScanLibrary
{
    /// <summary>
    /// 透视变换类
    /// </summary>
    public class PerspectiveWarper
    {
        #region 接口
        /// <summary>
        /// 求解变换矩阵
        /// </summary>
        /// <param name="ctrlPts"></param>
        /// <returns></returns>
        public bool SolveCoeff(Point3D[] src, Point3D[] dst)
        {
            solved = false;
            if (src == null || src.Length < 4 || dst == null || dst.Length < 4)
            {
                return false;
            }
            getPerspectiveTransform(src, dst, coeff);
            solved = true;
            return true;
        }

        /// <summary>
        /// 透视变换
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Point3D Transform(Point3D p)
        {
            if (!solved) return null;
            Point3D ret = Transform(p, coeff);
            return ret;
        }
        #endregion

        #region 参数
        double[] coeff = new double[16];
        bool solved = false;
        #endregion

        #region 计算透视变换
        void Gauss(double[,] A, int equ, int var, double[] ans)
        { //epu:A‘s row  var:A‘s col-1
            int row, col;
            for (row = 0, col = 0; col < var && row < equ; col++, row++)
            {
                int max_r = row;
                for (int i = row + 1; i < equ; i++)
                {
                    if ((1e-12) < Math.Abs(A[i, col]) - Math.Abs(A[max_r, col]))
                    {
                        max_r = i;
                    }
                }
                if (max_r != row)
                    for (int j = 0; j < var + 1; j++)
                    {
                        //swap(A[row, j], A[max_r, j]);
                        double t = A[row, j];
                        A[row, j] = A[max_r, j];
                        A[max_r, j] = t;
                        
                    }
                for (int i = row + 1; i < equ; i++)
                {
                    if (Math.Abs(A[i, col]) < (1e-12))
                        continue;
                    double tmp = -A[i, col] / A[row, col];
                    for (int j = col; j < var + 1; j++)
                    {
                        A[i, j] += tmp * A[row, j];
                    }
                }

            }
            for (int i = var - 1; i >= 0; i--)
            { //计算唯一解。
                double tmp = 0;
                for (int j = i + 1; j < var; j++)
                {
                    tmp += A[i, j] * (ans[j]);
                }
                ans[i] = (A[i, var] - tmp) / A[i, i];
            }
        }

        void getPerspectiveTransform(Point3D[] src, Point3D[] dst, double[] ret)
        {
            double x0 = src[0].X, x1 = src[1].X, x2 = src[3].X, x3 = src[2].X;
            double y0 = src[0].Y, y1 = src[1].Y, y2 = src[3].Y, y3 = src[2].Y;
            double u0 = dst[0].X, u1 = dst[1].X, u2 = dst[3].X, u3 = dst[2].X;
            double v0 = dst[0].Y, v1 = dst[1].Y, v2 = dst[3].Y, v3 = dst[2].Y;
            double[,] A = new double[8, 9]{
		    { x0, y0, 1, 0, 0, 0, -x0*u0, -y0*u0, u0 },
		    { x1, y1, 1, 0, 0, 0, -x1*u1, -y1*u1, u1 },
		    { x2, y2, 1, 0, 0, 0, -x2*u2, -y2*u2, u2 },
		    { x3, y3, 1, 0, 0, 0, -x3*u3, -y3*u3, u3 },
		    { 0, 0, 0, x0, y0, 1, -x0*v0, -y0*v0, v0 },
		    { 0, 0, 0, x1, y1, 1, -x1*v1, -y1*v1, v1 },
		    { 0, 0, 0, x2, y2, 1, -x2*v2, -y2*v2, v2 },
		    { 0, 0, 0, x3, y3, 1, -x3*v3, -y3*v3, v3 },
	    };
            double[] ans = new double[8];
            Gauss(A, 8, 8, ans);

            ret[0] = ans[0]; ret[1] = ans[1]; ret[2] = 0; ret[3] = ans[2];
            ret[4] = ans[3]; ret[5] = ans[4]; ret[6] = 0; ret[7] = ans[5];
            ret[8] = 0; ret[9] = 0; ret[10] = 1; ret[11] = 0;
            ret[12] = ans[6]; ret[13] = ans[7]; ret[14] = 0; ret[15] = 1;
        }

        Point3D Transform(Point3D p, double[] mat)
        {
            Point3D ret = new Point3D();
            double D = p.X * mat[12] + p.Y * mat[13] + mat[15];
            ret.X = (p.X * mat[0] + p.Y * mat[1] + mat[3]) / D;
            ret.Y = (p.X * mat[4] + p.Y * mat[5] + mat[7]) / D;
            return ret;
        }
        #endregion
    }
}
