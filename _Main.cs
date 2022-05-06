namespace LacunaTrial {

  class _Main {

    static readonly HttpClient client = new HttpClient();

    public const string username = "411gonep";
    public const string email = "cmcgb58912306345715@gmail.com";
    public const string password = "411daehgg";

    public const string decodeStrand = "DecodeStrand";
    public const string encodeStrand = "EncodeStrand";
    public const string checkGene = "CheckGene";

    public static async Task Main(string[] args) {

      string accessTok;
      string opType;
      string strandEncoded;
      string strandAnswer;
      string strand;
      string strandEncodedAnswer;
      string jobId;
      string geneEncoded;
      bool isActivatedAnswer;

      // Creating user
      DefaultResponse userRes = await HttpServices.SendUserData(client, username, email, password);

      // Requesting access token
      TokenUserResponse tokUserRes = await HttpServices.GetAccessToken(client, username, password);
      accessTok = tokUserRes.accessToken;

      // Getting job
      JobRequestResponse jobReqRes = await HttpServices.GetJob(client, accessTok);
      jobId = jobReqRes.job.id;
      opType = jobReqRes.job.type;
      strandEncoded = jobReqRes.job.strandEncoded;
      strand = jobReqRes.job.strand;
      geneEncoded = jobReqRes.job.geneEncoded;

      // Determining which job I got, doing it, and sending it
      DefaultResponse jobRes;
      switch (opType) {
        case decodeStrand:
          strandAnswer = BiologyDepartment.DecodeStrand(strandEncoded);
          jobRes = await HttpServices.SendDecodeJobAnswer(client, jobId, accessTok, strandAnswer);
          break;
        case encodeStrand:
          strandEncodedAnswer = BiologyDepartment.EncodeStrand(strand);
          jobRes = await HttpServices.SendEncodeJobAnswer(client, jobId, accessTok, strandEncodedAnswer);
          break;
        case checkGene:
          isActivatedAnswer = BiologyDepartment.CheckGene(strandEncoded, geneEncoded);
          jobRes = await HttpServices.SendCheckGeneJobAnswer(client, jobId, accessTok, isActivatedAnswer);
          break;
        default:
          Console.WriteLine("I got the job, but the operation type seems off...");
          return;
      }

      Console.WriteLine("opType: {0}", opType);
      Console.WriteLine("code: {0}", jobRes.code);
      Console.WriteLine("message: {0}", jobRes.message);
      Console.WriteLine("gg!");

    }

  }

}
