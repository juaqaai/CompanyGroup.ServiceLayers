using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.DataAccess
{
    public class BaseRepository
    {

        private DataAccess.SqlServer.Entities context;

        public BaseRepository(DataAccess.SqlServer.Entities context)
        {
            this.context = context;
        }

        protected DataAccess.SqlServer.Entities Context
        {
            get { return this.context; }
        }
    }
}
