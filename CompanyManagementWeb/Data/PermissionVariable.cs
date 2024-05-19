namespace CompanyManagementWeb.Data
{
    public enum PermissionEnum
    {
        None,
        Edit,
        View
    }
    
    public class PermissionVariable
    {
        public static string Get(PermissionEnum permissionEnum)
        {
            return permissionEnum switch
            {
                PermissionEnum.None => "None",
                PermissionEnum.Edit => "Edit",
                PermissionEnum.View => "View",
                _ => string.Empty,
            };
        }
    }  
}