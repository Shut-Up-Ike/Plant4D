using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant4D
{

    public class Server
    {
        public string Description { get; private set; }
        public string Name { get; private set; }
        public ServerType ServerType { get; private set; }
        public P4DVersion P4DVersion { get; private set; }
        public Server(ServerType servertype, P4DVersion p4dversion) 
        {
            ServerType = servertype;
            P4DVersion = p4dversion;
            Description = $"Plant-4D {P4DVersion} {ServerType} Server";
        }
        public Server(ServerTypeAndP4DVersion servertypeAndp4dversion)
        {
            ServerType = BaseClass.GetServerTypeFromCombination(servertypeAndp4dversion);
            P4DVersion = BaseClass.GetP4DVersionFromCombination(servertypeAndp4dversion);
            Description = $"Plant-4D {P4DVersion} {ServerType} Server";
        }

        /// <summary>
        /// Copy constructor. Creates a copy of the passed in Server.
        /// </summary>
        /// <param name="oldServer">Server object to make a copy of</param>
        public Server(Server oldServer) : this(oldServer.ServerType, oldServer.P4DVersion)
        {
        }
    }
}
