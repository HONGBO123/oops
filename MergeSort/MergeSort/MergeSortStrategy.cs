// Kyler Little
// 11472421

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MergeSort
{
    public interface IMergeSortable<T> where T : IComparable
    {
        void Sort(List<T> arr);
    }

    public class MergeSort<T> : IMergeSortable<T> where T : IComparable
    {
        public void Sort(List<T> arr)
        {
            __Sort__(arr, 0, arr.Count - 1);
        }

        /// <summary>
        /// Classic recursive merge sort algorithm.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private void __Sort__(List<T> arr, int left, int right)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;
                __Sort__(arr, left, middle);
                __Sort__(arr, middle + 1, right);
                __Merge__(arr, left, middle, right);
            }
        }

        /// <summary>
        /// Grabbed this code from https://www.geeksforgeeks.org/merge-sort/; modified a few things for more C#-like
        /// syntax and more readable code.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="left"></param>
        /// <param name="middle"></param>
        /// <param name="right"></param>
        private void __Merge__(List<T> arr, int left, int middle, int right)
        {
            int i = 0, j = 0, k = 0;
            int n1 = middle - left + 1;             // size of left chunk we're going to merge
            int n2 = right - middle;                // size of right chunk we're going to merge 

            // Create temporary lists
            List<T> leftArr = new List<T>(n1), rightArr = new List<T>(n2);

            // Simply copy the data we're merging into these temporary lists
            for (i = 0; i < n1; i++)
                leftArr.Add(arr[left + i]);
            for (j = 0; j < n2; j++)
                rightArr.Add(arr[middle + 1 + j]);

            // Now, we merge the temp arrays back into arr[l..r]
            i = 0;              // Initial index of first subarray
            j = 0;              // Initial index of second subarray
            k = left;           // Initial index of merged subarray
            while (i < n1 && j < n2)
            {
                if (leftArr[i].CompareTo(rightArr[j]) <= 0)       // left element should precede the right element
                {
                    arr[k] = leftArr[i];
                    i++;
                }
                else            // right element should precede the left element since it's smaller
                {
                    arr[k] = rightArr[j];
                    j++;
                }
                k++;
            }

            // Copy the remaining elements of leftArr, if there are any
            while (i < n1)
            {
                arr[k] = leftArr[i];
                i++;
                k++;
            }

            // Copy the remaining elements of rightArr, if there are any
            while (j < n2)
            {
                arr[k] = rightArr[j];
                j++;
                k++;
            }
        }
    }

    public class ThreadedMergeSort<T> : IMergeSortable<T> where T : IComparable
    {
        public void Sort(List<T> arr)
        {
            __Sort__(arr, 0, arr.Count - 1);
        }

        /// <summary>
        /// Classic recursive merge sort algorithm.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private void __Sort__(List<T> arr, int left, int right)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;
                // Create new thread to sort left half
                Thread threadLeft = new Thread(() => __Sort__(arr, left, middle));
                threadLeft.Start();
                // Create new thread to sort right half
                Thread threadRight = new Thread(() => __Sort__(arr, middle + 1, right));
                threadRight.Start();
                // Once both threads are finished, merge
                threadLeft.Join();
                threadRight.Join();
                __Merge__(arr, left, middle, right);
                // Clean-up: kill the currently active threads since this recursive call is done
                threadLeft.Abort();
                threadRight.Abort();
            }
        }

        /// <summary>
        /// Grabbed this code from https://www.geeksforgeeks.org/merge-sort/; modified a few things for more C#-like
        /// syntax and more readable code.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="left"></param>
        /// <param name="middle"></param>
        /// <param name="right"></param>
        private void __Merge__(List<T> arr, int left, int middle, int right)
        {
            int i = 0, j = 0, k = 0;
            int n1 = middle - left + 1;             // size of left chunk we're going to merge
            int n2 = right - middle;                // size of right chunk we're going to merge 

            // Create temporary lists
            List<T> leftArr = new List<T>(n1), rightArr = new List<T>(n2);

            // Simply copy the data we're merging into these temporary lists
            for (i = 0; i < n1; i++)
                leftArr.Add(arr[left + i]);
            for (j = 0; j < n2; j++)
                rightArr.Add(arr[middle + 1 + j]);

            // Now, we merge the temp arrays back into arr[l..r]
            i = 0;              // Initial index of first subarray
            j = 0;              // Initial index of second subarray
            k = left;           // Initial index of merged subarray
            while (i < n1 && j < n2)
            {
                if (leftArr[i].CompareTo(rightArr[j]) <= 0)       // left element should precede the right element
                {
                    arr[k] = leftArr[i];
                    i++;
                }
                else            // right element should precede the left element since it's smaller
                {
                    arr[k] = rightArr[j];
                    j++;
                }
                k++;
            }

            // Copy the remaining elements of leftArr, if there are any
            while (i < n1)
            {
                arr[k] = leftArr[i];
                i++;
                k++;
            }

            // Copy the remaining elements of rightArr, if there are any
            while (j < n2)
            {
                arr[k] = rightArr[j];
                j++;
                k++;
            }
        }
    }
}
