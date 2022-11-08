using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ThuPhi.Model.Receive;

namespace ThuPhi.Resources
{
    static class ExtentionHelper
    {
        public static ObservableCollection<Info> AddByOrder(this ObservableCollection<Info> infos, Info i)
        {
            for (int x = 0; x < infos.Count; x++)
            {
                var compare = string.Compare(infos[x].Name, i.Name, StringComparison.CurrentCulture);
                if (compare > 0)
                {
                    infos.Insert(x, i);
                    return infos;
                }    
            }

            return infos;
        }

    }
}
