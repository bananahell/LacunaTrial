namespace LacunaTrial {

  class _Main {

    static readonly HttpClient client = new HttpClient();

    // I will leave my information like this mumbo jumbo exactly because of you, beloved reader s2.
    // Running the code "in production" for me will be basically changing these fields into my actual fields.
    public const string username = "411gonep";
    public const string email = "cmcgb58912306345715@gmail.com";
    public const string password = "411daehgg";

    public const string decodeStrand = "DecodeStrand";
    public const string encodeStrand = "EncodeStrand";
    public const string checkGene = "CheckGene";

    // This is amount of jobs this program asks for. Be gentle with this number, since the jobs rely on connecting to the server, this tends to get very long with a high number.
    public const int jobsAmount = 20;

    /// <summary> The main in this project is acting basically as a controller for this project, establishing communication between the model (BiologyDepartment.cs) and the view (HttpServices.cs) </summary>
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
      int encodeJobs = 0;
      int decodeJobs = 0;
      int checkGeneJobs = 0;
      int badEncodeJobs = 0;
      int badDecodeJobs = 0;
      int badCheckGeneJobs = 0;

      try {

        for (int i = 0; i < jobsAmount; i++) {

          // Creating user.
          DefaultResponse userRes = await HttpServices.SendUserData(client, username, email, password);

          // Requesting access token.
          TokenUserResponse tokUserRes = await HttpServices.GetAccessToken(client, username, password);
          accessTok = tokUserRes.accessToken;

          // Getting job.
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
              decodeJobs++;
              break;
            case encodeStrand:
              strandEncodedAnswer = BiologyDepartment.EncodeStrand(strand);
              jobRes = await HttpServices.SendEncodeJobAnswer(client, jobId, accessTok, strandEncodedAnswer);
              encodeJobs++;
              break;
            case checkGene:
              isActivatedAnswer = BiologyDepartment.CheckGene(strandEncoded, geneEncoded);
              jobRes = await HttpServices.SendCheckGeneJobAnswer(client, jobId, accessTok, isActivatedAnswer);
              checkGeneJobs++;
              break;
            default:
              Console.WriteLine("I got the job, but the operation type seems off...");
              return;
          }

          if (jobRes.code != "Success") {
            switch (opType) {
              case decodeStrand:
                badDecodeJobs++;
                break;
              case encodeStrand:
                badEncodeJobs++;
                break;
              case checkGene:
                badCheckGeneJobs++;
                break;
            }
          }

        }

      } catch (Exception e) {
        Console.WriteLine("Returning exception!");
        Console.WriteLine(e);
      } finally {
        Console.WriteLine((encodeJobs + decodeJobs + checkGeneJobs) + " jobs completed");
        if (encodeJobs + decodeJobs + checkGeneJobs < jobsAmount) {
          Console.WriteLine((jobsAmount - encodeJobs - decodeJobs - checkGeneJobs) + " jobs not done!");
        } else {
          Console.WriteLine("All jobs completed!");
        }
        if (badDecodeJobs > 0) {
          Console.WriteLine(badDecodeJobs + " decode jobs failed...");
        }
        Console.WriteLine((decodeJobs - badDecodeJobs) + " decode jobs successful");
        if (badEncodeJobs > 0) {
          Console.WriteLine(badEncodeJobs + " encode jobs failed...");
        }
        Console.WriteLine((encodeJobs - badEncodeJobs) + " encode jobs successful");
        if (badCheckGeneJobs > 0) {
          Console.WriteLine(badCheckGeneJobs + " check gene jobs failed...");
        }
        Console.WriteLine((checkGeneJobs - badCheckGeneJobs) + " check gene jobs successful");
        if (badEncodeJobs + badDecodeJobs + badCheckGeneJobs == 0) {
          Console.WriteLine("All jobs successful!");
        }
      }

    }

  }

}
