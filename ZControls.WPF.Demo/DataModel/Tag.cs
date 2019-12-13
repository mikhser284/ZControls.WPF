using System;
using System.Text;

namespace ZControls.WPF.Demo.DataModel
{

    public class Tag : ITagTreeItem
    {
        public Boolean? IsSelected { get; set; }

        public String Name { get; set; }
        
        public ETagState State { get; set; }

        public ITagTreeItem Parent { get; set; }

        public Tag(String name, ETagState state = ETagState.Undefined)
        {
            Name = name;
            State = state;
            IsSelected = false;
        }
    }
}
