namespace WebApplication1
{
    public class FileEl
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public FileEl(string Name, string Type)
        {
            this.Name = Name;
            this.Type = Type;
        }

    }
}