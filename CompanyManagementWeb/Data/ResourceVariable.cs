namespace CompanyManagementWeb.Data
{
    public enum ResourceEnum
    {
        None,
        Post,
        Schedule,
        PostCategory,
        Department,
        Role,
        Member
    }

    public class ResourceVariable
    {
        public static string Get(ResourceEnum resourceEnum)
        {
            return resourceEnum switch
            {
                ResourceEnum.None => "None",
                ResourceEnum.Post => "Post",
                ResourceEnum.Schedule => "Schedule",
                ResourceEnum.PostCategory => "PostCategory",
                ResourceEnum.Department => "Department",
                ResourceEnum.Role => "Role",
                ResourceEnum.Member => "Member",
                _ => string.Empty,
            };
        }

        public static string GetLocalizedName(string resource)
        {
            return resource switch
            {
                "None" => "None",
                "Post" => "Bài đăng",
                "Schedule" => "Lịch trình",
                "PostCategory" => "Danh mục",
                "Department" => "Phòng ban",
                "Role" => "Vai trò",
                "Member" => "Thành viên",
                _ => string.Empty,
            };
        }
    }  
}