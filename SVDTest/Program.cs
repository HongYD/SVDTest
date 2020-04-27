using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;


namespace SVDTest
{
    class Program
    {
        static Matrix<double> U;
        static Matrix<double> M;
        static Matrix<double> E;
        static double[] H = { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0 };//Input

        static void Main(string[] args)
        {
            //1.创建一个3*3矩阵M作为SVD算法的输入
            M = Matrix<double>.Build.Dense(3, 3, H);
            Console.WriteLine("Origin Matrix M:");
            Console.WriteLine(M.ToString());
            //2.寻找(M*MT)T 的 eigenvectors 和eigenvalues:
            Matrix<double> tempMatrix = (M.Multiply(M.Transpose())).Transpose();
            Evd<double> eigen = tempMatrix.Evd();
            Matrix<double> eigenvectorTemp=eigen.EigenVectors;
            Vector<Complex> eigenvalueTemp = eigen.EigenValues;
            Vector<double> eigenvalueDoubleTemp;
            eigenvalueDoubleTemp = eigenvalueTemp.Real();
            IEnumerable<Vector<double>>ColumnsArrayTemp = eigenvectorTemp.EnumerateColumns();
            List<Vector<double>> ColumnsArray = new List<Vector<double>>();
            foreach(var value in ColumnsArrayTemp)
            {
                ColumnsArray.Add(value);
            }
            double[] eigenvalueDouble = eigenvalueDoubleTemp.Enumerate().ToArray<double>();
            //得出VT：
            Console.WriteLine("VT:");
            Console.WriteLine(eigenvectorTemp.ToString());
            //4.得出Σ：
            Vector<double> a = Vector<double>.Build.DenseOfArray(new double[] { eigenvalueDouble[0],0,0,0 });
            Vector<double> b = Vector<double>.Build.DenseOfArray(new double[] { 0, eigenvalueDouble[1], 0,0 });
            Vector<double> c = Vector<double>.Build.DenseOfArray(new double[] { 0, 0, eigenvalueDouble[2],0 });
            E = Matrix<double>.Build.DenseOfColumnVectors(a,b,c);
            Console.WriteLine("Σ：");
            Console.WriteLine(E.ToString());
            //5.求出M的NullSpace和其magnitude：
            Vector<double>[] nullspaceVector = M.Kernel();
            double[] nullspace=new double[3];
            double magnitude=0.0;
            nullspace = nullspaceVector[0].Enumerate().ToArray<double>();
            for(int i=0;i<nullspace.Length;i++)
            {                
                magnitude += (nullspace[i]*nullspace[i]);
            }                   
            magnitude = Math.Sqrt(magnitude);
            //6.知道所有数值后，根据公式求出U：
            var part1 = M.Multiply((1 / Math.Sqrt(eigenvalueDouble[0]))).Multiply(ColumnsArray[0]);
            var part2= M.Multiply((1 / Math.Sqrt(eigenvalueDouble[1]))).Multiply(ColumnsArray[1]);
            var part3 = M.Multiply((1 / Math.Sqrt(eigenvalueDouble[2]))).Multiply(ColumnsArray[2]);
            for (int k = 0; k < nullspace.Length; k++)
            {
                nullspace[k] /= magnitude;
            }
            Vector<double> part4= Vector<double>.Build.DenseOfArray(nullspace);
            U =Matrix<double>.Build.DenseOfColumnVectors(part1,part2,part3,part4);
            Console.WriteLine("U:");
            Console.WriteLine(U.ToString());
        }
    }
}
