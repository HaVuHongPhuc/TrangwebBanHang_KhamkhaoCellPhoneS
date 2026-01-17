using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace training
{
    internal class Program
    {
        static void hoanvi(ref int a, ref int b) //ref vs out
        {
            int temp = a;
            a = b;
            b = temp;
        }

        static void InterChangeSort(int[]arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1 ; j < arr.Length; j++)
                {
                    if (arr[i] > arr[j])
                    {
                        hoanvi(ref arr[i], ref arr[j]);
                    }    
                }    
            }    
        }

        static void SelectionSort(int[] arr)
        {
            int vtmin;
            for (int  i = 0;  i < arr.Length;  i++)
            {
                vtmin = i;
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[j] < arr[vtmin])
                        vtmin = j;
                      
                }  
                hoanvi(ref arr[i], ref arr[vtmin]);
            }
        }

        //Để đó
        static void InsertionSort(int[] arr)
        {
            int x, pos;
            for (int i = 1; i < arr.Length; i++)
            {
                x = arr[i];
                pos = i - 1;
                while (pos >= 0 && arr[pos] > x)
                {
                    arr[pos + 1] = arr[pos];
                    pos--;
                }
                arr[pos + 1] = x;
            }    
        }
        static void BublleSort(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr.Length -1 - i; j++)
                {
                    if (arr[j] > arr[j +1])
                    {
                        hoanvi(ref arr[j], ref arr[j + 1]);
                    }    
                }    
            }    
        }

        static int LinearSearach(int[]arr ,int x)// TÌm kiếm tuyến tính
        {
            for (int i = 0; i < arr.Length ; i++ )
            {
                if (arr[i] == x)
                {
                    Console.WriteLine("Found at index " + i);
                }    
            }
            return -1;
        }

        static int BinarySearch(int[]arr , int x)
        {
            int left = 0;
            int right = arr.Length - 1;
            while(left <= right)
            {
                int mid = (right + left) / 2;
                //mid = left + (right - left) / 2;
                if (arr[mid] == x)
                {
                    Console.WriteLine("Found at index " + mid);
                    break;
                }    
                if (arr[mid] < x)
                {
                    left = mid + 1;
                }    
                if (arr[mid] > x)
                {
                    right = mid - 1;
                }
            }
            return -1;
        }
        static void Main(string[] args)
        {
            Random rnd = new Random();
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            //Khởi tạo kích thước mảng
            Console.Write("Nhập kích thước của mảng: ");
            int n = int.Parse(Console.ReadLine());
            int[] arr = new int[n];
            for (int i = 0; i < n; i++) // 0 -> 3 
            {               
                arr[i] = rnd.Next(0, 100);
                Console.WriteLine($"a[{i}] = " + arr[i]);
            }
            //InterChangeSort(arr);
            //BublleSort(arr);
            //SelectionSort(arr);
            //Xuất mảng
            InsertionSort(arr);
            Console.Write("Mảng sau khi sắp xếp: ");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
            }    
            Console.WriteLine();
        }
    }
}
