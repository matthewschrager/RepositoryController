using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository;

namespace RepositoryController
{
    static class Utility
    {
        //===============================================================
        public static UpdateType ToUpdateType(String str)
        {
            switch (str.ToLower())
            {
                case "add":
                    return UpdateType.Add;
                case "set":
                    return UpdateType.Set;
                default:
                    throw new ArgumentException("Unexpected update type. Expected one of 'set' or 'add'");
            }
        }
        //===============================================================
    }
}
