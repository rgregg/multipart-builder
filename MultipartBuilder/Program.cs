using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MultipartBuilder
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string pathToLocalFile;
            string destinationName;
            string renameBehavior = null;

            if (args.Length < 2)
            {
                Console.WriteLine("Usage: multipartbuilder.exe <path_to_local_file> <destination_file_name> [rename_behavior]");
                return;
            }

            pathToLocalFile = args[0];
            destinationName = args[1];
            if (args.Length > 2)
                renameBehavior = args[2];

            Dictionary<string, object> metadataObj = new Dictionary<string,object>();
            metadataObj["name"] = destinationName;
            metadataObj["file"] = new Object();
            if (!string.IsNullOrEmpty(renameBehavior))
                metadataObj["@name.conflictBehavior"] = renameBehavior;
            metadataObj["@content.sourceUrl"] = "cid:content";

            string metadata = JsonConvert.SerializeObject(metadataObj);

            MultipartRelatedBuilder builder = new MultipartRelatedBuilder();
            builder.Parts.Add(new MultipartContent { ContentId = "<metadata>", ContentType = "application/json", TextContent = metadata });

            FileStream sourceFileStream = File.OpenRead(pathToLocalFile);
            builder.Parts.Add(new MultipartContent { ContentId = "<content>", ContentType = "application/octet-stream", StreamContent = sourceFileStream });

            builder.WriteMultipartRelatedToFile("multipart-related.bin");

            Console.WriteLine("Exported multipart/related request to 'multipart-related.bin'.");
            Console.WriteLine("HTTP Headers: ");
            Console.WriteLine("Content-Type: " + builder.ContentType);

        }
    }
}
