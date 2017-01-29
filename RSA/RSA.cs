using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RSA
{
    static class RSA
    {
        public static Key KeyGenerator()
        {
           
            {
                key.E = (ulong) random.Next(3, (int) ((key.p - 1)*(key.q - 1) >> 1));
            } while (GCD(key.E, (key.p - 1)*(key.q - 1)) != 1);
            key.D = Inverse(key.E, (key.p - 1)*(key.q - 1));

            return key;
        }
        public static ulong GetPrime(Random random)
        {
            const int t = 45;
            bool BOOL;
            ulong p;

            do
            {
                BOOL = false;
                p = (ulong)random.Next(1, 65535);
                const int mask = 1;
                p |= 1;
                p |= (mask << 15);
                for (uint i = 3; i < 2000; i += 2)
                    if (p % i == 0)
                    {
                        BOOL = true;
                        break;
                    }
                if (!BOOL)
                    BOOL = Test(p, t);
            } while (BOOL);
            return p;
        }
        public static ulong GCD(ulong a, ulong b)
        {
            ulong g = 1;
            while (a % 2 == 0 && b % 2 == 0)
            {
                a /= 2;
                b /= 2;
                g *= 2;
            }
            while (a != 0)
            {
                while (a % 2 == 0)
                    a /= 2;
                while (b % 2 == 0)
                    b /= 2;
                if (a >= b)
                    a = (a - b) / 2;
                else
                    b = (b - a) / 2;
            }
            return g * b;
        }

        public static ulong PowMod(ulong a, ulong x, ulong n)
        {
            ulong base_ = 1;
            int w = 3;
            for (var i = 0; i < w; i++)
                base_ *= 2;
            var A = new ulong[base_];
            A[0] = 1;
            for (ulong i = 1; i < base_; i++)
                A[i] = (A[i - 1] * a) % n;
            ulong worker = x;
            int n_x = 1;
            while (worker > base_ - 1)
            {
                worker /= base_;
                n_x++;
            }
            var x_base = new ulong[n_x];
            for (var i = 0; i < n_x; i++)
            {
                x_base[i] = x % base_;
                x /= base_;
            }
            ulong m = 1;
            for (var i = n_x - 1; i >= 0; i--)
            {
                for (var j = 0; j < w; j++)
                {
                    m = (m * m) % n;
                }
                ulong f = x_base[i];
                m = (m * A[f]) % n;

            }
            return m;
        }

        public static bool Test(ulong p, int t)
        {
            var r = new Random();
            int m = 0;
            ulong s = p - 1;
            while (s % 2 == 0)
            {
                s /= 2;
                m++;
            }
            for (var i = 0; i < t; i++)
            {
                int check = 0;
                ulong a = (ulong)r.Next(2, (int)(p - 1));
                if (GCD(a, p) != 1)
                    return true;
                ulong b = PowMod(a, s, p);
                if (b == 1 || b == p - 1)
                    continue;
                for (var l = 1; l < m; l++)
                {
                    b = PowMod(b, 2, p);
                    if (b == p - 1)
                    {
                        check = 1;
                        break;
                    }
                }
                if (check == 0)
                    return true;
            }
            return false;
        }

        public static ulong Inverse(ulong _a, ulong _n)
        {
            long n = (long)_n;
            long a = (long)_a;
            long g = 1;
            long n1 = n;
            while ((a & 1) == 0 && (n & 1) == 0)
            {
                a >>= 1;
                n >>= 1;
                g <<= 1;
            }
            long u = a;
            long v = n;
            long A = 1;
            long B = 0;
            long C = 0;
            long D = 1;
            do
            {
                while ((u & 1) == 0)
                {
                    u >>= 1;
                    if ((A & 1) == 0 && (B & 1) == 0)
                    {
                        A >>= 1;
                        B >>= 1;
                    }
                    else
                    {
                        A = (A + n) >> 1;
                        B = (B - a) >> 1;
                    }
                }
                while ((v & 1) == 0)
                {
                    v >>= 1;
                    if ((C & 1) == 0 && (D & 1) == 0)
                    {
                        C >>= 1;
                        D >>= 1;
                    }
                    else
                    {
                        C = (C + n) >> 1;
                        D = (D - a) >> 1;
                    }
                }
                if (u >= v)
                {
                    u -= v;
                    A -= C;
                    B -= D;
                }
                else
                {
                    v -= u;
                    C -= A;
                    D -= B;
                }
            } while (u != 0);
            return (ulong)((C < 0) ? C + n1 : C);
        }

        public static Key KeyHacking(ulong n, ulong E)
        {
            ulong i = 3;
            var key = new Key();
            while (i <= n)
            {
                if (n%i == 0) break;
                i += 2;
            }

            key.p = i;
            key.q = n/key.p;
            key.D = Inverse(E, (key.p - 1)*(key.q - 1));
           
            return key;

        }

       public static void Encryption(ulong n, ulong E, string filename)
       {
           var br = new BinaryReader(File.Open(filename, FileMode.Open), Encoding.ASCII);
           var bw = new BinaryWriter(File.Open(filename + ".rsa", FileMode.Create));
            try
            {
                while (br.PeekChar() > -1)
                {
                    byte p = br.ReadByte();
                    ulong m1 = p;
                    ulong c = PowMod(m1, E, n);
                    var m = (uint)c;
                    bw.Write(m);
                }
            }
            catch (Exception exp)
            {
               MessageBox.Show(exp.Message);
            }
            br.Close();
            bw.Close();
            MessageBox.Show(@"Шифрування завершено");
        }

       public static void Decryption(ulong n, ulong D, string filename)
       {
           var br = new BinaryReader(File.Open(filename, FileMode.Open),Encoding.ASCII);
           var bw = new BinaryWriter(File.Open(filename.Replace(".rsa", ""), FileMode.Create));
           try
           {
               while (br.PeekChar() > -1) 
               {
                   uint c1 = br.ReadUInt32();
                   ulong c = c1;
                   ulong m = PowMod(c, D, n);
                   byte p = (byte)m;
                   bw.Write(p);
               }
           }
           catch (Exception exp)
           {
               MessageBox.Show(exp.Message);
           }
           br.Close();
           bw.Close();
           MessageBox.Show(@"Розшифрування файлу завершено");

       }

        public delegate void DUlongUlongString(ulong n, ulong E, string filename);

        public delegate void Dvoid();

        public delegate void DUlongUlong(ulong a, ulong b);

        public delegate void DKey(Key key);

        public delegate void DvoidInt(int a);
    }
}
