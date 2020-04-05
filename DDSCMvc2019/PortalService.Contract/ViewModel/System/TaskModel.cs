using System;
using System.Collections.Generic;

namespace PortalService.Contract.ViewModel
{
    [Serializable]
    public class TaskModel
    {

        /// <summary>
        /// 絕對位置
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 工作名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 目前狀態
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 觸發程序
        /// </summary>
        public string Triggers { get; set; }

        /// <summary>
        /// 下次執行時間
        /// </summary>
        public DateTime NextRunTime { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string FolderPath { get; set; }
        
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 觸發程序列表
        /// </summary>
        public List<string> TriggerList { get; set; } = new List<string>();

        /// <summary>
        /// 動作列表
        /// </summary>
        public List<string> ActionList { get; set; } = new List<string>();
    }
}
