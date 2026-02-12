using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG_Tools.ClassesStructs
{
    public class PropertyClass
    {
        public class ClassPropertySet
        {
            struct InEngineValueTypes
            {
                public string? Name;
                public byte[]? Crc64Code;
            }

            public struct Props
            {
                public importProps imProps;
                public int block_length;
                public int count;
                public PropValues[] Values;
            }

            public struct ImProps
            {
                public int str_name_len;
                public string str_name;
            }

            public struct Values
            {
                public byte[] crc64_val_type;
                public int block_len_str_type;
                public int str_type_len;
                public string str_type;

                public string str_val;
            }

            public struct PropValues
            {
                public byte[] crc64_value;
                public int str_val_len;
                public string str_val; // value type

                public int count_values;
                public Values[] ValuesList;
            }

            public struct importProps
            {
                public int block_length;
                public int count;
                public ImProps[] ImportProps;
            }

            public Props Properties;
        }
    }
}
