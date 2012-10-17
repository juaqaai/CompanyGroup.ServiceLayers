using System;
using System.Collections.Generic;

namespace Newsletter.Repository
{
    /// <summary>
    /// SendOut repository
    /// </summary>
    public class SendOut : Newsletter.Repository.RepositoryBase, Newsletter.Repository.ISendOut
    {
        /// <summary>
        /// kiküldés műveletek
        /// </summary>
        /// <param name="session"></param>
        public SendOut(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// [InternetUser].[SetNewsletterSendOutHeader]( @HeaderId int = 0, @Status int = 2, @Ret INT OUTPUT )
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <example>
        /// IQuery query = session.CreateSQLQuery("exec LogData @Time=:time, @Data=:data");
        ///query.SetDateTime("time", time);
        ///query.SetString("data", data);
        ///query.ExecuteUpdate();
        /// </example>
        public void SetHeader(int headerId, int status)
        {
            CompanyGroup.Helpers.DesignByContract.Require(headerId > 0, "headerId may not be null or empty");

            NHibernate.IQuery query = Session.CreateSQLQuery("exec InternetUser.SetNewsletterSendOutHeader ?, ?, ?")
                                            .SetInt32(0, headerId)
                                            .SetInt32(1, status)
                                            .SetInt32(2, 0);
            query.ExecuteUpdate();
        }

        /// <summary>
        /// [InternetUser].[SetNewsletterSendOutDetail]( @Id bigint = 0, @Status int = 2, @Note nvarchar(1024), @Ret int OUTPUT ) 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public void SetDetail(long id, int status, string note)
        {
            CompanyGroup.Helpers.DesignByContract.Require(id > 0, "id may not be null or empty");

            NHibernate.IQuery query = Session.CreateSQLQuery("InternetUser.SetNewsletterSendOutDetail ?, ?, ?, ?")
                                            .SetInt64(0, id)
                                            .SetInt32(1, status)
                                            .SetString(2, note)
                                            .SetInt32(3, 0);
            query.ExecuteUpdate();
        }

        /// <summary>
        /// [InternetUser].[GetNewsletterSendOut]
        /// </summary>
        /// <returns></returns>
        public List<Newsletter.Dto.Newsletter> GetList()
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.GetNewsletterSendOut")
                                            .SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(Newsletter.Dto.Newsletter).GetConstructors()[0]));

            return query.List<Newsletter.Dto.Newsletter>() as List<Newsletter.Dto.Newsletter>;        
        }

    }
}
