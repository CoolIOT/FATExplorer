using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATExplorer
{
    /*
     * Container class for ComboBox Items
     */
    public class ComboBoxItem
    {
        public ComboBoxItem(string text, object value)
        {
            this.text = text;
            this.value = value;
        }

        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        private object value;

        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public override string ToString()
        {
            return text;
        }
    }
}
