using Challenge.DataContracts;
using Challenge.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Challenge
{
    public class ChallengeClient
    {
        private readonly string teamSecret;
        private readonly string baseUrl = "https://task-challenge.azurewebsites.net";
        private readonly HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Создает клиента к API платформы соревнований
        /// </summary>
        /// <param name="teamSecret">Секрет команды для авторизации запросов</param>
        public ChallengeClient(string teamSecret)
        {
            this.teamSecret = teamSecret;
        }

        /// <summary>
        /// Получает информацию о челлендже, доступную команде
        /// </summary>
        /// <param name="challengeId">Идентификатор челленджа</param>
        /// <returns>Информация о челлендже</returns>
        public async Task<ChallengeResponse> GetChallengeAsync(string challengeId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("secret", teamSecret);

            var url = new UriBuilder(baseUrl);
            url.Path = $"/api/challenges/{HttpUtility.UrlEncode(challengeId)}";
            url.Query = query.ToString();

            var response = await httpClient.GetAsync(url.ToString());

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ChallengeResponse>();
            throw ErrorResponseException.ExtractFrom(response);
        }

        /// <summary>
        /// Запрашивает новое задание некоторого типа в некотором раунде для команды
        /// </summary>
        /// <param name="round">Идентификатор раунда</param>
        /// <param name="type">Тип задачи. Если не передать, будет выбрана задача случайного типа</param>
        /// <returns>Полученное задание</returns>
        public async Task<TaskResponse> AskNewTaskAsync(string round, string type = null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("secret", teamSecret);
            query.Add("round", round);
            if (type != null)
                query.Add("type", type);

            var url = new UriBuilder(baseUrl);
            url.Path = $"/api/tasks";
            url.Query = query.ToString();

            var response = await httpClient.PostAsync(url.ToString(), null);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<TaskResponse>();
            throw ErrorResponseException.ExtractFrom(response);
        }

        /// <summary>
        /// Отправляет ответ на задание
        /// </summary>
        /// <param name="taskId">Идентификатор задания</param>
        /// <returns>Новое состояние задания, на которое давался ответ</returns>
        public async Task<TaskResponse> CheckTaskAnswerAsync(string taskId, string answer)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("secret", teamSecret);

            var url = new UriBuilder(baseUrl);
            url.Path = $"/api/tasks/{HttpUtility.UrlEncode(taskId)}";
            url.Query = query.ToString();

            var content = new AnswerRequest { Answer = answer }.SerializeToJsonContent();
            var response = await httpClient.PostAsync(url.ToString(), content);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<TaskResponse>();
            throw ErrorResponseException.ExtractFrom(response);
        }

        /// <summary>
        /// Получает все задания, запрошенные командой
        /// </summary>
        /// <returns>Список заданий</returns>
        public async Task<List<TaskResponse>> GetAllTasksAsync()
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("secret", teamSecret);

            var url = new UriBuilder(baseUrl);
            url.Path = $"/api/tasks";
            url.Query = query.ToString();

            var response = await httpClient.GetAsync(url.ToString());

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<List<TaskResponse>>();
            throw ErrorResponseException.ExtractFrom(response);
        }

        /// <summary>
        /// Получает задание по идентификатору
        /// </summary>
        /// <param name="taskId">Идентификатор задания</param>
        /// <returns>Запрошенное задание</returns>
        public async Task<TaskResponse> GetTask(string taskId)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("secret", teamSecret);

            var url = new UriBuilder(baseUrl);
            url.Path = $"/api/tasks/{HttpUtility.UrlEncode(taskId)}";
            url.Query = query.ToString();

            var response = await httpClient.GetAsync(url.ToString());

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<TaskResponse>();
            throw ErrorResponseException.ExtractFrom(response);
        }
    }
}
