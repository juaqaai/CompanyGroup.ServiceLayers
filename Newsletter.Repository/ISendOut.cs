using System;
using System.Collections.Generic;

namespace Newsletter.Repository
{
    /// <summary>
    /// kiküldendő repository
    /// </summary>
    public interface ISendOut
    {
        void SetHeader(int headerId, int status);

        void SetDetail(long id, int status, string note);

        List<Newsletter.Dto.Newsletter> GetList();
    }
}
