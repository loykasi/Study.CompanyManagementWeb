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

        public static string GetLocalizedName(string permission)
        {
            return permission switch
            {
                "None" => "Không có quyền",
                "Edit" => "Chỉnh sửa",
                "View" => "Xem",
                _ => string.Empty,
            };
        }
    }  
}