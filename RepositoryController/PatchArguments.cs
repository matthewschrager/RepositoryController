using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryController
{
    /// <summary>
    /// Contains information about how to patch a data record.
    /// </summary>
    public class PatchArguments
    {
        //===============================================================
        /// <summary>
        /// A JSON string describing the update to be performed. See full explanation at https://github.com/matthewschrager/RepositoryController.
        /// </summary>
        public String UpdateDescriptor { get; set; }
        //===============================================================
        /// <summary>
        /// A string describing the type of update to be performed. Should be either 'set' or 'add'.
        /// </summary>
        public String UpdateType { get; set; }
        //===============================================================
        /// <summary>
        /// Used when patching a property on a nested object. See full explanation at https://github.com/matthewschrager/RepositoryController.
        /// </summary>
        public String PathToProperty { get; set; }
        //===============================================================
    }
}
