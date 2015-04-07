using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace MultipartBuilder
{
    public class MultipartRelatedBuilder
    {

        public string Boundary { get; set; }

        public string ContentType { get { return string.Format("multipart/related; boundary=\"{0}\"", Boundary); } }

        public List<MultipartContent> Parts { get; private set; }

        public MultipartRelatedBuilder()
        {
            Boundary = "A100x";
            Parts = new List<MultipartContent>();
        }

        public void WriteMultipartRelatedToFile(string outputFile)
        {
            Stream outputStream = File.OpenWrite(outputFile);
            var encoding = new System.Text.UTF8Encoding(false);
            StreamWriter writer = new StreamWriter(outputStream, encoding, 4096, true);

            foreach (MultipartContent content in Parts)
            {
                writer.Write("--");
                writer.WriteLine(Boundary);

                // Headers
                writer.Write(content.GetHeaderText());
                writer.Flush();

                // Body
                if (content.StreamContent != null)
                {
                    content.StreamContent.CopyTo(outputStream);
                }
                else if (content.TextContent != null)
                {
                    writer.Write(content.TextContent);
                }
                writer.WriteLine();
                writer.WriteLine();
            }

            writer.Write("--");
            writer.Write(Boundary);
            writer.WriteLine("--");

            writer.Flush();
            writer.Close();
            outputStream.Close();
        }

    }


    public class MultipartContent 
    {
        public string ContentId {get;set;}
        public string ContentType {get;set;}

        public Dictionary<string, string> Headers {get; private set;}

        public string TextContent { get; set; }

        public Stream StreamContent { get; set; }

        public MultipartContent()
        {
            Headers = new Dictionary<string, string>();
        }

        public string GetHeaderText()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(ContentId))
                sb.AppendLine("Content-ID: " + ContentId);
            if (!string.IsNullOrEmpty(ContentType))
                sb.AppendLine("Content-Type: " + ContentType);
            foreach (var header in Headers)
            {
                sb.AppendLine(header.Key + ": " + header.Value);
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}

