namespace zhou.Models.DBModels
{
    public class Menu : BaseEntity
    {
        /// <summary>
        /// 父级菜单ID
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 菜单Name,通常为视图文件名或者视图所在文件夹名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单地址
        /// </summary>
        public string Jump { get; set; }

        /// <summary>
        /// 菜单图标Icon
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 是否默认展开：1=true，2=false
        /// </summary>
        public int Spread { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
