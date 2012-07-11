using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryController
{
    public class PatchArguments<TKey>
    {
        //===============================================================
        public String UpdateDescriptor { get; set; }
        //===============================================================
        public String UpdateType { get; set; }
        //===============================================================
        public String PathToProperty { get; set; }
        //===============================================================
        public TKey Key { get; set; }
        //===============================================================
    }

    public class PatchArguments<TKey1, TKey2>
    {
        //===============================================================
        public String UpdateDescriptor { get; set; }
        //===============================================================
        public String UpdateType { get; set; }
        //===============================================================
        public String PathToProperty { get; set; }
        //===============================================================
        public TKey1 Key1 { get; set; }
        //===============================================================
        public TKey2 Key2 { get; set; }
        //===============================================================
    }
}
