using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Helpers
{
    public class DynamicsConnector
    {
        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="language"></param>
        /// <param name="objectServer"></param>
        /// <param name="className"></param>
        public DynamicsConnector(string userName, string password, string domain, string dataAreaId, string language, string objectServer, string className)
        {
            _UserName = userName;
            _Password = password;
            _Domain = domain;
            _DataAreaId = dataAreaId;
            _Language = language;
            _ObjectServer = objectServer;
            _ClassName = className;
            _Axapta = new Microsoft.Dynamics.BusinessConnectorNet.Axapta();
        }

        #region "privat tagok"

        private bool _LoggedOn = false;

        private string _UserName = String.Empty;
        private string _Password = String.Empty;
        private string _Domain = String.Empty;

        private string _DataAreaId = String.Empty;
        private string _Language = String.Empty;
        private string _ObjectServer = String.Empty;
        private string _ClassName = String.Empty;

        private Microsoft.Dynamics.BusinessConnectorNet.AxaptaObject _AxaptaObject = null;

        private Microsoft.Dynamics.BusinessConnectorNet.Axapta _Axapta = null;

        #endregion

        #region "publikus jellemzok"

         /// <summary>
        /// vallalat kód
        /// </summary>
        public string DataAreaId
        {
            set { _DataAreaId = value; }
            get { return _DataAreaId; }
        }

        /// <summary>
        /// axapta metodus osztaly nev (WebControl)
        /// </summary>
        public string ClassName
        {
            set { this._ClassName = value; }
            get { return this._ClassName; }
        }

        /// <summary>
        /// bejelentkezés sikerült?
        /// </summary>
        public bool LoggedOn
        {
            get { return _LoggedOn; }
        }

        /// <summary>
        /// axapta object beallitasa, kiolvasasa
        /// </summary>
        public Microsoft.Dynamics.BusinessConnectorNet.AxaptaObject AxaptaObject
        {
            get { return this._AxaptaObject; }
        }

        #endregion

        /// <summary>
        /// kapcsolat inicializalasa, bejelentkezes 
        /// </summary>
        public void Connect()
        {
            //bejelentkezés még nem történt meg, vagy a business connector példány még nem jött létre
            if (!_LoggedOn)
            {
                try
                {
                    System.Net.NetworkCredential nc = new System.Net.NetworkCredential(_UserName, _Password, _Domain);

                    _Axapta.LogonAs(_UserName, _Domain, nc, _DataAreaId, _Language, _ObjectServer, null);

                    _LoggedOn = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// újrakapcolódás, bejelentkezés   
        /// </summary>
        public void ReConnect()
        {
            try
            {
                //kijelentkezés 
                if (_Axapta.Logoff())
                {
                    _Axapta.Dispose();

                    Connect();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// belso axapta object elkeszitese (webControl)
        /// </summary>
        public void CreateAxaptaObject()
        {
            if (String.IsNullOrEmpty(_ClassName))
            {
                throw new ArgumentException("ClassName argument not set");
            }
            _AxaptaObject = _Axapta.CreateAxaptaObject(_ClassName);
        }

        /// <summary>
        /// metodus hivasa parameterlistaval
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="xml"></param>
        public object CallMethod(string methodName, string xml)
        {
            if (_AxaptaObject == null)
            {
                CreateAxaptaObject();
            }
            if (String.IsNullOrEmpty(methodName) )
            {
                throw new ArgumentException("methodName argument not set");
            }
            if (String.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("xml argument not set");
            }
            return _AxaptaObject.Call(methodName, xml);
        }

        public object CallMethod(string methodName, long param)
        {
            if (_AxaptaObject == null)
            {
                CreateAxaptaObject();
            }
            if (String.IsNullOrEmpty(methodName))
            {
                throw new ArgumentException("methodName argument not set");
            }
            if (param.Equals(0))
            {
                throw new ArgumentException("param argument not set");
            }
            return _AxaptaObject.Call(methodName, param);
        }
        
        /// <summary>
        /// metodus hivasa parameterlista nelkul
        /// </summary>
        /// <param name="sMethodName"></param>
        public object CallMethod(string methodName)
        {
            if (_AxaptaObject == null)
            {
                CreateAxaptaObject();
            }
            if (String.IsNullOrEmpty(methodName))
            {
                throw new ArgumentException("methodName argument not set");
            }
            return _AxaptaObject.Call(methodName);
        }

        /// <summary>
        /// statikus osztálymetódus hívása
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public object CallStaticClassMethod(string className, string methodName, string xml)
        {
            if (String.IsNullOrEmpty(className))
            {
                throw new ArgumentException("className argument not set");
            }
            if (String.IsNullOrEmpty(methodName))
            {
                throw new ArgumentException("methodName argument not set");
            }
            if (String.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("xml argument not set");
            }
            return _Axapta.CallStaticClassMethod( className, methodName, xml );
        }

        /// <summary>
        /// kapcsolat bezarasa, eroforrasok felszabaditasa
        /// </summary>
        public void Disconnect()
        {
            try
            {
                _Axapta.Logoff();
                _Axapta.Dispose();
                _LoggedOn = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
