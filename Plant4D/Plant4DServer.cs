using System;

namespace Plant4D
{
    //Copied from P4DHelperClass. Is this necessary?
    public class Plant4DServer
    {
        private string _description;

        private string _p4dtype;

        private string _servertype;

        public string Description
        {
            get
            {
                return _description;
            }
            private set
            {
                SetDescription();
            }
        }

        public string P4DType
        {
            get
            {
                return _p4dtype;
            }
            set
            {
                if (value.ToLower() == "rome" || value.ToLower() == "athena")
                {
                    _p4dtype = value;
                    SetDescription();
                    return;
                }

                _p4dtype = "";
                SetDescription();
                throw new ArgumentException("Acceptible values: ROME, ATHENA");
            }
        }

        public string ServerType
        {
            get
            {
                return _servertype;
            }
            set
            {
                if (value.ToLower() == "production" || value.ToLower() == "development")
                {
                    _servertype = value;
                    SetDescription();
                    return;
                }

                _servertype = "";
                SetDescription();
                throw new ArgumentException("Acceptible values: PRODUCTION, DEVELOPMENT");
            }
        }

        public string ServerName { get; set; }

        public Plant4DServer()
        {
        }

        public Plant4DServer(string serverType, string p4dType)
        {
            P4DType = p4dType;
            ServerType = serverType;
        }

        public Plant4DServer(string serverType, string p4dType, string servername)
        {
            P4DType = p4dType;
            ServerType = serverType;
            ServerName = servername;
        }

        private void SetDescription()
        {
            _description = _p4dtype + ":" + _servertype;
        }
    }
}
