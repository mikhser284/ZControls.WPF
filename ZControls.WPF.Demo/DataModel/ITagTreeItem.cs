using System;
using System.Collections.Generic;
using System.Text;

namespace ZControls.WPF.Demo.DataModel
{
    public interface ITagTreeItem
    {
        String Name { get; set; }

        Boolean? IsSelected { get; set; }

        ITagTreeItem Parent { get; set; }
    }
}
