using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransForm
{
    class Program
    {
        static void Main(string[] args)
        {
            Point3D[] ptsrc = new Point3D[4];
            Point3D[] ptdst = new Point3D[4];
            Point3D pttest = new Point3D() { X = 400, Y = 240 };

            for (int i = 0; i < 4; i++)
            {
                ptsrc[i] = new Point3D();
                ptdst[i] = new Point3D();
            }

            ptsrc[0].X = 1;
            ptsrc[0].Y = 1;
            ptsrc[1].X = 1;
            ptsrc[1].Y = 480;
            ptsrc[2].X = 640;
            ptsrc[2].Y = 97;
            ptsrc[3].X = 640;
            ptsrc[3].Y = 384;

            ptdst[0].X = 1;
            ptdst[0].Y = 1;
            ptdst[1].X = 1;
            ptdst[1].Y = 480;
            ptdst[2].X = 640;
            ptdst[2].Y = 1;
            ptdst[3].X = 640;
            ptdst[3].Y = 480;

            PerspectiveWarper pw = new PerspectiveWarper();
            bool rtn = pw.SolveCoeff(ptsrc, ptdst);

            Point3D pt = pw.Transform(pttest);

            return;
        }
    }
}
