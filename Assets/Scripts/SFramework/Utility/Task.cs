using UnityEngine;


namespace SFramework
{
    /// <summary>
    /// 存储任务信息，存在数据库
    /// </summary>
    public class Task
    {
        public string TaskName { get; set; }
        public string TaskPlace { get; set; }
        public int TaskAward { get; set; }

        public Task()
        {
            TaskName = "讨伐xxx";
            TaskPlace = "xx";
            TaskAward = 0;
        }
    }
}
