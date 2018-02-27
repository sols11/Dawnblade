using UnityEngine;

namespace SFramework
{
    /// <summary>
    /// 存储任务完成相关信息，存在存档里
    /// </summary>
    public class TaskData
    {
        public int Id { get; set; } // 对应在任务数据库的下标
        public bool IsActive { get; set; }

        public bool HasCompleted
        {
            get { return HasCompletedRank1 || HasCompletedRank2 || HasCompletedRank3; }
        }
        public bool HasCompletedRank1 { get; set; }
        public bool HasCompletedRank2 { get; set; }
        public bool HasCompletedRank3 { get; set; }
        public bool OpenRank3 { get; set; }

        /// <summary>
        /// 添加新任务时，默认的构造
        /// </summary>
        public TaskData()
        {
            Id = 0;
            IsActive = true;
        }
    }
}
