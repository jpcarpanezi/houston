using System.Text.Json.Serialization;

namespace Houston.Core.Models.GitHub.Base
{
    public class Sender
    {
        [JsonPropertyName("login")]
        public string Login { get; }

        [JsonPropertyName("id")]
        public int Id { get; }

        [JsonPropertyName("node_id")]
        public string NodeId { get; }

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; }

        [JsonPropertyName("gravatar_id")]
        public string GravatarId { get; }

        [JsonPropertyName("url")]
        public string Url { get; }

        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; }

        [JsonPropertyName("followers_url")]
        public string FollowersUrl { get; }

        [JsonPropertyName("following_url")]
        public string FollowingUrl { get; }

        [JsonPropertyName("gists_url")]
        public string GistsUrl { get; }

        [JsonPropertyName("starred_url")]
        public string StarredUrl { get; }

        [JsonPropertyName("subscriptions_url")]
        public string SubscriptionsUrl { get; }

        [JsonPropertyName("organizations_url")]
        public string OrganizationsUrl { get; }

        [JsonPropertyName("repos_url")]
        public string ReposUrl { get; }

        [JsonPropertyName("events_url")]
        public string EventsUrl { get; }

        [JsonPropertyName("received_events_url")]
        public string ReceivedEventsUrl { get; }

        [JsonPropertyName("type")]
        public string Type { get; }

        [JsonPropertyName("site_admin")]
        public bool SiteAdmin { get; }

        [JsonConstructor]
        public Sender(string login, int id, string nodeId, string avatarUrl, string gravatarId, string url, string htmlUrl, string followersUrl, string followingUrl, string gistsUrl, string starredUrl, string subscriptionsUrl, string organizationsUrl, string reposUrl, string eventsUrl, string receivedEventsUrl, string type, bool siteAdmin)
        {
            Login = login ?? throw new ArgumentNullException(nameof(login));
            Id = id;
            NodeId = nodeId ?? throw new ArgumentNullException(nameof(nodeId));
            AvatarUrl = avatarUrl ?? throw new ArgumentNullException(nameof(avatarUrl));
            GravatarId = gravatarId ?? throw new ArgumentNullException(nameof(gravatarId));
            Url = url ?? throw new ArgumentNullException(nameof(url));
            HtmlUrl = htmlUrl ?? throw new ArgumentNullException(nameof(htmlUrl));
            FollowersUrl = followersUrl ?? throw new ArgumentNullException(nameof(followersUrl));
            FollowingUrl = followingUrl ?? throw new ArgumentNullException(nameof(followingUrl));
            GistsUrl = gistsUrl ?? throw new ArgumentNullException(nameof(gistsUrl));
            StarredUrl = starredUrl ?? throw new ArgumentNullException(nameof(starredUrl));
            SubscriptionsUrl = subscriptionsUrl ?? throw new ArgumentNullException(nameof(subscriptionsUrl));
            OrganizationsUrl = organizationsUrl ?? throw new ArgumentNullException(nameof(organizationsUrl));
            ReposUrl = reposUrl ?? throw new ArgumentNullException(nameof(reposUrl));
            EventsUrl = eventsUrl ?? throw new ArgumentNullException(nameof(eventsUrl));
            ReceivedEventsUrl = receivedEventsUrl ?? throw new ArgumentNullException(nameof(receivedEventsUrl));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            SiteAdmin = siteAdmin;
        }
    }
}
