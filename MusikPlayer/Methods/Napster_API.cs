using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MusikPlayer.Methods
{

    public class Napster_BandJsonHandler
    {
        public struct All
        {
            public meta meta { get; set; }
            public search search { get; set; }
        }

        public struct meta
        {
            public int totalCount { get; set; }
            public int returnedCount { get; set; }
        }

        public struct search
        {
            public string query { get; set; }
            public data Data { get; set; }
            //public order order { get; set; }
        }

        public struct data
        {
            public albums[] albums { get; set; }
            public Artist[] artist { get; set; }
            public Track[] track { get; set; }
            public Playlist[] playlist { get; set; }

        }
        public struct order
        {
            public string[] orders { get; set; }
        }

        public struct albums
        {
            public string type { get; set; }
            public string id { get; set; }
            public string upc { get; set; }
            public string shortcut { get; set; }
            public string href { get; set; }
            public string name { get; set; }
            public string released { get; set; }
            public string originallyReleased { get; set; }
            public string label { get; set; }
            public string copyright { get; set; }
            public string[] tags { get; set; }
            public int discCount { get; set; }
            public int trackCount { get; set; }
            public bool isExplicit { get; set; }
            public bool isSingle { get; set; }
            public string accountPartner { get; set; }
            public string artistName { get; set; }
            public bool isAvailableInHiRes { get; set; }
            public ContributingArtists contributingArtists { get; set; }
            public Discographies discographies { get; set; }
            public Links links { get; set; }
            public bool isStreamable { get; set; }



        }
        public struct ContributingArtists
        {
            public string primaryArtist { get; set; }

        }
        public struct Discographies
        {
            public string[] main { get; set; }
            public string[] others { get; set; }

        }
        public struct Links
        {
            public Images images { get; set; }
            public Tracks tracks { get; set; }
            public Posts posts { get; set; }
            public Artists artists { get; set; }
            public Genres genres { get; set; }

            public struct Images
            {
                public string href { get; set; }
            }
            public struct Tracks
            {
                public string href { get; set; }
            }
            public struct Posts
            {
                public string href { get; set; }
            }
            public struct Artists
            {
                public string[] ids { get; set; }
                public string href { get; set; }
            }
            public struct Genres
            {
                public string[] ids { get; set; }
                public string href { get; set; }
            }
        }

        public struct Artist
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("href")]
            public Uri Href { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("shortcut")]
            public string Shortcut { get; set; }

            [JsonProperty("amg")]
            public long Amg { get; set; }

            [JsonProperty("blurbs")]
            public List<object> Blurbs { get; set; }

            [JsonProperty("albumGroups")]
            public Discographies AlbumGroups { get; set; }

            [JsonProperty("links")]
            public Links Links { get; set; }
        }
        public struct Track
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("index")]
            public long Index { get; set; }

            [JsonProperty("disc")]
            public long Disc { get; set; }

            [JsonProperty("href")]
            public Uri Href { get; set; }

            [JsonProperty("playbackSeconds")]
            public long PlaybackSeconds { get; set; }

            [JsonProperty("isExplicit")]
            public bool IsExplicit { get; set; }

            [JsonProperty("isStreamable")]
            public bool IsStreamable { get; set; }

            [JsonProperty("isAvailableInHiRes")]
            public bool IsAvailableInHiRes { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("isrc")]
            public string Isrc { get; set; }

            [JsonProperty("shortcut")]
            public string Shortcut { get; set; }

            [JsonProperty("blurbs")]
            public List<object> Blurbs { get; set; }

            [JsonProperty("artistId")]
            public string ArtistId { get; set; }

            [JsonProperty("artistName")]
            public string ArtistName { get; set; }

            [JsonProperty("albumName")]
            public string AlbumName { get; set; }

            [JsonProperty("formats")]
            public List<string> Formats { get; set; }

            [JsonProperty("losslessFormats")]
            public List<string> LosslessFormats { get; set; }

            [JsonProperty("albumId")]
            public string AlbumId { get; set; }

            [JsonProperty("contributors")]
            public string Contributors { get; set; }

            [JsonProperty("links")]
            public string Links { get; set; }

            [JsonProperty("previewURL")]
            public string PreviewUrl { get; set; }
        }
        public struct Playlist
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("modified")]
            public string Modified { get; set; }

            [JsonProperty("href")]
            public Uri Href { get; set; }

            [JsonProperty("trackCount")]
            public long TrackCount { get; set; }

            [JsonProperty("privacy")]
            public string Privacy { get; set; }

            [JsonProperty("images")]
            public List<Image> Images { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("favoriteCount")]
            public long FavoriteCount { get; set; }

            [JsonProperty("freePlayCompliant")]
            public bool FreePlayCompliant { get; set; }

            [JsonProperty("links")]
            public string Links { get; set; }
        }
    }

    public class Napster_AlbumJsonHandler
    {
        public struct All
        {
            public Meta Meta { get; set; }

            public Track[] Tracks { get; set; }
        }

        public struct Meta
        {
            [JsonProperty("returnedCount")]
            public int ReturnedCount { get; set; }

            [JsonProperty("totalCount")]
            public int? TotalCount { get; set; }
        }

        public struct Track
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("index")]
            public int Index { get; set; }

            [JsonProperty("disc")]
            public int Disc { get; set; }

            [JsonProperty("href")]
            public string Href { get; set; }

            [JsonProperty("playbackSeconds")]
            public int PlaybackSeconds { get; set; }

            [JsonProperty("isExplicit")]
            public bool IsExplicit { get; set; }

            [JsonProperty("isStreamable")]
            public bool IsStreamable { get; set; }

            [JsonProperty("isAvailableInHiRes")]
            public bool IsAvailableInHiRes { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("isrc")]
            public string Isrc { get; set; }

            [JsonProperty("shortcut")]
            public string Shortcut { get; set; }

            [JsonProperty("blurbs")]
            public string[] Blurbs { get; set; }

            [JsonProperty("artistId")]
            public string ArtistId { get; set; }

            [JsonProperty("artistName")]
            public string ArtistName { get; set; }

            [JsonProperty("albumName")]
            public string AlbumName { get; set; }

            [JsonProperty("formats")]
            public Format[] Formats { get; set; }

            [JsonProperty("losslessFormats")]
            public Format[] LosslessFormats { get; set; }

            [JsonProperty("albumId")]
            public string AlbumId { get; set; }

            [JsonProperty("contributors")]
            public Contributors Contributors { get; set; }

            [JsonProperty("links")]
            public Links Links { get; set; }

            [JsonProperty("previewURL")]
            public string PreviewUrl { get; set; }
        }

        public struct Contributors
        {
            [JsonProperty("primaryArtist")]
            public string PrimaryArtist { get; set; }

            [JsonProperty("featuredPerformer")]
            public string FeaturedPerformer { get; set; }

            [JsonProperty("guestVocals")]
            public string GuestVocals { get; set; }

            [JsonProperty("guestMusician")]
            public string GuestMusician { get; set; }

            [JsonProperty("remixer")]
            public string Remixer { get; set; }

            [JsonProperty("producer")]
            public string Producer { get; set; }

            [JsonProperty("engineer")]
            public string Engineer { get; set; }

            [JsonProperty("conductor", NullValueHandling = NullValueHandling.Ignore)]
            public string Conductor { get; set; }

            [JsonProperty("composer")]
            public string Composer { get; set; }
        }

        public struct Format
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("bitrate")]
            public long Bitrate { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("sampleBits")]
            public long SampleBits { get; set; }

            [JsonProperty("sampleRate")]
            public long SampleRate { get; set; }
        }

        public struct Links
        {
            public Artists artists { get; set; }
            public Albums albums { get; set; }
            public Composers composers { get; set; }
            public Genres genres { get; set; }
            public Tags tags { get; set; }

            public struct Artists
            {
                public string[] ids { get; set; }
                public string href { get; set; }
            }
            public struct Albums
            {
                public string[] ids { get; set; }
                public string href { get; set; }
            }
            public struct Composers
            {
                public string[] ids { get; set; }
                public string href { get; set; }
            }
            public struct Genres
            {
                public string[] ids { get; set; }
                public string href { get; set; }
            }
            public struct Tags
            {
                public string[] ids { get; set; }
                public string href { get; set; }
            }
        }

    }


    public class Napster_FilteredJsonHandler
    {
        public class All
        {
            [JsonProperty("meta")]
            public Meta Meta { get; set; }

            [JsonProperty("search")]
            public Search Search { get; set; }
        }

        public class Meta
        {
            [JsonProperty("totalCount")]
            public long TotalCount { get; set; }

            [JsonProperty("returnedCount")]
            public long ReturnedCount { get; set; }
        }

        public class Search
        {
            [JsonProperty("query")]
            public string Query { get; set; }

            [JsonProperty("data")]
            public Data Data { get; set; }

            [JsonProperty("order")]
            public string[] Order { get; set; }
        }

        public class Data
        {
            [JsonProperty("artists")]
            public Artist[] Artists { get; set; }
        }

        public class Artist
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("href")]
            public string Href { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("shortcut")]
            public string Shortcut { get; set; }

            [JsonProperty("amg")]
            public string Amg { get; set; }

            [JsonProperty("blurbs")]
            public object[] Blurbs { get; set; }

            [JsonProperty("albumGroups")]
            public AlbumGroups AlbumGroups { get; set; }

            [JsonProperty("links")]
            public Links Links { get; set; }
        }

        public class AlbumGroups
        {
            [JsonProperty("singlesAndEPs", NullValueHandling = NullValueHandling.Ignore)]
            public string[] SinglesAndEPs { get; set; }

            [JsonProperty("others", NullValueHandling = NullValueHandling.Ignore)]
            public string[] Others { get; set; }

            [JsonProperty("compilations", NullValueHandling = NullValueHandling.Ignore)]
            public string[] Compilations { get; set; }

            [JsonProperty("main", NullValueHandling = NullValueHandling.Ignore)]
            public string[] Main { get; set; }
        }

        public class Links
        {
            [JsonProperty("albums")]
            public Albums Albums { get; set; }

            [JsonProperty("images")]
            public Albums Images { get; set; }

            [JsonProperty("posts")]
            public Albums Posts { get; set; }

            [JsonProperty("topTracks")]
            public Albums TopTracks { get; set; }

            [JsonProperty("genres", NullValueHandling = NullValueHandling.Ignore)]
            public Genres Genres { get; set; }

            [JsonProperty("stations")]
            public Genres Stations { get; set; }
        }

        public class Albums
        {
            [JsonProperty("href")]
            public string Href { get; set; }
        }

        public class Genres
        {
            [JsonProperty("ids")]
            public string[] Ids { get; set; }

            [JsonProperty("href")]
            public string Href { get; set; }
        }

    }



    public class Napster_API
    {
        static string API_KEY = "ZGNlMzg0ZTYtZjQyYS00NWQ1LWI1OTEtZmVmYzRkZDhlYjAx";
        static List<string> AlbumIDs = new List<string>();

        public static string BandInfoLinkByName = ""; //$"https://api.napster.com/v2.2/albums/alb.203610935/tracks?apikey=ZGNlMzg0ZTYtZjQyYS00NWQ1LWI1OTEtZmVmYzRkZDhlYjAx";


        public static string AlbumDetailsLinkByID = "";
        public static Size PictureSize = new Size(300, 300);
        public static string CoverURL = ""; //$"https://api.napster.com/imageserver/v2/albums/{AlbumID}/images/{PictureSize.Width}x{PictureSize.Height}.jpg";
        WebClient webClient = new WebClient();

        public string JsonText = "";
        public string dataTest = "";

        public Napster_AlbumJsonHandler.All GetAlbumData(int i, Napster_BandJsonHandler.All data)
        {
            string AlbumID = data.search.Data.albums[i].id;
            string AlbumDetailsLinkByID = $"https://api.napster.com/v2.2/albums/{AlbumID}/tracks?apikey={API_KEY}";
            string AlbumJsonText = JObject.Parse(webClient.DownloadString(AlbumDetailsLinkByID)).ToString();
            Napster_AlbumJsonHandler.All AlbumDate = JsonConvert.DeserializeObject<Napster_AlbumJsonHandler.All>(AlbumJsonText);
            return AlbumDate;
        }

        public void GetAllInfo(string BandName)
        {
            JsonText = webClient.DownloadString($"http://api.napster.com/v2.2/search/verbose?apikey=YTkxZTRhNzAtODdlNy00ZjMzLTg0MWItOTc0NmZmNjU4Yzk4&query={BandName}");
            JsonText = JObject.Parse(JsonText).ToString();
            Napster_BandJsonHandler.All data = JsonConvert.DeserializeObject<Napster_BandJsonHandler.All>(JsonText);

            for (int i = 0; i < data.search.Data.albums.Length; i++)
            {
                for (int j = 0; j < GetAlbumData(i, data).Tracks.Length; j++)
                {
                    Console.WriteLine(GetAlbumData(i, data).Tracks[j].Name);
                }
                Console.WriteLine("");
            }
        }

        public string GetAlbumID_BySongName(string BandName, string SongName, Napster_BandJsonHandler.All data, int i, int j)
        {
            // Console.WriteLine(GetAlbumData(i, data).Tracks[j].Name);
            if (GetAlbumData(i, data).Tracks[j].Name.Contains(SongName))
            {
                //Console.WriteLine($"{GetAlbumData(i, data).Tracks[j].Links.albums.ids[0]} | {GetAlbumData(i, data).Tracks[j].Name}");
                return GetAlbumData(i, data).Tracks[j].Links.albums.ids[0];
            }
            return "Not Found";
        }
        public void ImageURL(string BandName, string SongName)
        {
            string JsonText = webClient.DownloadString($"http://api.napster.com/v2.2/search/verbose?apikey=YTkxZTRhNzAtODdlNy00ZjMzLTg0MWItOTc0NmZmNjU4Yzk4&query={BandName}");
            JsonText = JObject.Parse(JsonText).ToString();
            Napster_BandJsonHandler.All Band_Data = JsonConvert.DeserializeObject<Napster_BandJsonHandler.All>(JsonText);

            string ImageURL = "";
            string AlbumID = "";
            for (int i = 0; i < Band_Data.search.Data.albums.Length; i++)
            {
                for (int j = 0; j < GetAlbumData(i, Band_Data).Tracks.Length; j++)
                {
                    AlbumID = GetAlbumID_BySongName(BandName, SongName, Band_Data, i, j);
                    //Console.WriteLine(AlbumID);
                    if (AlbumID != "Not Found")
                    {
                        ImageURL = $"https://api.napster.com/imageserver/v2/albums/{AlbumID}/images/400x400.jpg";
                        Console.WriteLine(ImageURL);
                    }
                }
            }
        }

        public string GetImageURL(string BandName, string SongName)
        {
            string JsonText = webClient.DownloadString($"http://api.napster.com/v2.2/search/verbose?apikey=YTkxZTRhNzAtODdlNy00ZjMzLTg0MWItOTc0NmZmNjU4Yzk4&query={BandName}");
            JsonText = JObject.Parse(JsonText).ToString();
            Napster_BandJsonHandler.All Band_Data = JsonConvert.DeserializeObject<Napster_BandJsonHandler.All>(JsonText);

            string ImageURL = "";//"https://api.napster.com/imageserver/v2/albums/alb.203610935/images/400x400.jpg";
            string AlbumID = "";
            for (int i = 0; i < Band_Data.search.Data.albums.Length; i++)
            {
                for (int j = 0; j < GetAlbumData(i, Band_Data).Tracks.Length; j++)
                {
                    AlbumID = GetAlbumID_BySongName(BandName, SongName, Band_Data, i, j);
                    //Console.WriteLine(AlbumID);
                    if (AlbumID != "Not Found")
                    {
                        ImageURL = $"https://api.napster.com/imageserver/v2/albums/{AlbumID}/images/400x400.jpg";
                        return ImageURL;
                    }
                }
            }
            return "Image Not Found";
        }
    }

    public class Napster_API2
    {
        public class JsonHandler
        {
            public partial class All
            {
                [JsonProperty("meta")]
                public Meta Meta { get; set; }

                [JsonProperty("search")]
                public Search Search { get; set; }
            }

            public partial class Meta
            {
                [JsonProperty("totalCount")]
                public long TotalCount { get; set; }

                [JsonProperty("returnedCount")]
                public long ReturnedCount { get; set; }
            }

            public partial class Search
            {
                [JsonProperty("query")]
                public string Query { get; set; }

                [JsonProperty("data")]
                public Data Data { get; set; }

                [JsonProperty("order")]
                public string[] Order { get; set; }
            }

            public partial class Data
            {
                [JsonProperty("albums")]
                public object[] Albums { get; set; }

                [JsonProperty("artists")]
                public Artist[] Artists { get; set; }

                [JsonProperty("tracks")]
                public Track[] Tracks { get; set; }

                [JsonProperty("playlists")]
                public Playlist[] Playlists { get; set; }
            }

            public partial class Artist
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("href")]
                public Uri Href { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("shortcut")]
                public string Shortcut { get; set; }

                [JsonProperty("amg")]
                public string Amg { get; set; }

                [JsonProperty("blurbs")]
                public object[] Blurbs { get; set; }

                [JsonProperty("albumGroups")]
                public AlbumGroups AlbumGroups { get; set; }

                [JsonProperty("links")]
                public ArtistLinks Links { get; set; }
            }

            public partial class AlbumGroups
            {
                [JsonProperty("singlesAndEPs")]
                public string[] SinglesAndEPs { get; set; }

                [JsonProperty("others")]
                public string[] Others { get; set; }

                [JsonProperty("compilations")]
                public string[] Compilations { get; set; }

                [JsonProperty("main")]
                public string[] Main { get; set; }
            }

            public partial class ArtistLinks
            {
                [JsonProperty("albums")]
                public Albums Albums { get; set; }

                [JsonProperty("images")]
                public Albums Images { get; set; }

                [JsonProperty("posts")]
                public Albums Posts { get; set; }

                [JsonProperty("topTracks")]
                public Albums TopTracks { get; set; }

                [JsonProperty("genres")]
                public Genres Genres { get; set; }

                [JsonProperty("stations")]
                public Genres Stations { get; set; }
            }

            public partial class Albums
            {
                [JsonProperty("href")]
                public string Href { get; set; }
            }

            public partial class Genres
            {
                [JsonProperty("ids")]
                public string[] Ids { get; set; }

                [JsonProperty("href", NullValueHandling = NullValueHandling.Ignore)]
                public Uri Href { get; set; }
            }

            public partial class Playlist
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("modified")]
                public DateTimeOffset Modified { get; set; }

                [JsonProperty("href")]
                public Uri Href { get; set; }

                [JsonProperty("trackCount")]
                public int TrackCount { get; set; }

                [JsonProperty("privacy")]
                public string Privacy { get; set; }

                [JsonProperty("images")]
                public Image[] Images { get; set; }

                [JsonProperty("description")]
                public string Description { get; set; }

                [JsonProperty("favoriteCount")]
                public int FavoriteCount { get; set; }

                [JsonProperty("freePlayCompliant")]
                public bool FreePlayCompliant { get; set; }

                [JsonProperty("links")]
                public PlaylistLinks Links { get; set; }
            }

            public partial class Image
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("url")]
                public Uri Url { get; set; }

                [JsonProperty("contentId")]
                public string ContentId { get; set; }

                [JsonProperty("width")]
                public long? Width { get; set; }

                [JsonProperty("height")]
                public long? Height { get; set; }

                [JsonProperty("isDefault")]
                public bool IsDefault { get; set; }

                [JsonProperty("version")]
                public long Version { get; set; }

                [JsonProperty("imageType")]
                public string ImageType { get; set; }
            }

            public partial class PlaylistLinks
            {
                [JsonProperty("members")]
                public Genres Members { get; set; }

                [JsonProperty("tracks")]
                public Albums Tracks { get; set; }

                [JsonProperty("tags")]
                public Genres Tags { get; set; }

                [JsonProperty("sampleArtists")]
                public Genres SampleArtists { get; set; }
            }

            public partial class Track
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("index")]
                public int Index { get; set; }

                [JsonProperty("disc")]
                public int Disc { get; set; }

                [JsonProperty("href")]
                public Uri Href { get; set; }

                [JsonProperty("playbackSeconds")]
                public int PlaybackSeconds { get; set; }

                [JsonProperty("isExplicit")]
                public bool IsExplicit { get; set; }

                [JsonProperty("isStreamable")]
                public bool IsStreamable { get; set; }

                [JsonProperty("isAvailableInHiRes")]
                public bool IsAvailableInHiRes { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("isrc")]
                public string Isrc { get; set; }

                [JsonProperty("shortcut")]
                public string Shortcut { get; set; }

                [JsonProperty("blurbs")]
                public object[] Blurbs { get; set; }

                [JsonProperty("artistId")]
                public string ArtistId { get; set; }

                [JsonProperty("artistName")]
                public string ArtistName { get; set; }

                [JsonProperty("albumName")]
                public string AlbumName { get; set; }

                [JsonProperty("formats")]
                public Format[] Formats { get; set; }

                [JsonProperty("losslessFormats")]
                public Format[] LosslessFormats { get; set; }

                [JsonProperty("albumId")]
                public string AlbumId { get; set; }

                [JsonProperty("isAvailableInAtmos")]
                public bool IsAvailableInAtmos { get; set; }

                [JsonProperty("contributors")]
                public Contributors Contributors { get; set; }

                [JsonProperty("links")]
                public TrackLinks Links { get; set; }

                [JsonProperty("previewURL")]
                public Uri PreviewUrl { get; set; }
            }

            public partial class Contributors
            {
                [JsonProperty("primaryArtist")]
                public string PrimaryArtist { get; set; }
            }

            public partial class Format
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("bitrate")]
                public long Bitrate { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("sampleBits")]
                public long SampleBits { get; set; }

                [JsonProperty("sampleRate")]
                public long SampleRate { get; set; }
            }

            public partial class TrackLinks
            {
                [JsonProperty("artists")]
                public Genres Artists { get; set; }

                [JsonProperty("albums")]
                public Genres Albums { get; set; }

                [JsonProperty("genres")]
                public Genres Genres { get; set; }

                [JsonProperty("tags")]
                public Genres Tags { get; set; }
            }
        }

        WebClient webClient = new WebClient();
        static string API_KEY = "ZGNlMzg0ZTYtZjQyYS00NWQ1LWI1OTEtZmVmYzRkZDhlYjAx";

        public JsonHandler.All GetSongData(string BandName, string SongName)
        {
            string AlbumDetailsLinkByID = $"http://api.napster.com/v2.2/search?apikey={API_KEY}&query={BandName}%20{SongName}";
            string AlbumJsonText = JObject.Parse(webClient.DownloadString(AlbumDetailsLinkByID)).ToString();
            JsonHandler.All AlbumData = JsonConvert.DeserializeObject<JsonHandler.All>(AlbumJsonText);
            return AlbumData;
        }
        public JsonHandler.All GetSongData(string SearchQuery)
        {
            string AlbumDetailsLinkByID = $"http://api.napster.com/v2.2/search?apikey={API_KEY}&query={SearchQuery}";
            string AlbumJsonText = JObject.Parse(webClient.DownloadString(AlbumDetailsLinkByID)).ToString();
            JsonHandler.All AlbumData = JsonConvert.DeserializeObject<JsonHandler.All>(AlbumJsonText);
            return AlbumData;
        }

        public string GetImageURL(string BandName, string SongName, Size size)
        {
            int Count = GetSongData(BandName, SongName).Search.Data.Tracks.Length;
            if (Count > 0)
            {
                string albumID = GetSongData(BandName, SongName).Search.Data.Tracks[0].AlbumId;
                return $"https://api.napster.com/imageserver/v2/albums/{albumID}/images/{size.Width}x{size.Height}.jpg";
            }
            else
            {
                return "Image Not Found";
            }
        }


        public string GetSongName(string SearchQuery)
        {
            int Count = GetSongData(SearchQuery).Search.Data.Tracks.Length;
            if (Count > 0)
            {
                return GetSongData(SearchQuery).Search.Data.Tracks[0].Name;
            }
            return "Not Found";
        }
        public string GetArtistName(string SearchQuery)
        {
            int Count = GetSongData(SearchQuery).Search.Data.Tracks.Length;

            if (Count > 0)
            {
                return GetSongData(SearchQuery).Search.Data.Tracks[0].ArtistName;
            }
            return "Not Found";
        }
        public string GetAlbumName(string SearchQuery)
        {
            int Count = GetSongData(SearchQuery).Search.Data.Tracks.Length;

            if (Count > 0)
            {
                return GetSongData(SearchQuery).Search.Data.Tracks[0].AlbumName;
            }
            return "Not Found";
        }
        public int GetSongDuration(string SearchQuery)
        {
            int Count = GetSongData(SearchQuery).Search.Data.Tracks.Length;
            if (Count > 0)
            {
                return GetSongData(SearchQuery).Search.Data.Tracks[0].PlaybackSeconds;
            }
            return -1;
        }


        public void DataTest()
        {
            Console.WriteLine(GetImageURL("Adept", "Black Veins", new Size(400, 400)));
        }
    }
}

