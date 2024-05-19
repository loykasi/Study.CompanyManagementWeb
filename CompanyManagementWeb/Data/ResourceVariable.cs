namespace CompanyManagementWeb.Data
{
    public enum ResourceEnum
    {
        None,
        Post,
        Schedule
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
                _ => string.Empty,
            };
        }
    }  
}