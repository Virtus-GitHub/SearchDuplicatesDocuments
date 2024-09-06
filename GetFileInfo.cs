namespace SearchDuplicatesDocument
{
    public static class GetFileInfo
    {
        public static string GetInformationDocuments() 
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory.Split("bin")[0];
            var finalPath = Path.Combine(basePath, "sample_input.txt");

            var information = File.ReadAllText(finalPath).Replace("\r\n", ";");

            string[] data = information.Split(';');

            return LookingForDuplucates.CheckingDocuments(data);
        }
    }
}
