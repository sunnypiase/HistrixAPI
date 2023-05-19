namespace HistrixAPI.Models.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Api { get; set; }
        public string SecretKey { get; set; }
    }
}
