using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Dto.ApiResponse
{
    public class PageList<T>
    {
        #region CTRS
        public PageList()
        {
            DataList = new List<T>();
        }
        #endregion

        #region Prop
        public List<T> DataList { get; private set; }
        public int TotalCount { get; private set; }
        #endregion

        #region Methods
        public void SetResult(int totalCount, List<T> dataList)
        {
            TotalCount = totalCount;
            DataList = dataList;
        }
        #endregion
    }
}
