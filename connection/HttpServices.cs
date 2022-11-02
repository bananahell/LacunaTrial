using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace LacunaTrial {

  /// <summary> Class containing the communications with the servers and all the HTTP requests made. Basically the view of this project. </summary>
  class HttpServices {

    public static readonly string lacunaSite = "https://gene.lacuna.cc/";
    public static readonly string createUserPost = lacunaSite + "api/users/create";
    public static readonly string reqAccessTokPost = lacunaSite + "api/users/login";
    public static readonly string reqJobGet = lacunaSite + "api/dna/jobs";

    /// <summary> Sends a Json with all the user information to Lacuna's website. </summary>
    /// <param name="client"> HttpClient global to the program that makes the HTTP connection. </param>
    /// <param name="username"> User's username. </param>
    /// <param name="email"> User's email. </param>
    /// <param name="password"> User's password (ugh). </param>
    /// <returns> A DefaultResponse object containing a code for success and failure and a message explaining the conditions of a failure. </returns>
    public static async Task<DefaultResponse> SendUserData(HttpClient client, string username, string email, string password) {

      DefaultResponse userRes;
      HttpResponseMessage httpResMess;
      User user = new User() {
        username = username,
        email = email,
        password = password
      };

      httpResMess = await client.PostAsJsonAsync(createUserPost, user);
      userRes = await httpResMess.Content.ReadFromJsonAsync<DefaultResponse>() ?? throw new NullReferenceException("SendUserData Exception!");

      return userRes;

    }

    /// <summary> Sends a Json with a user's information asking for an access token. </summary>
    /// <param name="client"> HttpClient global to the program that makes the HTTP connection. </param>
    /// <param name="username"> User's username. </param>
    /// <param name="password"> User's password (ugh). </param>
    /// <returns> A TokenUserResponse object containing a code for success and failure, a message explaining the conditions of a failure, and a generated access token valid for 2 minutes. </returns>
    public static async Task<TokenUserResponse> GetAccessToken(HttpClient client, string username, string password) {

      TokenUserResponse tokUserRes;
      HttpResponseMessage httpResMess;
      TokenUser tokUser = new TokenUser() {
        username = username,
        password = password
      };

      httpResMess = await client.PostAsJsonAsync(reqAccessTokPost, tokUser);
      tokUserRes = await httpResMess.Content.ReadFromJsonAsync<TokenUserResponse>() ?? throw new NullReferenceException("GetAccessToken Exception!");

      return tokUserRes;

    }

    /// <summary> Sends a request for a random job using an access token. </summary>
    /// <param name="client"> HttpClient global to the program that makes the HTTP connection. </param>
    /// <param name="accessTok"> Access token acquired previously. Valid for 2 minutes starting from acquisition. </param>
    /// <returns> A JobRequestResponse object containing a code for success and failure, a message explaining the conditions of a failure, and a random job object. </returns>
    public static async Task<JobRequestResponse> GetJob(HttpClient client, string accessTok) {

      JobRequestResponse jobReqRes;
      HttpResponseMessage httpResMess;

      HttpRequestMessage httpReqMess = new HttpRequestMessage(HttpMethod.Get, reqJobGet);
      httpReqMess.Headers.Add("Authorization", "Bearer " + accessTok);
      httpResMess = await client.SendAsync(httpReqMess);
      jobReqRes = await httpResMess.Content.ReadFromJsonAsync<JobRequestResponse>() ?? throw new NullReferenceException("GetJob Exception!");

      return jobReqRes;

    }

    /// <summary> Sends a Json with the answer to a decode job. </summary>
    /// <param name="client"> HttpClient global to the program that makes the HTTP connection. </param>
    /// <param name="jobId"> Job's id acquired from the acquisition of the job and used in the url of the HTTP request. </param>
    /// <param name="accessTok"> Access token acquired previously. Valid for 2 minutes starting from acquisition. </param>
    /// <param name="strandAnswer"> The answer to the job - a DNA strand represented by a string of nucleobases A T C G received by the job as a string of binary base 64. </param>
    /// <returns> A DefaultResponse object containing a code for success and failure and a message explaining the conditions of a failure. </returns>
    public static async Task<DefaultResponse> SendDecodeJobAnswer(HttpClient client, string jobId, string accessTok, string strandAnswer) {

      DefaultResponse jobRes;
      HttpResponseMessage httpResMess = new HttpResponseMessage();
      DecodeJob decodeJob = new DecodeJob() {
        strand = strandAnswer
      };

      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTok);
      httpResMess = await client.PostAsJsonAsync(reqJobGet + "/" + jobId + "/decode", decodeJob);
      jobRes = await httpResMess.Content.ReadFromJsonAsync<DefaultResponse>() ?? throw new NullReferenceException("SendDecodeJobAnswer Exception!");

      return jobRes;

    }

    /// <summary> Sends a Json with the answer to an encode job. </summary>
    /// <param name="client"> HttpClient global to the program that makes the HTTP connection. </param>
    /// <param name="jobId"> Job's id acquired from the acquisition of the job and used in the url of the HTTP request. </param>
    /// <param name="accessTok"> Access token acquired previously. Valid for 2 minutes starting from acquisition. </param>
    /// <param name="strandEncodedAnswer"> The answer to the job - a DNA strand represented by a string of binary base 64 received by the job as a string of nucleobases A T C G. </param>
    /// <returns> A DefaultResponse object containing a code for success and failure and a message explaining the conditions of a failure. </returns>
    public static async Task<DefaultResponse> SendEncodeJobAnswer(HttpClient client, string jobId, string accessTok, string strandEncodedAnswer) {

      DefaultResponse jobRes;
      HttpResponseMessage httpResMess = new HttpResponseMessage();
      EncodeJob encodeJob = new EncodeJob() {
        strandEncoded = strandEncodedAnswer
      };

      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTok);
      httpResMess = await client.PostAsJsonAsync(reqJobGet + "/" + jobId + "/encode", encodeJob);
      jobRes = await httpResMess.Content.ReadFromJsonAsync<DefaultResponse>() ?? throw new NullReferenceException("SendEncodeJobAnswer Exception!");

      return jobRes;

    }

    /// <summary> Sends a Json with the answer to a check gene job. </summary>
    /// <param name="client"> HttpClient global to the program that makes the HTTP connection. </param>
    /// <param name="jobId"> Job's id acquired from the acquisition of the job and used in the url of the HTTP request. </param>
    /// <param name="accessTok"> Access token acquired previously. Valid for 2 minutes starting from acquisition. </param>
    /// <param name="isActivatedAnswer"> The answer to the job - whether if the gene received is activated in the DNA strand received. </param>
    /// <returns> A DefaultResponse object containing a code for success and failure and a message explaining the conditions of a failure. </returns>
    public static async Task<DefaultResponse> SendCheckGeneJobAnswer(HttpClient client, string jobId, string accessTok, bool isActivatedAnswer) {

      DefaultResponse jobRes;
      HttpResponseMessage httpResMess = new HttpResponseMessage();
      CheckGeneJob checkGeneJob = new CheckGeneJob() {
        isActivated = isActivatedAnswer
      };

      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTok);
      httpResMess = await client.PostAsJsonAsync(reqJobGet + "/" + jobId + "/gene", checkGeneJob);
      jobRes = await httpResMess.Content.ReadFromJsonAsync<DefaultResponse>() ?? throw new NullReferenceException("SendCheckGeneJobAnswer Exception!");

      return jobRes;

    }

  }

}
