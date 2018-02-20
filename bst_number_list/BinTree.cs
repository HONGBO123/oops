using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bst_number_list
{
    public abstract class BinTree<T>
    {
        public abstract void Insert(T val);
        public abstract bool Contains(T val);
        public abstract void InOrder();
        public abstract void PreOrder();
        public abstract void PostOrder();
    }
}
