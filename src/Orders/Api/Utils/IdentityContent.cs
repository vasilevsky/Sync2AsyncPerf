using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace API.Utils
{
    public class IdentityContent : HttpContent
    {
        private Type type = typeof(int);
        public const int IdentityBase = 100000000;
        private readonly int identity;
        private readonly MediaTypeFormatter formatter;

        public IdentityContent(int identity, MediaTypeFormatter formatter)
        {
            this.identity = identity;
            this.formatter = formatter;
            formatter.SetDefaultContentHeaders(type, Headers, null);
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await formatter.WriteToStreamAsync(type, IdentityBase + identity, stream, this, context);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 9;
            return true;
        }
    }
}