namespace WarehouseInventoryManagementAPI.DTO.Account
{
    public class LogedInUser
    {
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; set; }

        public bool isLogged { get; set; }

        public LogedInUser()
        {
            UserEmail = "";
            UserFullName = "";
            UserId = "";
            UserName = "";
            isLogged = false;
        }
    }
}
