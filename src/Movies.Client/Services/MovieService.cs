using Common;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Extensions;
using Movies.Client.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Movies.Client.Services
{
    public class MovieService : IMovieService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MovieService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            var movieList = await Get<List<Movie>>(Url.Movies);

            return movieList;
        }

        public async Task<Movie> GetMovie(int id)
        {
            var movie = await Get<Movie>(string.Format(Url.Movies_Id, id));

            return movie;
        }

        public async Task<bool> CreateMovie(Movie movie)
        {
            await Execute<Movie>(HttpMethod.Post, Url.Movies, movie);

            return true;
        }

        public async Task<bool> UpdateMovie(int id, Movie movie)
        {
            await Execute<Movie>(HttpMethod.Put, string.Format(Url.Movies_Id, id), movie);

            return true;
        }

        public async Task<bool> DeleteMovie(int id)
        {
            await Execute<Movie>(HttpMethod.Delete, string.Format(Url.Movies_Id, id), default);

            return true;
        }

        public async Task<UserInfoViewModel> GetUserInfo()
        {
            var idpClient = _httpClientFactory.CreateClient(Constant.Http_Client_Idp);

            var metaDataResponse = await idpClient.GetDiscoveryDocumentAsync();

            if (metaDataResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while requesting the access token");
            }

            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var userInfoResponse = await idpClient.GetUserInfoAsync(
               new UserInfoRequest
               {
                   Address = metaDataResponse.UserInfoEndpoint,
                   Token = accessToken
               });

            if (userInfoResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while getting user info");
            }

            var userInfoDictionary = new Dictionary<string, string>();

            foreach (var claim in userInfoResponse.Claims)
            {
                userInfoDictionary.Add(claim.Type, claim.Value);
            }

            return new UserInfoViewModel(userInfoDictionary);
        }

        private async Task<T> Get<T>(string uri)
        {
            return await Execute<T>(HttpMethod.Get, uri, default);
        }

        private async Task<T> Execute<T>(HttpMethod method, string uri, T data)
        {
            var httpClient = _httpClientFactory.CreateClient(Constant.Http_Client_Movies_Api);

            var request = new HttpRequestMessage(method, uri);

            if (method == HttpMethod.Post || method == HttpMethod.Put)
            {
                request.SerializeData<T>(data);
            }

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return await response.ReadContentAs<T>();
        }
    }
}
