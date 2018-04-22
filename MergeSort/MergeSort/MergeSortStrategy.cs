using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
        }
    }

    public class ThreadedMergeSort<T> : IMergeSortable<T> where T : IComparable
    {
        public void Sort(List<T> arr)
        {

        }
    }
}
