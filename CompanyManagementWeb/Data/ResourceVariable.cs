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
    }  
}