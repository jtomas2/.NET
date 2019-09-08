namespace Sabio.Models.Requests.Files
{
    public class FileUpdateRequest : FileAddRequest, IModelIdentifier
    {
        public int Id { get; set; }
    }
}