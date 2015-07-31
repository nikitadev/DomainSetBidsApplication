using System;
using System.Text;

namespace RegAPI.Library.Models
{
    public sealed partial class QuerySettings
    {
        public override string ToString()
        {
            var builder = new StringBuilder();

            if (!String.IsNullOrEmpty(InputData))
            {
                if (builder.Length != 0)
                {
                    builder.Append("&");
                }

                builder.AppendFormat("input_data={0}", InputData);
            }

            if (!String.IsNullOrEmpty(UserName))
            {
                if (builder.Length != 0)
                {
                    builder.Append("&");
                }

                builder.AppendFormat("username={0}", UserName);
            }

            if (!String.IsNullOrEmpty(Password))
            {
                if (builder.Length != 0)
                {
                    builder.Append("&");
                }

                builder.AppendFormat("password={0}", Password);
            }

            builder.Append(String.Concat("&input_format=json&output_content_type=plain"));
            return builder.ToString();
        }
    }
}
