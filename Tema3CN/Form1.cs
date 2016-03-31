using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tema3CN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int n = 10;
        private double epsilon = 0.0000000001;

        private double[,] GimmeMatrixA(int n)
        {

            double[,] matrix = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                matrix[i, i] = 2 + 1/(double) (n*n);
                if (i < n - 1)
                    matrix[i, i + 1] = matrix[i + 1, i] = -1;
            }
            return matrix;
        }

        private double[] GimmeB(int n)
        {
            double[] v = new double[n];

            for (int i = 0; i < n; i++)
            {
                v[i] = 1/(double) (n*n);
            }

            return v;
        }

        private void PrintMatrix(double[,] matrix)
        {
            Console.WriteLine("MATRIX-------------------------MATRIX");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0,25} ", matrix[i, j]);
                }
                Console.Write('\n');
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[,] X = GimmeMatrixA(n);
            //PrintMatrix(X);
            double[] B = GimmeB(n);
            double modul = 0;
            int it = 0;
            do
            {
                it++;
                int p = 0, q = 0;
                double max = double.MinValue;
                double theta = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        if (Math.Abs(X[i, j]) > max)
                        {
                            max = Math.Abs(X[i, j]);
                            p = i;
                            q = j;
                        }
                    }
                }
                //Console.WriteLine("Maxim:" + max);
                if (X[p, p] == X[q, q])
                    theta = Math.PI/4;
                else
                    theta = 0.5*Math.Atan(2*X[p, q]/(X[p, p] - X[q, q]));

                double c = Math.Cos(theta);
                double s = Math.Sin(theta);

                double[,] Y = new double[n, n];
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (i != p && i != q && j != p && j != q)
                            Y[i, j] = X[i, j];

                for (int j = 0; j < n; j++)
                {
                    Y[p, j] = Y[j, p] = c*X[p, j] + s*X[q, j];
                    Y[q, j] = Y[j, q] = -s*X[p, j] + c*X[q, j];
                }
                Y[p, q] = Y[q, p] = 0;
                Y[p, p] = c*c*X[p, p] + 2*c*s*X[p, q] + s*s*X[q, q];
                Y[q, q] = s*s*X[p, p] - 2*c*s*X[p, q] + c*c*X[q, q];

                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        X[i, j] = Y[i, j];

                //Console.WriteLine("------START---------");
                modul = 0;
                //PrintMatrix(X);
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (i != j)
                        {
                            modul += X[i, j]*X[i, j];
                            //Console.WriteLine(X[i,j]);
                            //if (Math.Abs(X[i, j]) > 1) { }
                        }
                //Console.WriteLine("------END---------");
                modul = Math.Sqrt(modul);
                //Console.WriteLine(modul);
            } while (modul > epsilon);
            //PrintMatrix(X);
            listBox1.Items.Clear();
            double[] Z = new double[n];
            for (int i = 0; i < n; i++)
                Z[i] = X[i, i];
            var temp = Z.ToList();
            temp.Sort();
            foreach (double d in temp)
            {
                listBox1.Items.Add(d);
            }
            listBox1.Items.Add(it);
        }

        private void gauss(object sender, EventArgs e)
        {
            double p = int.Parse(textBox1.Text);
            double[,] A = GimmeMatrixA(n);
            //PrintMatrix(X);
            double[] B = GimmeB(n);
            double[] Z = new double[n];
            double n0 = 0;
            for (int k = 1; k < p; k++)
            {
                double na = 0;
                double[] X = new double[n];
                int it = 0;

                double sigma = (2/p)*k;
                do
                {
                    double[] Y = new double[n];
                    it++;
                    for (int i = 0; i < n; i++)
                    {
                        double sum1 = 0, sum2 = 0;
                        for (int j = 0; j < i; j++)
                            sum1 += A[i, j]*Y[j];
                        for (int j = i + 1; j < n; j++)
                            sum2 += A[i, j]*X[j];
                        Y[i] = (1 - sigma)*X[i] + (sigma/A[i, i])*(B[i] - sum1 - sum2);
                    }
                    double sum = 0;
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            sum += A[i, j]*(Y[i] - X[i])*(Y[j] - X[j]);
                        }
                    }
                    na = Math.Sqrt(sum);
                    for (int i = 0; i < n; i++)
                    {
                        X[i] = Y[i];
                    }

                } while (na > epsilon);

                if (k == 1)
                {
                    n0 = it;
                    for (int i = 0; i < n; i++)
                    {
                        Z[i] = X[i];
                    }
                }
                if (k > 1 && it < n0)
                {
                    n0 = it;
                    for (int i = 0; i < n; i++)
                    {
                        Z[i] = X[i];
                    }
                }

            }
            listBox1.Items.Clear();
            for (int i = 0; i < n; i++)
            {
                listBox1.Items.Add(Z[i]);
            }
            listBox1.Items.Add(n0);

        }

        private void jacobi(object sender, EventArgs e)
        {
            double p = int.Parse(textBox1.Text);
            double[,] A = GimmeMatrixA(n);
            //PrintMatrix(X);
            double[] B = GimmeB(n);
            double ni = 0;
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += Math.Abs(A[i, j]);
                }
                ni = Math.Max(ni, sum);
            }
            double l = 2/ni;
            double[] Z = new double[n];
            double n0 = 0;
            for (int k = 1; k < p; k++)
            {
                double na = 0;
                double[] X = new double[n];
                int it = 0;
                double sigma = (l/p)*k;
                double[,] bSigma = new double[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i == j)
                            bSigma[i, j] = 1 - sigma*A[i, i];
                        else
                            bSigma[i, j] = -sigma*A[i, j];
                    }
                }
                double[] bNormal = new double[n];
                for (int i = 0; i < n; i++)
                {
                    bNormal[i] = sigma*B[i];
                }

                do
                {
                    double[] Y = new double[n];
                    it++;
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            Y[i] += bSigma[i, j]*X[j];
                        }
                        Y[i] += bNormal[i];
                    }
                    double sum = 0;
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            sum += A[i, j]*(Y[i] - X[i])*(Y[j] - X[j]);
                        }
                    }
                    na = Math.Sqrt(sum);
                    for (int i = 0; i < n; i++)
                    {
                        X[i] = Y[i];
                    }

                } while (na > epsilon);

                if (k == 1)
                {
                    n0 = it;
                    for (int i = 0; i < n; i++)
                    {
                        Z[i] = X[i];
                    }
                }
                if (k > 1 && it < n0)
                {
                    n0 = it;
                    for (int i = 0; i < n; i++)
                    {
                        Z[i] = X[i];
                    }
                }

            }
            listBox1.Items.Clear();
            for (int i = 0; i < n; i++)
            {
                listBox1.Items.Add(Z[i]);
            }
            listBox1.Items.Add(n0);
        }

        private double Norma(double[] v)
        {
            double sum = 0;
            for (int i = 0; i < v.Length; i++)
            {
                sum += v[i]*v[i];
            }
            return Math.Sqrt(sum);
        }

        private double Norma(double[] v, double[] v2)
        {
            double sum = 0;
            for (int i = 0; i < v.Length; i++)
            {
                sum += v[i]*v2[i];
            }
            return sum;
        }

        private double[] MultiplyMatrixVector(double[,] matrix, double[] vector)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            double[] result=new double[rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i] += matrix[i,j]*vector[j];
                }
            }
            return result;
        }

        private double[] MultiplyVectorScalar(double[] vector, double scalar)
        {
            double[] result=new double[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                result[i] = vector[i]*scalar;
            }
            return result;
        }

        private double[] SumVectors(double[] a, double[] b)
        {
            double[] result = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = a[i] + b[i];
            }
            return result;
        }

        private double[] DifVectors(double[] a, double[] b)
        {
            double[] result = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = a[i] - b[i];
            }
            return result;
        }

        private void gradient(object sender, EventArgs e)
        {
            double[,] A = GimmeMatrixA(n);
            //PrintMatrix(X);
            double[] B = GimmeB(n);
            double[] X=new double[n];
            double[] r = DifVectors(B, MultiplyMatrixVector(A, X));
            double[] v = r;
            int it = 0;
            double er = 0;

            do
            {
                double alfa = (Norma(r)*Norma(r))/Norma(MultiplyMatrixVector(A, v), v);
                double[] newX = SumVectors(X, MultiplyVectorScalar(v, alfa));
                double[] newR = DifVectors(B, MultiplyMatrixVector(A, newX));
                double c = (Norma(newR)*Norma(newR))/(Norma(r)*Norma(r));
                double[] newV = SumVectors(newR, MultiplyVectorScalar(v, c));
                
                it++;
                er = Norma(DifVectors(newX, X));
                X = newX;
                r = newR;
                v = newV;
            } while (er>epsilon);

            listBox1.Items.Clear();
            for (int i = 0; i < n; i++)
            {
                listBox1.Items.Add(X[i]);
            }
            listBox1.Items.Add(it);
        }
    }
}
