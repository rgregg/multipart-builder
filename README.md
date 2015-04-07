# multipart-builder
A simple builder for creating multipart-related files. Since it can be hard to figure out
how to build a request that supports the multipart/related format, I build a little tool
to do it for you.

To generate a multipart/related request, build this sample and then exeucte the command
line app as follows:

```
multipartbuilder.exe "source-file-path" "remote-filename" ["optional rename behavior"]
```

This will combine the file at **source-file-path** together with OneDrive API metadata based on
the **remote-filename** and **optional rename behavior** argument into a single multipart/related
request that can be submitted using a POST request to api.onedrive.com:

```http
POST https://api.onedrive.com/v1.0/drive/root/children
Content-Type: multipart/related; boundary="A100x"

{output from the tool}
```
