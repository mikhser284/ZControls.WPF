using System;
using System.Collections.Generic;
using System.Text;

namespace ZControls.WPF.Demo.DataModel
{
    public interface ITagTreeItem
    {
        String Name { get; }

        Boolean? IsSelected { get; }

        ITagTreeItem Parent { get; set; }
    }
}
