
namespace YeahTVApi.DomainModel
{
    public class DecryptionModel
    {

        public string PublicKey { get; set; }

        public string PraviteKey { get; set; }

        public EcryptionType ecryptionType { get; set; }

        public byte[] buffer { get; set; }
    }
}
