using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace LacunaTrial {

  class HttpServices {

    public static readonly string lacunaSite = "https://gene.lacuna.cc/";
    public static readonly string createUserPost = lacunaSite + "api/users/create";
    public static readonly string reqAccessTokPost = lacunaSite + "api/users/login";
    public static readonly string reqJobGet = lacunaSite + "api/dna/jobs";

    /**
     * Sends a Json with all the user information to Lacuna's website.
     * @param client HttpClient global to the program that makes the HTTP connection.
     * @param username User's username.
     * @param email User's email.
     * @param password User's password (ugh).
     * @return POST response containing a code variable stating Success or Failure and a message variable explaining errors.
     */

    /// <summary> Sends a Json with all the user information to Lacuna's website. </summary>
    /// <param name="client"> HttpClient global to the program that makes the HTTP connection. </param>
    /// <param>
    ///
    /// </param>
    /// <param>
    ///
    /// </param>
    /// <param>
    ///
    /// </param>
    public static async Task<DefaultResponse> SendUserData(HttpClient client, string username, string email, string password) {

      DefaultResponse userRes;
      HttpResponseMessage httpResMess;
      User user = new User() {
        username = username,
        email = email,
        password = password
      };

      try {
        httpResMess = await client.PostAsJsonAsync(createUserPost, user);
        userRes = await httpResMess.Content.ReadFromJsonAsync<DefaultResponse>();
      } catch (Exception exception) {
        throw exception;
      }

      return userRes;

    }

    public static async Task<TokenUserResponse> GetAccessToken(HttpClient client, string username, string password) {

      TokenUserResponse tokUserRes;
      HttpResponseMessage httpResMess;
      TokenUser tokUser = new TokenUser() {
        username = username,
        password = password
      };

      try {
        httpResMess = await client.PostAsJsonAsync(reqAccessTokPost, tokUser);
        tokUserRes = await httpResMess.Content.ReadFromJsonAsync<TokenUserResponse>();
      } catch (Exception exception) {
        throw exception;
      }

      return tokUserRes;

    }

    public static async Task<JobRequestResponse> GetJob(HttpClient client, string accessTok) {

      JobRequestResponse jobReqRes;
      HttpResponseMessage httpResMess;

      try {
        HttpRequestMessage httpReqMess = new HttpRequestMessage(HttpMethod.Get, reqJobGet);
        httpReqMess.Headers.Add("Authorization", "Bearer " + accessTok);
        httpResMess = await client.SendAsync(httpReqMess);
        jobReqRes = await httpResMess.Content.ReadFromJsonAsync<JobRequestResponse>();
      } catch (Exception exception) {
        throw exception;
      }

      return jobReqRes;

    }

    public static async Task<DefaultResponse> SendDecodeJobAnswer(HttpClient client, string jobId, string accessTok, string strandAnswer) {

      DefaultResponse jobRes;
      HttpResponseMessage httpResMess = new HttpResponseMessage();
      DecodeJob decodeJob = new DecodeJob() {
        strand = strandAnswer
      };

      try {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTok);
        httpResMess = await client.PostAsJsonAsync(reqJobGet + "/" + jobId + "/decode", decodeJob);
        jobRes = await httpResMess.Content.ReadFromJsonAsync<DefaultResponse>();
      } catch (Exception exception) {
        throw exception;
      }

      return jobRes;

    }

    public static async Task<DefaultResponse> SendEncodeJobAnswer(HttpClient client, string jobId, string accessTok, string strandEncodedAnswer) {

      DefaultResponse jobRes;
      HttpResponseMessage httpResMess = new HttpResponseMessage();
      EncodeJob encodeJob = new EncodeJob() {
        strandEncoded = strandEncodedAnswer
      };

      try {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTok);
        httpResMess = await client.PostAsJsonAsync(reqJobGet + "/" + jobId + "/encode", encodeJob);
        jobRes = await httpResMess.Content.ReadFromJsonAsync<DefaultResponse>();
      } catch (Exception exception) {
        throw exception;
      }

      return jobRes;

    }

    public static async Task<DefaultResponse> SendCheckGeneJobAnswer(HttpClient client, string jobId, string accessTok, bool isActivatedAnswer) {

      DefaultResponse jobRes;
      HttpResponseMessage httpResMess = new HttpResponseMessage();
      CheckGeneJob checkGeneJob = new CheckGeneJob() {
        isActivated = isActivatedAnswer
      };

      try {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTok);
        httpResMess = await client.PostAsJsonAsync(reqJobGet + "/" + jobId + "/gene", checkGeneJob);
        jobRes = await httpResMess.Content.ReadFromJsonAsync<DefaultResponse>();
      } catch (Exception exception) {
        throw exception;
      }

      return jobRes;

    }

  }

}
