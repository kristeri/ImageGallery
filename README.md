# ImageGallery
Image gallery demo using ASP.NET Core and Microsoft Azure for image hosting. The software requires Microsoft SQL Server to be installed.

# Usage

Right click ImageGallery and select "Manage User Secrets".

Add AzureStorageConnectionString as a key and use Azure Storage Account access key as a value in secrets.json.

```json
{
  "AzureStorageConnectionString": "YourConnectionStringHere"
}
```

Go to your Azure resource and in the blob service blobs section create a new container called "images". Select blob 
in the Public access level dropdown.

In Visual Studio, go to View -> Other Windows -> Package Manager Console  Run "update-database" in the Package Manager Console.

You can now run and test the app.

The data will be stored in (localdb)\MSSQLLocalDB and the image blob data into Azure.

