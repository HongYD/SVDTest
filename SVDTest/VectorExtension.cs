using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace SVDTest
{
    static class VectorExtensions
    {
        public static Vector<double> Real(this Vector<Complex> v)
        {
            return v.Map(x => x.Real);
        }
        public static Vector<double> Imag(this Vector<Complex> v)
        {
            return v.Map(x => x.Imaginary);
        }
    }
}
