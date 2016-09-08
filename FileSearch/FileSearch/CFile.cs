using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSearch
{
    class CFile
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastChanged { get; set; }

        public CFile(){}

        public CFile(string name, long size, string type, DateTime creationtime, DateTime lastchanged)
        {
            Name = name;
            Size = size;
            Type = type;
            CreationDate = creationtime;
            LastChanged = lastchanged;
        }
    }
}
