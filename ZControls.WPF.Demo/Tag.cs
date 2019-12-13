using System;
using System.Collections.Generic;
using System.Text;

namespace ZControls.WPF.Demo
{
    public enum TagState
    {
        Exclude = -1,
        Undefined = 0,
        Include = 1,

    }

    public class TagsFolder
    {
        public String Name { get; set; }

        public List<Tag> Tags { get; set; }

        
    }

    public class Tag
    {
        public String Name { get; set; }

        
        public TagState State { get; set; }

    }
}
