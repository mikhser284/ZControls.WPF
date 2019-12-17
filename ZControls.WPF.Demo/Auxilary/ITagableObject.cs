using System;
using System.Collections.Generic;
using System.Text;

namespace ZControls.WPF.Demo.Aux
{
    public interface ITagableObject
    {
        Boolean IsSelected { get; set; }

        HashSet<Int32> TagsIds { get; set; }
    }
}
