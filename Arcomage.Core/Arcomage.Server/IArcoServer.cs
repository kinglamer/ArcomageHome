using System.ServiceModel;

namespace Arcomage.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IArcoServer" in both code and config file together.
    [ServiceContract]
    public interface IArcoServer
    {
        [OperationContract]
        string GetRandomCard();
    }
}
