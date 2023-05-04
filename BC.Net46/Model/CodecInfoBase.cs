using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC.Net46.Model
{
    public  class CodecInfoBase
    {
        public CodecInfoBase(string codec_name, string codec_long_name) 
        {
            CodecName = codec_name;
            CodecLongName = codec_long_name;
        }
        public string CodecName { get; set; }
        public string CodecLongName { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CodecInfoBase info &&
                   CodecName == info.CodecName;
        }

        public override int GetHashCode()
        {
            return 1040136710 + EqualityComparer<string>.Default.GetHashCode(CodecName);
        }
    }
}
