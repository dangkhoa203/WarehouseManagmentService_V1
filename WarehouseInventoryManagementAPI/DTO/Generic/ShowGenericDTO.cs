namespace WarehouseInventoryManagementAPI.DTO.Generic
{
    public abstract class ShowGenericDTO
    {
        protected ShowGenericDTO()
        {
        }

        protected ShowGenericDTO(string id, string name, DateTime createdate)
        {
            Id = id;
            Name = name;
            CreateDate = createdate;
        }

        public string Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreateDate { get; set; }

       
    }
}
