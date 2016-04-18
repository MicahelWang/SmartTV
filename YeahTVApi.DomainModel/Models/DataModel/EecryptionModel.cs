using YeahTVApi.Entity;

namespace YeahTVApi.DomainModel.Models
{
    public class EcryptionModel
    {
        public IApiResult apiResult { get; set; }

        public ApiResultFormat apiResultFormat { get; set; }

        public string PublicKey { get; set; }

        public string PraviteKey { get; set; }

        public EcryptionType ecryptionType { get; set; }
    }
}
